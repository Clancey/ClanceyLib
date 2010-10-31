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
		

		public NavLauncher () :base()
		{
			var mainFrame = this.View.Bounds;
			// Navbar 
			mainFrame.Height -= 44;
			MainView = new UIView(mainFrame);
			var scrollRect = MainView.Bounds;
			scrollRect.Height -= pageControlH;			
			scrollView = new UIScrollView (scrollRect);
			scrollView.ShowsHorizontalScrollIndicator = false;
			scrollView.ShowsVerticalScrollIndicator = false;
			var pageRect = new RectangleF(0,scrollRect.Height,scrollRect.Width,pageControlH);
			pageControl = new UIPageControl(pageRect);
			pageControl.BackgroundColor = UIColor.Black;
			this.View.AddSubview(MainView);
			MainView.AddSubview(scrollView);
			MainView.AddSubview (pageControl);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			CreatePanels();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">
		/// The FullName of the View With the Assembly seperated by a ,
		/// ex: "ClanceysLib.NavLauncher,ClanceysLib"
		/// </param>
		/// <param name="parameters">
		/// Object Array to be passed into the constructor
		/// </param>
		public void LaunchModal(UIResponder responder)
		{
			topModal = responder;
			if(responder is UIView)
			{
				var modal = responder as UIView;
				modal.Frame = MainView.Frame;
			
				AddViewToScreen(modal,true);
			}
			else if(responder is UIViewController)
			{
				var vc = responder as UIViewController;
				AddViewControllerToScreen(vc,true);
			}
		}
		
		private void AddViewControllerToScreen(UIViewController modal, bool animate)
		{
			if(animate)
			{
				modal.View.Transform = CGAffineTransform.MakeScale(0.2f,0.2f);
				modal.View.Alpha = 0.5f;
				UIView.BeginAnimations ("addModel"); 
				this.NavigationController.PushViewController(modal,false);
	            UIView.SetAnimationDuration (0.5); 
				UIView.SetAnimationDidStopSelector(new Selector("fadeInDidFinish"));
				modal.View.Alpha = 1;
				modal.View.Transform = CGAffineTransform.MakeScale(1f,1f);
	            UIView.CommitAnimations ();  
			}
			modal.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromResource(GetType().Assembly,"ClanceysLib.Images.Home.png"),UIBarButtonItemStyle.Bordered,delegate{
				
				CloseModalViewController(animate);
			});
		}
		
		private void AddViewToScreen(UIView modal, bool animate)
		{
			MainView.AddSubview(modal);
			if(animate)
			{
				modal.Transform = CGAffineTransform.MakeScale(0.2f,0.2f);
				modal.Alpha = 0.5f;
				UIView.BeginAnimations ("addModel"); 
	            UIView.SetAnimationDuration (0.5); 
				UIView.SetAnimationDidStopSelector(new Selector("fadeInDidFinish"));
				modal.Alpha = 1;
				modal.Transform = CGAffineTransform.MakeScale(1f,1f);
	            UIView.CommitAnimations ();  
			}
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromResource(GetType().Assembly,"ClanceysLib.Images.Home.png"),UIBarButtonItemStyle.Bordered,delegate{
				CloseModal(animate);
			});
		}
		
		public void CloseModal( bool animate)
		{
			NavigationItem.LeftBarButtonItem = null;
			if(topModal == null)
				return;
			
			if(animate)
			{
				var modal = topModal as UIView;
				UIView.BeginAnimations ("closeModel"); 
	            UIView.SetAnimationDuration (0.5);  
				UIView.SetAnimationDelegate(this);
				UIView.SetAnimationDidStopSelector(new Selector("fadeOutDidFinish"));	           
				modal.Transform = CGAffineTransform.MakeScale(0.2f,0.2f);
				modal.Alpha = 0.5f;
				UIView.CommitAnimations();
			}
			else
				FadeOutDidFinish();
		}
		
		public void CloseModalViewController(bool animate)
		{
			if(topModal == null)
				return;
			
			if(animate)
			{
				var modal = topModal as UIViewController;
				UIView.BeginAnimations ("closeModel"); 
	            UIView.SetAnimationDuration (0.5);  
				UIView.SetAnimationDelegate(this);
				UIView.SetAnimationDidStopSelector(new Selector("fadeOutVcDidFinish"));
				modal.View.Transform = CGAffineTransform.MakeScale(0.2f,0.2f);
				modal.View.Alpha = 0.5f;
				UIView.CommitAnimations();
			}
			else
				this.NavigationController.PopViewControllerAnimated(false);
		}
		
		[Export("fadeOutDidFinish")]
		public void FadeOutDidFinish()
		{
				var modal = topModal as UIView;
			
				modal.Transform = CGAffineTransform.MakeScale(1f,1f);
				modal.RemoveFromSuperview();
				topModal = null;
		}	
		
		[Export("fadeOutVcDidFinish")]
		public void FadeOutVcDidFinish()
		{
				this.NavigationController.PopViewControllerAnimated(false);
				topModal = null;
		}
		[Export("fadeInDidFinish")]
		public void FadeInDidFinish()
		{
			/*
			if(topModal is UIViewController)
				((UIViewController)topModal).ViewWillAppear(true);
				*/
		}
	
		
		
		private void CreatePanels ()
		{
			scrollView.Scrolled += ScrollViewScrolled;
			scrollView.PagingEnabled = true;
			pageControl.TouchUpInside += HandlePageControlTouchUpInside;
			
			int count = 0;
			RectangleF scrollFrame = scrollView.Frame;
			scrollFrame.Width = scrollFrame.Width * Pages.Count;
			scrollView.ContentSize = scrollFrame.Size;
			
			
			foreach (var page in Pages)
			{
				RectangleF frame = scrollView.Frame;
		        PointF location = new PointF();
		        location.X = frame.Width *count;
		
		        frame.Location = location;
				page.parent = this;
				page.Frame = frame;
				page.Columns = Columns;
				page.Rows = Rows;
				
				if(page.Superview != scrollView)
					scrollView.AddSubview(page);
				page.Refresh();
				
				Console.WriteLine("Page " + count + ": " + frame.X);
				count+=1;
			}
			
			pageControl.Pages = count;
		}


		private void ScrollViewScrolled (object sender, EventArgs e)
		{
			double page = Math.Floor ((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;
			
			pageControl.CurrentPage = (int)page;
		}
		
		void HandlePageControlTouchUpInside (object sender, EventArgs e)
        {
            RectangleF toRect = new RectangleF(scrollView.Frame.Width * pageControl.CurrentPage, scrollView.Frame.Y, scrollView.Frame.Width, scrollView.Frame.Height);
            scrollView.ScrollRectToVisible(toRect, true);        
        }
		//
	}
}

