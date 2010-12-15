using System;
using System.Linq;
using System.Collections;
using MonoTouch.AddressBook;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace ClanceysLib
{
	public static class AddressBook
	{
		public static ABPerson[] SearchByPhoneNumber(string phoneNumber)
		{
			List<ABPerson> singlePeople = new List<ABPerson>();
			phoneNumber = Regex.Replace(phoneNumber,"[^0-9]", "");
			ABAddressBook ab = new ABAddressBook(); 
			var people = ab.Where(x=> x is ABPerson).Cast<ABPerson>().Where(x=> x.GetPhones().Where(p=> Regex.Replace(p.Value,"[^0-9]", "").Contains(phoneNumber)  || phoneNumber.Contains(Regex.Replace(p.Value,"[^0-9]", ""))).Count() > 0).ToArray();
			foreach(var person in people)
			{
				if( singlePeople.Intersect(person.GetRelatedNames().Cast<ABPerson>()).Count() <= 0)
					singlePeople.Add(person);
				
			}
			return singlePeople.ToArray();;
		}
		public static object[] searc(string phoneNumber)
		{
			List<ABPerson> singlePeople = new List<ABPerson>();
				phoneNumber = Regex.Replace(phoneNumber,"[^0-9]", "");
				ABAddressBook ab = new ABAddressBook(); 
				var people = ab.Where(x=> x is ABPerson).Cast<ABPerson>().Where(x=> x.GetPhones().Where(p=> Regex.Replace(p.Value,"[^0-9]", "").Contains(phoneNumber)  || phoneNumber.Contains(Regex.Replace(p.Value,"[^0-9]", ""))).Count() > 0).ToArray();
				foreach(var person in people)
				{
					if( singlePeople.Intersect(person.GetRelatedNames().Cast<ABPerson>()).Count() <= 0)
						singlePeople.Add(person);
					
				}
				return singlePeople.ToArray();
			
		}
	}
}

