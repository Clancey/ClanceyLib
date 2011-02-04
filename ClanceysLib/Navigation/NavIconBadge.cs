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
		const float badgeCornerRoundness = .4f;
		
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

			Width = Convert.ToInt32(numberSize.Width + 13);
			
			if(!DockLeft)
				origin = new PointF(origin.X + Width, origin.Y);
			
			this.Frame = new RectangleF(origin, new SizeF(Width, Width));
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

			context.SetShouldAntialias(true);
			CGLayer buttonLayer = CGLayer.Create(context,rect.Size);
			drawRoundedRect(buttonLayer.Context,rect);
			context.DrawLayer(buttonLayer,rect);
			buttonLayer.Dispose();
			

			CGLayer frameLayer = CGLayer.Create(context,rect.Size);
			drawFrame(frameLayer.Context,rect);
			context.DrawLayer(frameLayer,rect);
			frameLayer.Dispose();
	
	
			UIFont textFont = UIFont.BoldSystemFontOfSize(13); 
			SizeF textSize = this.StringSize(_badgeNumber.ToString(),textFont);
		//[self.badgeText drawAtPoint:CGPointMake((rect.size.width/2-textSize.width/2), (rect.size.height/2-textSize.height/2)) withFont:textFont];

			var newPoint = new PointF((rect.Size.Width/2-textSize.Width/2), (rect.Size.Height/2-textSize.Height/2));
			
			
			
			context.RestoreState();
			
			bounds.X = (bounds.Size.Width - numberSize.Width) / 2 + 0.5f;
			bounds.Y = (bounds.Size.Height - numberSize.Height) / 2 + 0.5f;

			context.SetBlendMode( CGBlendMode.Clear);
			this.DrawString(countString, bounds, textFont);
		}
		
		
		// Draws the Badge with Quartz
		private void drawRoundedRect (CGContext context, RectangleF rect)
		{
			
			float radius = rect.GetMaxY() * badgeCornerRoundness;
			float puffer = rect.GetMaxY() * 0.10f;
			
			float maxX = rect.GetMaxX() - puffer;
			float maxY = rect.GetMaxY() - puffer;
			float minX = rect.GetMinX() + puffer;
			float minY = rect.GetMinY() + puffer;
			
			context.BeginPath();
			context.SetFillColorWithColor(BadgeColor.CGColor);
			context.AddArc((float)(maxX-radius), (float)(minY+radius), radius,(float)(Math.PI +(Math.PI/2)), 0f, false);
			context.AddArc(maxX-radius, maxY-radius, radius, 0, (float)(Math.PI/2), false);
			
			context.AddArc(minX+radius, maxY-radius, radius, (float)(Math.PI/2), (float)Math.PI, false);
			context.AddArc(minX+radius, minY+radius, radius, (float)Math.PI, (float)(Math.PI+Math.PI/2), false);
			context.SetShadowWithColor(new SizeF(2,2),3f,UIColor.Black.CGColor);
			context.ClosePath();
			context.FillPath();
			
		}

		// Draws the Badge Frame with Quartz
		private void drawFrame (CGContext context, RectangleF rect)
		{
			float radius = rect.GetMaxY() * badgeCornerRoundness;
			float puffer = rect.GetMaxY() * 0.10f;
			
			float maxX = rect.GetMaxX() - puffer;
			float maxY = rect.GetMaxY() - puffer;
			float minX = rect.GetMinX() + puffer;
			float minY = rect.GetMinY() + puffer;
			
			context.BeginPath();
			context.SetLineWidth(2f);
			context.SetStrokeColorWithColor(UIColor.White.CGColor);
			
			
			context.AddArc( maxX-radius, minY+radius, radius, (float)(Math.PI+(Math.PI/2)), 0, false);
			context.AddArc( maxX-radius, maxY-radius, radius, 0, (float)(Math.PI/2), false);
			context.AddArc( minX+radius, maxY-radius, radius, (float)(Math.PI/2), (float)Math.PI, false);
			context.AddArc( minX+radius, minY+radius, radius, (float)Math.PI,(float)( Math.PI+Math.PI/2), false);
			context.ClosePath();
			context.StrokePath();
			
		}
		
	}
	
}

