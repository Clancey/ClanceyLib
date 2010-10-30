using System;
using MonoTouch.UIKit;
using System.Drawing;
namespace ClanceySamples
{
	public class TestView : UIView
	{
		private UILabel label;
		public TestView ()
		{
			this.BackgroundColor = UIColor.Red;
		}
		public TestView(RectangleF rect) :base(rect)
		{
			this.BackgroundColor = UIColor.Red;
		}
		public TestView(RectangleF rect,string text) :base(rect)
		{
			this.BackgroundColor = UIColor.Red;
			label = new UILabel(rect);
			label.Text = text;
			label.BackgroundColor = this.BackgroundColor;
			AddSubview(label);
		}
		public TestView(RectangleF rect,string text,UIColor backColor) :base(rect)
		{
			this.BackgroundColor = backColor;
			label = new UILabel(rect);
			label.Text = text;
			label.BackgroundColor = backColor;
			AddSubview(label);
		}
	}
}

