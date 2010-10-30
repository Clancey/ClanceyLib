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
		private Dictionary<string,NavModal> modals = new Dictionary<string, NavModal>();
		public int Columns = 3;
		public int Rows = 5;
		public float spacing = 10;
		private UIView MainView;
		private UIScrollView scrollView;
		private UIPageControl pageControl;
		private float pageControlH = 30;
		private NavModal topModal;

		public NavLauncher () :base()
		{
			var mainFrame = this.View.Bounds;
			mainFrame.Height -= 44;
			MainView = new UIView(mainFrame);
			//this.Frame = rect;
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
		public void LaunchModal(string name, object[] parameters)
		{
		
			if(string.IsNullOrEmpty(name))
			{
				return;
			}
			NavModal modal = null; 
			modals.TryGetValue(name,out modal);
			if(modal != null)
			{
				this.AddModalToScreen(modal,true);
				return;
			}
			modal = new NavModal(MainView.Frame);
			var theView = ObjectFactory.Create(name,parameters);
			if(theView is UIViewController)
				modal.MainView = (theView as UIViewController).View;
			else if(theView is UIView)
				modal.MainView = (theView as UIView);
			modals.Add(name,modal);		
			
			AddModalToScreen(modal,true);
		}
		
		private void AddModalToScreen(NavModal modal, bool animate)
		{
			MainView.AddSubview(modal);
			topModal = modal;
			if(animate)
			{
				modal.Transform = CGAffineTransform.MakeScale(0.2f,0.2f);
				topModal.Alpha = 0.5f;
				UIView.BeginAnimations ("addModel"); 
	            UIView.SetAnimationDuration (0.5);  
	  			
	           
				topModal.Alpha = 1;
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
				UIView.BeginAnimations ("closeModel"); 
	            UIView.SetAnimationDuration (0.5);  
				UIView.SetAnimationDelegate(this);				
				UIView.SetAnimationDidStopSelector(new Selector("fadeOutDidFinish"));	           
				topModal.Transform = CGAffineTransform.MakeScale(0.2f,0.2f);
				topModal.Alpha = 0.5f;
				UIView.CommitAnimations();
			}
			else
				FadeOutDidFinish();
		}
		[Export("fadeOutDidFinish")]
		public void FadeOutDidFinish()
		{
				topModal.Transform = CGAffineTransform.MakeScale(1f,1f);
				topModal.RemoveFromSuperview();
				topModal = null;
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
            // big assumption: the user moves the page control and after the control has set the current page this event is fired
            RectangleF toRect = new RectangleF(scrollView.Frame.Width * pageControl.CurrentPage, scrollView.Frame.Y, scrollView.Frame.Width, scrollView.Frame.Height);
            scrollView.ScrollRectToVisible(toRect, true);        
        }
		//
	}
}

