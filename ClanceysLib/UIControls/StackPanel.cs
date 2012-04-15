using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
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
		public float Padding {get;set;}
		public bool StretchWidth { get; set; }
		public bool CenterItems {get;set;}
		public UIScrollView ParentScrollView {get;set;}
		

		public StackPanel (RectangleF rect) : base (rect)
		{
			StretchWidth = true;
			Padding = 5;
		}
		
		public StackPanel (IntPtr handle) : base(handle)
		{

		}
		
		public StackPanel () : base()
		{
			StretchWidth = true;	
			Padding = 5;	
		}

		public override void LayoutSubviews ()
		{
			float lastY = this.Bounds.Y + Padding;
			float maxWidth = 0;
			if(CenterItems)
			{
				var totalHeight = Subviews.Sum(x=> x.Frame.Height + Padding) - Padding;
				if(totalHeight < Bounds.Height)
				{
					lastY = (Bounds.Height - totalHeight)/2;
				}
			}
			
			foreach (var view in Subviews)
			{
				var rect = view.Frame;
				if (StretchWidth)
					rect.Width = this.Bounds.Width - (Padding * 2);
				rect.X = Padding + this.Bounds.X;
				rect.Y = lastY;
				
				view.Frame = rect;
				if(maxWidth < rect.Width)
					maxWidth = rect.Width;
				lastY += rect.Height + Padding;
			}
			if(ParentScrollView != null)
				ParentScrollView.ContentSize = new SizeF(maxWidth,lastY);
		}
	}
}
