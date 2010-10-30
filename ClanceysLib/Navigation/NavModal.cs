using System;
using MonoTouch.UIKit;
using System.Drawing;
namespace ClanceysLib
{
	public class NavModal: UIView
	{
		public NavModal(){}
		public NavModal (RectangleF rect):base(rect)
		{
			
		}
		public UIView MainView;
		public override void WillMoveToSuperview (UIView newsuper)
		{
			MainView.Frame = Frame;
			if(MainView.Superview != this)
				this.AddSubview(MainView);
			base.WillMoveToSuperview (newsuper);
		}
	}
}

