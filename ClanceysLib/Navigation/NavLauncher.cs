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
		private UIView MainView;
		private UIScrollView scrollView;
		private UIPageControl pageControl;
		private float pageControlH = 30;
		private UIResponder topModal;
		public UIBarButtonItem LeftButton;
		public UIBarButtonItem RightButton;


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
			topModal = responder;
			if (responder is UIView)
			{
				var modal = responder as UIView;
				modal.Frame = MainView.Frame;
				
				AddViewToScreen (modal);
			}
			else if (responder is UIViewController)
			{
				var vc = responder as UIViewController;
				AddViewToScreen (vc.View);
				this.NavigationController.PushViewController ((UIViewController)topModal, false);
			}
		}

		private void AddViewToScreen (UIView modal)
		{
			MainView.AddSubview (modal);
			modal.Transform = CGAffineTransform.MakeScale (0.2f, 0.2f);
			modal.Alpha = 0.5f;
			UIView.BeginAnimations ("addModel");
			UIView.SetAnimationDuration (0.5);
			modal.Alpha = 1;
			modal.Transform = CGAffineTransform.MakeScale (1f, 1f);
			UIView.CommitAnimations ();
			var leftButton = new UIBarButtonItem (UIImage.FromResource (GetType ().Assembly, "ClanceysLib.Images.Home.png"), UIBarButtonItemStyle.Bordered, delegate { CloseModal (); });
			if (topModal is UIViewController)
				(topModal as UIViewController).NavigationItem.LeftBarButtonItem = leftButton;
			else
				NavigationItem.LeftBarButtonItem = leftButton;
		}

		public void CloseModal ()
		{
			NavigationItem.LeftBarButtonItem = null;
			if (topModal == null)
				return;
			if (topModal is UIViewController)
			{
				//Fixes keyboard glitch for mt.d
				if (topModal is MonoTouch.Dialog.DialogViewController)
					(topModal as MonoTouch.Dialog.DialogViewController).FinishSearch ();
				
				var vcv = (topModal as UIViewController).View;
				this.NavigationController.PopViewControllerAnimated (false);
				MainView.AddSubview (vcv);
				topModal = vcv;
			}
			var modal = topModal as UIView;
			UIView.BeginAnimations ("closeModel");
			UIView.SetAnimationDuration (0.5);
			UIView.SetAnimationDelegate (this);
			UIView.SetAnimationDidStopSelector (new Selector ("fadeOutDidFinish"));
			modal.Transform = CGAffineTransform.MakeScale (0.2f, 0.2f);
			modal.Alpha = 0.5f;
			UIView.CommitAnimations ();
		}

		[Export("fadeOutDidFinish")]
		public void FadeOutDidFinish ()
		{
			var modal = topModal as UIView;
			if(modal != null)
			{
				modal.Transform = CGAffineTransform.MakeScale (1f, 1f);
				modal.RemoveFromSuperview ();
			}
			topModal = null;
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

