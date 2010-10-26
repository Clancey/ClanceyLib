using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch;
namespace ClanceysLib
{
[Register("RotatingViewController")]
	public partial class RotatingViewController : UIViewController
	{
		public UIViewController LandscapeLeftViewController {get;set;}
		public UIViewController LandscapeRightViewController {get;set;}
		public UIViewController PortraitViewController {get;set;}

		private NSObject notificationObserver;

		public RotatingViewController (IntPtr handle) : base(handle)
		{
		
		}

		[Export("initWithCoder:")]
		public RotatingViewController (NSCoder coder) : base(coder)
		{
		}

		public RotatingViewController (string nibName, NSBundle bundle) : base(nibName, bundle)
		{
		}

		public RotatingViewController () {}

		public override void ViewWillAppear (bool animated)
		{
			SetView();
		}
		private void _showView(UIView view){
			/*
			if (this.NavigationController!=null)
				NavigationController.SetNavigationBarHidden(view!=PortraitViewController.View, false);
			 */
			_removeAllViews();
			view.Frame = this.View.Frame;
			View.AddSubview(view);

		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}


		public override void ViewDidLoad()
		{
			
		}
		
		public virtual void SetupNavBar()
		{
			//Add you nav bars here
		}
		

		public override void ViewDidAppear (bool animated)
		{
			UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
		}

		public override void ViewWillDisappear (bool animated)
		{
			UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
		}

		private void SetView()
		{
			switch (UIDevice.CurrentDevice.Orientation){

				case  UIDeviceOrientation.Portrait:
					_showView(PortraitViewController.View);
					break;

				case UIDeviceOrientation.LandscapeLeft:
					_showView(LandscapeLeftViewController.View);

					break;
				case UIDeviceOrientation.LandscapeRight:
					_showView(LandscapeRightViewController.View);
					break;
			}
			SetupNavBar();
		}
			
		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			Console.WriteLine("rotated! "+UIDevice.CurrentDevice.Orientation);
			SetView();
		}
		

		private void _removeAllViews(){
			PortraitViewController.View.RemoveFromSuperview();
			LandscapeLeftViewController.View.RemoveFromSuperview();
			LandscapeRightViewController.View.RemoveFromSuperview();
		}
		protected void OnDeviceRotated(){

		}


		public override void ViewDidDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

	}
}

