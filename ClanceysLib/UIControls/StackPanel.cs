using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
namespace ClanceysLib
{

	/*
	 
	 Sample Code
	 
		button = UIButton.FromType(UIButtonType.RoundedRect);
		button.Frame = new RectangleF(0,0,10,20);
		button.SetTitle("Remove Me",UIControlState.Normal);
		button.TouchDown += delegate {
			panel.Remove(0);
		};
		StackView panel = panel = new StackView(window.Bounds);
		panel.StretchWidth = true;
		panel.AddRange(new UIView[]{new UILabel(new RectangleF(0,0,100,50)){Text = "Test Text Label", BackgroundColor = UIColor.Blue}
			,new UITextView(new RectangleF(0,0,300,50)){Text = "Test Text Box",BackgroundColor = UIColor.Yellow}
			, button});
		

		window.AddSubview (panel);
*/
	public class StackView : UIView
	{
		private List<UIView> views = new List<UIView> ();
		public float padding = 10;
		public bool StretchWidth { get; set; }

		public StackView (RectangleF rect)
		{
			this.Frame = rect;
			this.Bounds = rect;
		}

		public override void WillMoveToSuperview (UIView newsuper)
		{
			Redraw ();
			base.WillMoveToSuperview (newsuper);
		}

		public override void WillMoveToWindow (UIWindow window)
		{
			Redraw ();
			base.WillMoveToWindow (window);
		}

		public void Add (UIView view)
		{
			views.Add (view);
			Redraw ();
		}
		public void AddRange (IEnumerable<UIView> Views)
		{
			views.AddRange (Views);
			Redraw ();
		}
		public void Insert (int Index, UIView view)
		{
			views.Insert (Index, view);
			Redraw ();
		}
		public void Remove (UIView view)
		{
			views.Remove (view);
			view.RemoveFromSuperview ();
			Redraw ();
		}
		public void Remove (int Index)
		{
			var view = views[Index];
			if (view != null)
			{
				view.RemoveFromSuperview ();
				views.Remove (view);
			}
			Redraw ();
		}


		public void Redraw ()
		{
			float lastY = 0 + padding;
			
			foreach (var view in views)
			{
				var rect = view.Frame;
				if (StretchWidth)
					rect.Width = this.Frame.Width - (padding * 2);
				rect.X = padding;
				rect.Y = lastY;
				
				view.Frame = rect;
				if (view.Superview != this)
					AddSubview (view);
				lastY += rect.Height + padding;
				
			}
		}
	}
}
