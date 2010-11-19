using System;
using System.Linq;
using MonoTouch.AddressBook;
using System.Text.RegularExpressions;
namespace ClanceysLib
{
	public static class AddressBook
	{
		public static ABPerson[] SearchByPhoneNumber(string phoneNumber)
		{
			phoneNumber = Regex.Replace(phoneNumber,"[^0-9]", "");
			ABAddressBook ab = new ABAddressBook(); 
			var people = ab.Where(x=> x is ABPerson).Cast<ABPerson>().Where(x=> x.GetPhones().Where(p=> Regex.Replace(p.Value,"[^0-9]", "").Contains(phoneNumber) ).Count() > 0).ToArray();
			return people;
		}
	}
}

