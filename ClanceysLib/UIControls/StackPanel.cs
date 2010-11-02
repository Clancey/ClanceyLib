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
	public class StackPanel : UIView
	{
		public float padding = 10;
		public bool StretchWidth { get; set; }
		public UIScrollView ParentScrollView {get;set;}

		public StackPanel (RectangleF rect)
		{
			this.Frame = rect;
			this.Bounds = rect;
		}
		
		public StackPanel (IntPtr handle) : base(handle)
		{
		
		}
		
		public override void SubviewAdded (UIView uiview)
		{
			base.SubviewAdded (uiview);
			Redraw();
		}
		public override void WillRemoveSubview (UIView uiview)
		{
			base.WillRemoveSubview (uiview);
			Redraw();
		}


		public void Redraw ()
		{
			float lastY = 0 + padding;
			float maxWidth = 0;
			
			foreach (var view in Subviews)
			{
				var rect = view.Frame;
				if (StretchWidth)
					rect.Width = this.Frame.Width - (padding * 2);
				rect.X = padding;
				rect.Y = lastY;
				
				view.Frame = rect;
				if(maxWidth < rect.Width)
					maxWidth = rect.Width;
				lastY += rect.Height + padding;
			}
			if(ParentScrollView != null)
				ParentScrollView.ContentSize = new SizeF(maxWidth,lastY);
		}
	}
}
