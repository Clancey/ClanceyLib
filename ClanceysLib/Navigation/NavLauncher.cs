using System;
using MonoTouch.Foundation;
using MonoTouch;
using System.Drawing;
using System.Collections;
using MonoTouch.UIKit;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
namespace ClanceysLib
{
	public class NavLauncher : UIViewController
	{
		public List<NavPage> Pages { get; set; }
		public int Columns = 3;
		public int Rows = 5;
		public float spacing = 10;
		public UIView MainView;
		private UIScrollView scrollView;
		private UIPageControl pageControl;
		private float pageControlH = 30;
		private UIResponder topModal;
		public UIBarButtonItem LeftButton;
		public UIBarButtonItem RightButton;
		public double AnimationSpeed = 0.3;

		public NavLauncher () : base()
		{
			var mainFrame = this.View.Bounds;
			// Navbar 
			mainFrame.Height -= 44;
			MainView = new UIView (mainFrame);
			var scrollRect = MainView.Frame;
			scrollRect.Height -= pageControlH;
			scrollView = new UIScrollView (scrollRect);
			scrollView.ShowsHorizontalScrollIndicator = false;
			scrollView.ShowsVerticalScrollIndicator = false;
			scrollView.Scrolled += ScrollViewScrolled;
			scrollView.PagingEnabled = true;
			var pageRect = new RectangleF (0, scrollRect.Height, scrollRect.Width, pageControlH);
			pageControl = new UIPageControl (pageRect);
			pageControl.BackgroundColor = UIColor.Black;			
			pageControl.TouchUpInside += HandlePageControlTouchUpInside;
			this.View.AddSubview (MainView);
			MainView.AddSubview (scrollView);
			MainView.AddSubview (pageControl);
			//scrollView.BackgroundColor = UIColor.Gray;
		}

		public override void ViewWillAppear (bool animated)
		{
			if(RefreshPages != null)
			{
				RefreshPages();
			}
			CreatePanels ();
		}
		public Action RefreshPages {get;set;}

		public void LaunchModal (UIResponder responder)
		{
			if(closing || Adding)
				return;
			Adding = true;
			topModal = responder;
			if (responder is UIView)
			{
				var modal = responder as UIView;
				modal.Frame = MainView.Frame;
				
				AddViewToScreen (modal);
			}
			else if (responder is UIViewController)
			{					
				AddViewControllerToScreen ();
			}
		}
		bool Adding;
		UIImageView tempImage;
		private void AddViewControllerToScreen()
		{
			var vc = topModal as UIViewController;
			UIGraphics.BeginImageContext(View.Frame.Size);
			vc.View.Layer.RenderInContext(UIGraphics.GetCurrentContext());
			UIImage test = UIGraphics.GetImageFromCurrentImageContext();
			tempImage = new UIImageView(test);
			UIGraphics.EndImageContext();
			
			MainView.AddSubview (tempImage);
			tempImage.Transform = CGAffineTransform.MakeScale (0.2f, 0.2f);
			tempImage.Alpha = 0.5f;
			UIView.BeginAnimations ("addModel");
			UIView.SetAnimationDuration (AnimationSpeed);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
			UIView.SetAnimationDelegate (this);
			tempImage.Alpha = 1;
			tempImage.Transform = CGAffineTransform.MakeScale (1f, 1f);			
			UIView.SetAnimationDidStopSelector (new Selector ("fadeInDidFinish"));
			UIView.CommitAnimations ();
		}
		
		[Export("fadeInDidFinish")]
		public void FadeInDidFinish ()
		{		
			var vc = topModal as UIViewController;
			NavigationController.PushViewController(vc,false);
			tempImage.Transform = CGAffineTransform.MakeScale (1f, 1f);
			tempImage.RemoveFromSuperview ();			
			tempImage = null;
			var leftButton = new UIBarButtonItem (UIImage.FromResource (GetType ().Assembly, "ClanceysLib.Images.Home.png"), UIBarButtonItemStyle.Bordered, delegate { CloseModal (); });
			(topModal as UIViewController).NavigationItem.LeftBarButtonItem = leftButton;
			Adding = false;
		}
		
		private void AddViewToScreen (UIView modal)
		{
			MainView.AddSubview (modal);
			modal.Transform = CGAffineTransform.MakeScale (0.2f, 0.2f);
			modal.Alpha = 0.5f;
			UIView.BeginAnimations ("addModel");
			UIView.SetAnimationDuration (AnimationSpeed);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
			modal.Alpha = 1;
			modal.Transform = CGAffineTransform.MakeScale (1f, 1f);
			UIView.CommitAnimations ();
			var leftButton = new UIBarButtonItem (UIImage.FromResource (GetType ().Assembly, "ClanceysLib.Images.Home.png"), UIBarButtonItemStyle.Bordered, delegate { CloseModal (); });
			if (topModal is UIViewController)
				(topModal as UIViewController).NavigationItem.LeftBarButtonItem = leftButton;
			else
				NavigationItem.LeftBarButtonItem = leftButton;
			Adding = false;
		}
		bool closing;
		public void CloseModal ()
		{
			if(closing || topModal == null)
				return;	
			closing = true;
			NavigationItem.LeftBarButtonItem = null;
			
			var tb = new UITextField(new RectangleF(0,-40,40,40));
			if (topModal is UIViewController)
			{			
				var vcv = (topModal as UIViewController).View;
				vcv.AddSubview(tb);
				tb.BecomeFirstResponder();
				tb.ResignFirstResponder();
				this.NavigationController.PopViewControllerAnimated (false);
				MainView.AddSubview (vcv);
				topModal = vcv;
				
			}
			var modal = topModal as UIView;
			
			modal.AddSubview(tb);
			tb.BecomeFirstResponder();
			tb.ResignFirstResponder();
			UIView.BeginAnimations ("closeModel");
			UIView.SetAnimationDuration (AnimationSpeed);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseIn);
			UIView.SetAnimationDelegate (this);
			UIView.SetAnimationDidStopSelector (new Selector ("fadeOutDidFinish"));
			modal.Transform = CGAffineTransform.MakeScale (0.2f, 0.2f);
			modal.Alpha = 0.5f;
			UIView.CommitAnimations ();
			
		}

		[Export("fadeOutDidFinish")]
		public void FadeOutDidFinish ()
		{
			
			var	modal = (UIView)topModal;
			modal.Transform = CGAffineTransform.MakeScale (1f, 1f);
			modal.RemoveFromSuperview ();			
			topModal = null;			
			closing = false;
		}

		private void CreatePanels ()
		{
			ClearScrollView();
			int count = 0;
			RectangleF scrollFrame = scrollView.Frame;
			scrollFrame.Width = scrollFrame.Width * Pages.Count;
			scrollView.ContentSize = scrollFrame.Size;
			
			NavigationItem.LeftBarButtonItem = LeftButton;
			NavigationItem.RightBarButtonItem = RightButton;
			
			
			foreach (var page in Pages)
			{
				RectangleF frame = scrollView.Frame;
				PointF location = new PointF ();
				location.X = frame.Width * count;
				
				frame.Location = location;
				page.parent = this;
				page.Frame = frame;
				page.Columns = Columns;
				page.Rows = Rows;
				
				if (page.Superview != scrollView)
					scrollView.AddSubview (page);
				page.Refresh ();
				
				Console.WriteLine ("Page " + count + ": " + frame.X);
				count += 1;
			}
			
			pageControl.Pages = count;
		}
		private void ClearScrollView()
		{
			foreach(var view in scrollView.Subviews)
			{
				view.RemoveFromSuperview();	
			}
		}
		
		private void ScrollViewScrolled (object sender, EventArgs e)
		{
			double page = Math.Floor ((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;
			pageControl.CurrentPage = (int)page;
		}

		void HandlePageControlTouchUpInside (object sender, EventArgs e)
		{
			RectangleF toRect = new RectangleF (scrollView.Frame.Width * pageControl.CurrentPage, scrollView.Frame.Y, scrollView.Frame.Width, scrollView.Frame.Height);
			scrollView.ScrollRectToVisible (toRect, true);
		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return false;
		}
		//
	}
}

