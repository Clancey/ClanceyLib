
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ClanceysLib;
using System.Drawing;

namespace ClanceySamples
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		public NavLauncher launcher;
		public UINavigationController navigationController;
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// If you have defined a view, add it here:
			// window.AddSubview (navigationController.View);
			var launcher = new NavLauncher();
			launcher.Pages = new List<NavPage>(){
				new NavPage()
				{
					Icons = new List<NavIcon>{
						new NavIcon()
						{
							Image = Images.Contacts,
							Title = "Test Label",
							ModalName = "ClanceySamples.TestView,ClanceySamples",
							ModalParameters = new object[]{new RectangleF(100,100,100,100),"Label Text"},
						},
						new NavIcon()
						{
							Image = Images.Favorites,
							Title = "Favorites"
						},
						new NavIcon()
						{
							Image = Images.Featured,
							Title = "Featured"
						},
						new NavIcon()
						{
							Image = Images.History,
							Title = "Calendar",
							ModalName = "ClanceySamples.CalendarView,ClanceySamples",
						},
					}
				},
				new NavPage()
				{
					Icons = new List<NavIcon>{
						new NavIcon()
						{
							Image = Images.Most,
							Title = "Most"
						},
						new NavIcon()
						{
							Image = Images.Organize,
							Title = "Organize"
						},
						new NavIcon()
						{
							Image = Images.Recent,
							Title = "Recent"
						},
					}
					
				}
				
			};
			
			navigationController = new UINavigationController();
			navigationController.PushViewController(launcher,false);
			window.AddSubview(navigationController.View);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
}

