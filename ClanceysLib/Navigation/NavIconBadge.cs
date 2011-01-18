using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
namespace ClanceysLib
{
	public class NavIconBadge : UIView
	{
		int Width {get;set;}
		SizeF numberSize;
		UIFont font = UIFont.BoldSystemFontOfSize(14f);
		string countString;
		public UIColor BadgeColor {get;set;}
		public UIColor BadgeColorHighlighted {get;set;}
		public bool DockLeft {get;set;}
		
		private int _badgeNumber;
		
		public int BadgeNumber
		{
			get {return _badgeNumber;}
			set {
				_badgeNumber = value;
				CalculateSize();
			}
			
		}
		
		public NavIconBadge ()
		{
			this.BackgroundColor = UIColor.Clear;
		}
		
		public void SetLocation(PointF origin,bool dockLeft)
		{
			DockLeft = dockLeft;
			CalculateSize(origin);	
		}
		
		private void CalculateSize(PointF origin)
		{
			countString = _badgeNumber.ToString();
			NSString ns = new NSString(countString);
			numberSize = ns.StringSize (font);

			Width = Convert.ToInt32(numberSize.Width + 10);
			
			if(!DockLeft)
				origin = new PointF(origin.X + Width, origin.Y);
			
			this.Frame = new RectangleF(origin, new SizeF(Width, 18));
		}
		
		private void CalculateSize()
		{
			CalculateSize(new PointF(Frame.X + Frame.Width,Frame.Y));
		}
		
		
		public override void Draw (RectangleF rect)
		{
			if(_badgeNumber == 0 )
				return;
			
			var bounds = Frame;
			var context = UIGraphics.GetCurrentContext();
			
			float radius = bounds.Size.Height / 2.0f;

			context.SaveState();


			UIColor col;
			if (this.BadgeColor != null)
				col = this.BadgeColor;
			else 
				col = UIColor.FromRGBA(0.530f, 0.600f, 0.738f, 1.000f);

			context.SetFillColorWithColor (col.CGColor);
		

			context.BeginPath();
			float a = Convert.ToSingle(Math.PI / 2f);
			float b = Convert.ToSingle(3f * Math.PI / 2f);
			context.AddArc(radius, radius, radius, a, b, false);
			context.AddArc(bounds.Size.Width - radius, radius, radius, b, a, false);
			context.ClosePath();
			context.FillPath();
			context.RestoreState();
			
			bounds.X = (bounds.Size.Width - numberSize.Width) / 2 + 0.5f;

			context.SetBlendMode( CGBlendMode.Clear);
			this.DrawString(countString, bounds, font);
		}
	}
	
}

