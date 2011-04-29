using System;
using System.Linq;
using System.Collections;
using MonoTouch.AddressBook;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.AddressBookUI;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
namespace ClanceysLib
{
	public static class AddressBook
	{
		public static ABPeoplePickerNavigationController picker;
		public static ABPersonViewController personViewer;
		public static ABNewPersonViewController newPersonVc;
		public static DialogViewController dvc;
		public static void PickContact (UIViewController view, Action<ABPerson> picked)
		{
			/*
			ABAddressBook ab = new ABAddressBook();
			ABPerson p = new ABPerson();
			
			p.FirstName = "Brittani";
			p.LastName = "Clancey";
			
			ABMutableMultiValue<string> phones = new ABMutableStringMultiValue();
			phones.Add("9079470168", ABPersonPhoneLabel.Mobile);
			
			p.SetPhones(phones);
			
			
			ab.Add(p);
			ab.Save();
			*/			
			
			
			picker = new ABPeoplePickerNavigationController ();
//picker.DismissModalViewControllerAnimated (true);
				//picker.Dispose();
			picker.SelectPerson += delegate(object sender, ABPeoplePickerSelectPersonEventArgs e) { picked (e.Person); };
				
			picker.PerformAction += delegate(object sender, ABPeoplePickerPerformActionEventArgs e) { };
			
			picker.Cancelled += delegate {
				picker.DismissModalViewControllerAnimated (true);
				picked (null);
				picker.Dispose ();
			};
			view.PresentModalViewController (picker, true);
			
			
		}

		public static void PickContactPhoneNumber (UIViewController view, string currentValue, Action<string> picked)
		{
			PickContact (view, person =>
			{
				personViewer = new ABPersonViewController ();
				personViewer.DisplayedPerson = person;
				personViewer.DisplayedProperties.Add (ABPersonProperty.FirstName);
				personViewer.DisplayedProperties.Add (ABPersonProperty.LastName);
				personViewer.DisplayedProperties.Add (ABPersonProperty.Phone);
				personViewer.PerformDefaultAction += delegate(object sender, ABPersonViewPerformDefaultActionEventArgs e) {
					var prop = (ABMultiValue<string>)e.Person.GetProperty (e.Property);
					var phone = prop[e.Identifier.Value].Value;
					picked (phone);
				};
				picker.PushViewController (personViewer, true);
			});
		}

		public static ABPerson[] SearchByPhoneNumber (string phoneNumber)
		{
			List<ABPerson> singlePeople = new List<ABPerson> ();
			phoneNumber = Regex.Replace (phoneNumber, "[^0-9]", "");
			ABAddressBook ab = new ABAddressBook ();
			var people = ab.Where (x => x is ABPerson).Cast<ABPerson> ().Where (x => x.GetPhones ().Where (p => Regex.Replace (p.Value, "[^0-9]", "").Contains (phoneNumber) || phoneNumber.Contains (Regex.Replace (p.Value, "[^0-9]", ""))).Count () > 0).ToArray ();
			foreach (var person in people) {
				if (singlePeople.Intersect (person.GetRelatedNames ().Cast<ABPerson> ()).Count () <= 0)
					singlePeople.Add (person);
				
			}
			return singlePeople.ToArray ();
			;
		}

		public static object[] search (string phoneNumber)
		{
			List<ABPerson> singlePeople = new List<ABPerson> ();
			phoneNumber = Regex.Replace (phoneNumber, "[^0-9]", "");
			ABAddressBook ab = new ABAddressBook ();
			var people = ab.Where (x => x is ABPerson).Cast<ABPerson> ().Where (x => x.GetPhones ().Where (p => Regex.Replace (p.Value, "[^0-9]", "").Contains (phoneNumber) || phoneNumber.Contains (Regex.Replace (p.Value, "[^0-9]", ""))).Count () > 0).ToArray ();
			foreach (var person in people) {
				if (singlePeople.Intersect (person.GetRelatedNames ().Cast<ABPerson> ()).Count () <= 0)
					singlePeople.Add (person);
				
			}
			return singlePeople.ToArray ();
			
		}
		
		
	}

	public class CustomPhonePicker : DialogViewController
	{
		public EntryElement phoneInput;
		public StyledStringElement personPicker;
		public PersonPickerReturnResult SelectedValue;
		ABPeoplePickerNavigationController picker;
		ABPersonViewController personViewer;

		public CustomPhonePicker (string currentValue) : base(null,true)
		{
			SelectedValue = new PersonPickerReturnResult ();
			phoneInput = new EntryElement ("Phone Number", "", currentValue);
			phoneInput.Changed += delegate { SelectedValue.PhoneNumber = phoneInput.Value; };
			
			personPicker = new StyledStringElement ("Chose from Contact");
			personPicker.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			personPicker.Tapped += delegate {
				setupPicker ();
				this.PresentModalViewController (picker, true);
			};
			
			Root = new RootElement ("Phone Picker") { new Section { phoneInput, personPicker } };
		}

		void setupPicker ()
		{
			picker = new ABPeoplePickerNavigationController ();
			picker.Delegate = new PickerDelegate ();
			picker.SelectPerson	+= delegate(object sender, ABPeoplePickerSelectPersonEventArgs e) {
				personViewer.DisplayedPerson = e.Person;
				picker.PushViewController (personViewer, true);
			};
			picker.PerformAction += delegate(object sender, ABPeoplePickerPerformActionEventArgs e) {
				var prop = (ABMultiValue<string>)e.Person.GetProperty (e.Property);
				var phone = prop[e.Identifier.Value].Value;
				SelectedValue.PhoneNumber = phone;
				SelectedValue.Person = e.Person;
				picker.DismissModalViewControllerAnimated (true);
				this.NavigationController.PopViewControllerAnimated (false);
			};
			
			picker.Cancelled += delegate {
				picker.DismissModalViewControllerAnimated (true);
				this.NavigationController.PopViewControllerAnimated (false);
				
			};
			
			personViewer = new ABPersonViewController ();
			personViewer.DisplayedProperties.Add (ABPersonProperty.FirstName);
			personViewer.DisplayedProperties.Add (ABPersonProperty.LastName);
			personViewer.DisplayedProperties.Add (ABPersonProperty.Phone);
			personViewer.PerformDefaultAction += delegate(object sender, ABPersonViewPerformDefaultActionEventArgs e) {
				var prop = (ABMultiValue<string>)e.Person.GetProperty (e.Property);
				var phone = prop[e.Identifier.Value].Value;
				SelectedValue.PhoneNumber = phone;
				SelectedValue.Person = e.Person;
				picker.DismissModalViewControllerAnimated (true);
				this.NavigationController.PopViewControllerAnimated (false);
			};
		}

		public class PersonPickerReturnResult
		{
			public ABPerson Person;
			public string PhoneNumber;
		}
	}
	
	public class CustomEmailPicker : DialogViewController
	{
		public EntryElement emailInput;
		public StyledStringElement personPicker;
		public PersonPickerReturnResult SelectedValue;
		ABPeoplePickerNavigationController picker;
		ABPersonViewController personViewer;

		public CustomEmailPicker (string currentValue) : base(null,true)
		{
			SelectedValue = new PersonPickerReturnResult{Email = currentValue};
			emailInput = new EntryElement ("Email", "", currentValue);
			emailInput.Changed += delegate { SelectedValue.Email = emailInput.Value; };
			
			personPicker = new StyledStringElement ("Chose from Contact");
			personPicker.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			personPicker.Tapped += delegate {
				setupPicker ();
				this.PresentModalViewController (picker, true);
			};
			
			Root = new RootElement ("Email Picker") { new Section { emailInput, personPicker } };
		}

		void setupPicker ()
		{
			picker = new ABPeoplePickerNavigationController ();
			picker.Delegate = new PickerDelegate ();
			picker.SelectPerson	+= delegate(object sender, ABPeoplePickerSelectPersonEventArgs e) {
				personViewer.DisplayedPerson = e.Person;
				picker.PushViewController (personViewer, true);
			};
			picker.PerformAction += delegate(object sender, ABPeoplePickerPerformActionEventArgs e) {
				var prop = (ABMultiValue<string>)e.Person.GetProperty (e.Property);
				var email = prop[e.Identifier.Value].Value;
				SelectedValue.Email = email;
				SelectedValue.Person = e.Person;
				picker.DismissModalViewControllerAnimated (true);
				this.NavigationController.PopViewControllerAnimated (false);
			};
			
			picker.Cancelled += delegate {
				picker.DismissModalViewControllerAnimated (true);
				this.NavigationController.PopViewControllerAnimated (false);
				
			};
			
			personViewer = new ABPersonViewController ();
			personViewer.DisplayedProperties.Add (ABPersonProperty.FirstName);
			personViewer.DisplayedProperties.Add (ABPersonProperty.LastName);
			personViewer.DisplayedProperties.Add (ABPersonProperty.Email);
			personViewer.PerformDefaultAction += delegate(object sender, ABPersonViewPerformDefaultActionEventArgs e) {
				var prop = (ABMultiValue<string>)e.Person.GetProperty (e.Property);
				var email = prop[e.Identifier.Value].Value;
				SelectedValue.Email = email;
				SelectedValue.Person = e.Person;
				picker.DismissModalViewControllerAnimated (true);
				this.NavigationController.PopViewControllerAnimated (false);
			};
		}

		public class PersonPickerReturnResult
		{
			public ABPerson Person;
			public string Email;
		}
	}
	
	public class PickerDelegate : ABPeoplePickerNavigationControllerDelegate
	{
		public override bool ShouldContinue (ABPeoplePickerNavigationController peoplePicker, IntPtr selectedPerson)
		{
			return true;
		}
		public override bool ShouldContinue (ABPeoplePickerNavigationController peoplePicker, IntPtr selectedPerson, int propertyId, int identifier)
		{
			return true;
		}
	}
	
}

