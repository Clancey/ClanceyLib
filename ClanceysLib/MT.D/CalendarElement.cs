// 
//  Copyright 2012  Clancey
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using System.Collections.Generic;

namespace ClanceysLib
{
	public class CalendarDateTimeElement : StringElement {
		public enum PickerTypes {
			DatePicker,Calendar	
		}
			
		public static DateTime DateTimeMin
		{
			get{return DateTime.FromFileTimeUtc(0);}
		}
		
		
		public static DateTime NSDateToDateTime(MonoTouch.Foundation.NSDate date)
		{
			var nsDateNow = (DateTime)NSDate.Now;
			var diff = DateTime.Now.Subtract(nsDateNow);
			var newDate = ((DateTime)date).Add(diff);
		   	return newDate;
			// return (new DateTime(2001,1,1,0,0,0)).AddSeconds(date.SecondsSinceReferenceDate);
		}
		public static NSDate DateTimeToNSDate(DateTime date)
		{
			//var nsDateNow = (DateTime)NSDate.Now;
			//var diff = DateTime.Now.Subtract(nsDateNow);
			//var newDate = (NSDate)date.Add(-diff);
			var newDate = (NSDate)date;
			return newDate;
		}
		public PickerTypes PickerType;
		public DateTime DateValue;
		public UIDatePicker datePicker;
		public CalendarMonthView calView;
		private UIBarButtonItem leftOld;
		private UIBarButtonItem rightOld;
		private UIBarButtonItem switchButon;
		private UIBarButtonItem doneButton;
		public bool DisableSwitching;
		public NSAction DoneEditing;
		public DateSelected OnDateSelected;
		public bool closeOnSelect;
		protected internal NSDateFormatter fmt = new NSDateFormatter () {
			DateStyle = NSDateFormatterStyle.Short
		};
		
		public CalendarDateTimeElement (string caption, DateTime date) : base (caption)
		{
			PickerType  = PickerTypes.Calendar;
			DateValue = date;
			Value = DateValue.ToShortDateString();
		}	
				
		public CalendarDateTimeElement (string caption, DateTime date, PickerTypes pickerType) : base (caption)
		{
			PickerType = pickerType;
			DateValue = date;
			Value = DateValue.ToShortDateString();
		}	
		
		public override UITableViewCell GetCell (UITableView tv)
		{
			if (DateValue.Year < 2000)
				Value = "No Due Date";
			else
				Value = FormatDate(DateValue);
			var cell =  base.GetCell (tv);
			
			
			return cell;
		}
 
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (disposing){
				if (fmt != null){
					fmt.Dispose ();
					fmt = null;
				}
				if (datePicker != null){
					datePicker.Dispose ();
					datePicker = null;
				}
			}
		}
		
		public virtual string FormatDate (DateTime dt)
		{
			return fmt.ToString (dt) + " " + dt.ToLocalTime ().ToShortTimeString ();
		}
		
		public virtual UIDatePicker CreatePicker ()
		{
			var picker = new UIDatePicker (RectangleF.Empty){
				AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
				Mode = UIDatePickerMode.Date,
				Date = DateValue
			};
			return picker;
		}
	
		static RectangleF PickerFrameWithSize (SizeF size)
		{
			var screenRect = UIScreen.MainScreen.ApplicationFrame;
			float fY = 0, fX = 0;
			
			switch (UIApplication.SharedApplication.StatusBarOrientation){
			case UIInterfaceOrientation.LandscapeLeft:
			case UIInterfaceOrientation.LandscapeRight:
				fX = (screenRect.Height - size.Width) /2;
				fY = (screenRect.Width - size.Height) / 2 -17;
				break;
				
			case UIInterfaceOrientation.Portrait:
			case UIInterfaceOrientation.PortraitUpsideDown:
				fX = (screenRect.Width - size.Width) / 2;
				fY = (screenRect.Height - size.Height) / 2 - 25;
				break;
			}
			
			return new RectangleF (fX, fY, size.Width, size.Height);
		}
		
		public override void Deselected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			//tableView.DeselectRow(path,true);
			if (datePicker != null) 
			{
				this.DateValue = (DateTime)datePicker.Date;
				SlideDown(dvc,tableView,path);
			}
			if (calView != null)
				CloseCalendar(dvc,tableView,path);
			
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			if (DisableSwitching)
				ShowDatePicker(dvc,tableView,path);			
			else if (PickerType == PickerTypes.Calendar)
				ShowCalendar(dvc,tableView,path);
			else
				ShowDatePicker(dvc,tableView,path);
			
			
		}
		
		private void ShowCalendar(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			if (calView == null)
			{
				calView = new CalendarMonthView(DateValue,true){					
					AutoresizingMask = UIViewAutoresizing.FlexibleWidth };
				//calView.ToolbarColor = dvc.SearchBarTintColor;
				calView.SizeChanged += delegate {
					RectangleF screenRect =  tableView.Window.Frame;
					
					SizeF pickerSize = calView.Size;
					// compute the end frame
					RectangleF pickerRect = new RectangleF(0.0f,
											screenRect.Y + screenRect.Size.Height - pickerSize.Height,
											pickerSize.Width,
											pickerSize.Height);
					// start the slide up animation
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.3);
					
					// we need to perform some post operations after the animation is complete
					UIView.SetAnimationDelegate(dvc);
					
					calView.Frame = pickerRect;
					
					UIView.CommitAnimations();
						
					
				};
			}
			if (calView.Superview == null)
			{
				if (DateValue.Year < 2010)
					DateValue = DateTime.Today;
				calView.SizeThatFits(SizeF.Empty);
				calView.OnDateSelected += (date) => {					
					DateValue = date;
					if (OnDateSelected != null)
						OnDateSelected(date);
					//Console.WriteLine(String.Format("Selected {0}", date.ToShortDateString()));					
					if(closeOnSelect)
						CloseCalendar(dvc,tableView,path);
				};
				
				
				tableView.Window.AddSubview(calView);
				//dvc.View.Window.AddSubview(datePicker);	
				//
				// size up the picker view to our screen and compute the start/end frame origin for our slide up animation
				//
				// compute the start frame
				RectangleF screenRect =  tableView.Window.Frame;
				
				SizeF pickerSize = calView.Size;
				RectangleF startRect = new RectangleF(0.0f,
										screenRect.Y + screenRect.Size.Height,
										pickerSize.Width, pickerSize.Height);
				calView.Frame = startRect;
				
				// compute the end frame
				RectangleF pickerRect = new RectangleF(0.0f,
										screenRect.Y + screenRect.Size.Height - pickerSize.Height,
										pickerSize.Width,
										pickerSize.Height);
				// start the slide up animation
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.3);
				
				// we need to perform some post operations after the animation is complete
				UIView.SetAnimationDelegate(dvc);
				
				calView.Frame = pickerRect;
				
				// shrink the table vertical size to make room for the date picker
				RectangleF newFrame =  new RectangleF(tableView.Frame.X, tableView.Frame.Y,
											tableView.Frame.Size.Width, tableView.Frame.Size.Height + 55 - calView.Frame.Height) ;
				// newFrame.Size.Height -= datePicker.Frame.Height;
				//tableView.Frame = newFrame;
				UIView.CommitAnimations();
				rightOld = dvc.NavigationItem.RightBarButtonItem;
				//Multi Buttons
				
				// create a toolbar to have two buttons in the right
				UIToolbar tools = new UIToolbar(new RectangleF(0, 0, 133, 44.01f));
				//tools.TintColor = dvc.SearchBarTintColor;
				// create the array to hold the buttons, which then gets added to the toolbar
				List<UIBarButtonItem> buttons = new List<UIBarButtonItem>(3);

				// create switch button
				switchButon = new UIBarButtonItem("Switch",UIBarButtonItemStyle.Bordered,delegate{
					CloseCalendar(dvc,tableView,path);
					ShowDatePicker(dvc,tableView,path);
				});
				
				buttons.Add(switchButon);
				
				// create a spacer
				UIBarButtonItem spacer = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace,null,null);
				buttons.Add(spacer);
				
				//create done button
				doneButton = new UIBarButtonItem("Done",UIBarButtonItemStyle.Done, delegate{
					//DateValue = calView.CurrentDate;
					CloseCalendar(dvc,tableView,path);
				});
				buttons.Add(doneButton);

				tools.SetItems(buttons.ToArray(),false);

				//
				dvc.NavigationItem.RightBarButtonItem = new UIBarButtonItem(tools);

				leftOld = dvc.NavigationItem.LeftBarButtonItem;
				dvc.NavigationItem.LeftBarButtonItem =  new UIBarButtonItem("None",UIBarButtonItemStyle.Bordered, delegate{
					DateValue = DateTime.MinValue;
					if (OnDateSelected != null)
						OnDateSelected(CalendarDateTimeElement.DateTimeMin);
					CloseCalendar(dvc,tableView,path);
				});
				// add the "Done" button to the nav bar
				//dvc.NavigationItem.SetRightBarButtonItem(doneButton,true);
				//
			}
		}
		
		private void CloseCalendar(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			tableView.ReloadRows(new NSIndexPath[]{path},UITableViewRowAnimation.None);
			RectangleF screenRect =  dvc.View.Window.Frame;
			RectangleF endFrame = new RectangleF( calView.Frame.X,calView.Frame.Y + screenRect.Size.Height,
			                                     calView.Frame.Size.Width,calView.Frame.Size.Height);
			//endFrame.origin.y = screenRect.Y + screenRect.Size.Height;
			
			// start the slide down animation
			UIView.BeginAnimations(null);
			UIView.SetAnimationDuration(0.3);
			
			// we need to perform some post operations after the animation is complete
			UIView.SetAnimationDelegate(dvc);
			//UIView.SetAnimationDidStopSelector(slideDownDidStop());
			
			calView.Frame = endFrame;
			UIView.CommitAnimations();
			
			// remove the "Done" button in the nav bar
			dvc.NavigationItem.RightBarButtonItem = rightOld;
			dvc.NavigationItem.LeftBarButtonItem = leftOld;
			
			// deselect the current table row
			tableView.DeselectRow(path,true); 
			calView.RemoveFromSuperview();	
			calView = null;
			if (DoneEditing != null)
				DoneEditing();
		}
		
		private void ShowDatePicker(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			if (datePicker == null)
				datePicker = CreatePicker();
			if (datePicker.Superview == null)
			{
				if (DateValue.Year < 2010)
					datePicker.Date = DateTime.Today;
				else
					datePicker.Date = DateValue ;
				datePicker.MinimumDate = new DateTime(2010,1,1);
				tableView.Window.AddSubview(datePicker);
				//dvc.View.Window.AddSubview(datePicker);	
			    //
				// size up the picker view to our screen and compute the start/end frame origin for our slide up animation
				//
				// compute the start frame
				RectangleF screenRect =  tableView.Window.Frame;
				
				SizeF pickerSize = datePicker.Frame.Size;
				RectangleF startRect = new RectangleF(0.0f,
				                      screenRect.Y + screenRect.Size.Height,
				                      pickerSize.Width, pickerSize.Height);
				datePicker.Frame = startRect;
				
				// compute the end frame
				RectangleF pickerRect = new RectangleF(0.0f,
				                       screenRect.Y + screenRect.Size.Height - pickerSize.Height,
				                       pickerSize.Width,
				                       pickerSize.Height);
				// start the slide up animation
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.3);
				
				// we need to perform some post operations after the animation is complete
				UIView.SetAnimationDelegate(dvc);
				
				datePicker.Frame = pickerRect;
				
				UIView.CommitAnimations();
				rightOld = dvc.NavigationItem.RightBarButtonItem;
				
				
				//create done button
				doneButton = new UIBarButtonItem("Done",UIBarButtonItemStyle.Done, delegate{
					SlideDown(dvc,tableView,path);
				});
				// create a toolbar to have two buttons in the right
				
				if(DisableSwitching)
					dvc.NavigationItem.RightBarButtonItem = doneButton;
				else 
				{
					UIToolbar tools = new UIToolbar(new RectangleF(0, 0, 133, 44.01f));
				//	tools.TintColor = dvc.SearchBarTintColor;
				// create the array to hold the buttons, which then gets added to the toolbar
					List<UIBarButtonItem> buttons = new List<UIBarButtonItem>(3);

				// create switch button
					switchButon = new UIBarButtonItem("Switch",UIBarButtonItemStyle.Bordered,delegate{
						SlideDown(dvc,tableView,path);
						ShowCalendar(dvc,tableView,path);
					});
				
					buttons.Add(switchButon);
					
					// create a spacer
					UIBarButtonItem spacer = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace,null,null);
					buttons.Add(spacer);
					
					buttons.Add(doneButton);
	
					tools.SetItems(buttons.ToArray(),false);
	
					//
					dvc.NavigationItem.RightBarButtonItem = new UIBarButtonItem(tools);
				}
				

				leftOld = dvc.NavigationItem.LeftBarButtonItem;
				dvc.NavigationItem.LeftBarButtonItem =  new UIBarButtonItem("None",UIBarButtonItemStyle.Bordered, delegate{
					datePicker.Date = DateTime.MinValue;
					SlideDown(dvc,tableView,path);
					if (OnDateSelected != null)
						OnDateSelected(CalendarDateTimeElement.DateTimeMin);
				});
				// add the "Done" button to the nav bar
				//dvc.NavigationItem.SetRightBarButtonItem(doneButton,true);
				//
			}
		}
		private void SlideDown(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			
			this.DateValue = datePicker.Date;
			tableView.ReloadRows(new NSIndexPath[]{path},UITableViewRowAnimation.None);
			RectangleF screenRect =  dvc.View.Window.Frame;
			RectangleF endFrame = new RectangleF( datePicker.Frame.X,datePicker.Frame.Y + screenRect.Size.Height,
			                                     datePicker.Frame.Size.Width,datePicker.Frame.Size.Height);
			//endFrame.origin.y = screenRect.Y + screenRect.Size.Height;
			
			// start the slide down animation
			UIView.BeginAnimations(null);
			UIView.SetAnimationDuration(0.3);
			
			// we need to perform some post operations after the animation is complete
			UIView.SetAnimationDelegate(dvc);
			//UIView.SetAnimationDidStopSelector(slideDownDidStop());
			
			datePicker.Frame = endFrame;
			UIView.CommitAnimations();
			
			// remove the "Done" button in the nav bar
			dvc.NavigationItem.RightBarButtonItem = rightOld;
			dvc.NavigationItem.LeftBarButtonItem = leftOld;
			
			// deselect the current table row
			tableView.DeselectRow(path,true); 
			datePicker.RemoveFromSuperview();	
			datePicker = null;
			if (DoneEditing != null)
				DoneEditing();
			
		}
		
	}
	
	
	public class CalendarElement : CalendarDateTimeElement {
		public CalendarElement (string caption, DateTime date) : base (caption, date)
		{
			fmt.DateStyle = NSDateFormatterStyle.Medium;
		}
		
		public override string FormatDate (DateTime dt)
		{
			return fmt.ToString (dt);
		}
		
		public override UIDatePicker CreatePicker ()
		{
			var picker = base.CreatePicker ();
			picker.Mode = UIDatePickerMode.Date;
			return picker;
		}
	}
	
	public class CalendarTimeElement : CalendarDateTimeElement {
		public CalendarTimeElement (string caption, DateTime date) : base (caption,date)
		{
		}
		
		public override string FormatDate (DateTime dt)
		{
			return dt.ToLocalTime().ToShortTimeString ();
		}
		
		public override UIDatePicker CreatePicker ()
		{
			var picker = base.CreatePicker ();
			picker.Mode = UIDatePickerMode.Time;
			return picker;
		}
	}
	
}

