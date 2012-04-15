
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ClanceysLib;
using System.Drawing;
using MonoTouch.Dialog;
using System.Reflection;
using MonoTouch.MediaPlayer;

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
			//var version = NSBundle.MainBundle.InfoDictionary.ObjectForKey(new NSString("CFBundleName")).ToString();
			AppRater.AppLaunched("myappid");
			var launcher = new NavLauncher ();
			launcher.Pages = new List<NavPage>(){
				new NavPage(3,3)
				{
					Icons = new List<NavIcon>{
						new NavIcon()
						{
							Image = Images.Contacts,
							Title = "Test Label",
							NotificationCount = 10,
							ModalView = delegate() { 
								return new TestView(new RectangleF(100,100,100,100),"Label Text"); },
						},
						new NavIcon()
						{
							Image = Images.Favorites,
							Title = "Stack Panel",
							ModalView = delegate() 
							{
								return new 	StackPanelView(this.window.Frame );
							}
						},
						new NavIcon()
						{
							Image = Images.History,
							Title = "Calendar",
							ModalView = delegate() {return new CalendarView();}
						},
						new NavIcon()
						{
							Image = Images.Most,
							Title = "MT.D",
							ModalView = delegate()
							{
								return new DialogViewController(DemoElementApi());	
							}
						},
						new NavIcon()
						{
							Image = Images.Favorites,
							Title = "Datagrid",
							ModalView = delegate()
							{
								return new DataGrid(this.window.Frame);	
							}
							
						},
						new NavIcon()
						{
							Image = Images.Recent,
							Title = "Play movie",
							ModalView = delegate()
							{
								MPMoviePlayerController mp = new MPMoviePlayerController(new NSUrl("http://content.mp3tunes.com/storage/lockerplay/be266176d810daf35bdc2cc6469da98c/?sid=92ba9ce91039f0e2b1d46bbd3d0714cb&partner_token=1159283925"));
								mp.ShouldAutoplay = true;
								mp.Fullscreen = true;
								return mp.View;
								//moviePlayer.Play();
								//return moviePlayer;
								
							}
						}
					}
				},
				new NavPage(3,3)
				{
					Icons = new List<NavIcon>{
					
						new NavIcon()
						{
							Image = Images.Recent,
							Title = "nothing"
						},
					}
					
				}
				
			};			
			navigationController = new UINavigationController ();
			navigationController.PushViewController (launcher, false);
			window.AddSubview (navigationController.View);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
			var types = typeof(UIView).Assembly.GetTypes().Where(x=> x.IsSubclassOf(typeof(UIView)) && x.Namespace == "MonoTouch.UIKit").Select(x=> x.Name).Aggregate((current,next) => current + "," + next);
			types.Split(new char[]{char.Parse(",")});
			Console.WriteLine(types);
		}
	}
}

