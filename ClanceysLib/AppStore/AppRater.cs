// 
//  Copyright 2011  James Clancey
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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
namespace ClanceysLib
{
	public static class AppRater
	{
		static NSUserDefaults settings = new NSUserDefaults("appRater");
		public static int RunCountNeeded = 5;
		public static int DaysInstalledCountNeeded = 1;
		static string url =  @"itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=" + AppId;
		static string AppId = "";
		public static void AppLaunched(string appId)
		{
			AppId = appId;
			var version = NSBundle.MainBundle.InfoDictionary.ObjectForKey(new NSString("CFBundleVersion")).ToString();
			if(settings.StringForKey("lastInstalledVersion") != version)
			{
				ResetWarningIndicators();
				settings.SetString(version,"lastInstalledVersion");
				settings.Synchronize();
			}
			TryToRate();
			RunCount += 1;
			Console.WriteLine("runcount" + RunCount);
			
			
		}
		public static void DidSomethingSignificant()
		{
			TryToRate();
			RunCount += 1;
		}
		static void ResetWarningIndicators()
		{
			RunCount = 0;
			DateVersionInstalled = DateTime.UtcNow;
			ShouldRateThisVersion = true;
			DidRate = false;
		}
		static int RunCount {
			get{return settings.IntForKey("runCount");}
			set {settings.SetInt(value,"runCount");
				settings.Synchronize();
			}
		}
		static DateTime DateVersionInstalled
		{
			get{return DateTime.Parse(settings.StringForKey("dateInstalled"));}
			set{settings.SetString(value.ToString(),"dateInstalled");
				settings.Synchronize();}
		}
		static bool ShouldRateThisVersion
		{
			get{return settings.BoolForKey("shouldRate");}
			set{settings.SetBool(value,"shouldRate");}
		}
		static void TryToRate()
		{
			if(ShouldRate())
				Rate();
		}
		public static bool DidRate{
			get{return settings.BoolForKey("didRateVersion");}
			set{settings.SetBool(value,"didRateVersion");
				settings.Synchronize();}
		}
		public static void Rate()
		{
			isRating = true;
			var version = NSBundle.MainBundle.InfoDictionary.ObjectForKey(new NSString("CFBundleVersion"));
			
			var name = NSBundle.MainBundle.InfoDictionary.ObjectForKey(new NSString("CFBundleName")).ToString();
			var alert = new UIAlertView("Rate " + name,"If you enjoyed using " + name +". Will you please take a moment to rate it? Thanks for your support",null,"No, Thanks", "Rate " + name,"Remind Me Later");
			alert.Clicked += delegate(object sender, UIButtonEventArgs e) {
				isRating = false;
				if(e.ButtonIndex == 0)
					ShouldRateThisVersion = false;
				else if(e.ButtonIndex == 1)
				{
					DidRate = true;
					UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
				}
				else if(e.ButtonIndex == 2)
					ResetWarningIndicators();
			};
			alert.Show();
		}
		static bool isRating = false;
		static bool ShouldRate()
		{
			if(isRating)
				return false;
			if(DidRate)
				return false;
			if(!ShouldRateThisVersion)
				return false;
			if((DateTime.UtcNow - DateVersionInstalled).Days < DaysInstalledCountNeeded)
				return false;
			if(RunCount < RunCountNeeded)
				return false;
			return true;
		}
	}
}

