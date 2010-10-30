using System;
using MonoTouch.UIKit;
using System.Drawing;
namespace ClanceysLib
{
	public class testView :UIView
	{
		public testView (object test)
		{
		}
		public testView (RectangleF rect): base (rect)
		{
			this.BackgroundColor = UIColor.White;
		}
	}
}

