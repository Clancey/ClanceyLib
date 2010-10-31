
using System;
using System.IO;
using SQLite;
using ClanceysLib;
namespace ClanceySamples
{	
	public class Database : SQLiteConnection {
		internal Database (string file) : base (file)
		{
			//Util.ReportTime ("Database init");
			CreateTable<NavIcon> ();
			
			//CreateTable<User> ();
			//Util.ReportTime ("Database finish");
		}
		
		static Database ()
		{
			// For debugging
			var sampleDb = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "..") + "/Documents/sample.db";
			//System.IO.File.Delete (collaboratedb);
			Main = new Database (sampleDb);
		}
		
		static public Database Main { get; private set; }
	}
	
	public class ReturnCount
	{
		public int Count {get;set;}	
	}
}

