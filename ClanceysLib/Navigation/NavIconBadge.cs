using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using System.Collections.Generic;
using OpenTK.Graphics.ES11;
namespace ClanceysLib
{
	public class NavIconBadge : UIView
	{
		const float height = 25;
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

			Width = Convert.ToInt32(numberSize.Width);
			Width = (int)(Width < height ? height : Width);
			
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
				col = UIColor.FromRGBA(139f, 0f, 0f, 1.000f);

			context.SetShouldAntialias(true);
			
			CGLayer buttonLayer = CGLayer.Create(context,rect.Size);
			drawRoundedRect(buttonLayer.Context,rect);
			context.DrawLayer(buttonLayer,rect);
			buttonLayer.Dispose();
			
			
			CGLayer shineLayer = CGLayer.Create(context,rect.Size);
			drawGradient(shineLayer.Context,rect);
			context.DrawLayer(shineLayer,rect);
			shineLayer.Dispose();

			CGLayer frameLayer = CGLayer.Create(context,rect.Size);
			drawFrame(frameLayer.Context,rect);
			context.DrawLayer(frameLayer,rect);
			frameLayer.Dispose();
	
			UIFont textFont = UIFont.BoldSystemFontOfSize(13); 
			SizeF textSize = this.StringSize(_badgeNumber.ToString(),textFont);
	
			var newPoint = new PointF((rect.Size.Width/2-textSize.Width/2), (rect.Size.Height/2-textSize.Height/2));
			
			
			
			context.RestoreState();
			
			bounds.X = (bounds.Size.Width  - numberSize.Width) / 2 ;
			bounds.Y = (bounds.Size.Height  - numberSize.Height) / 2;

			UIColor.White.SetFill();
			this.DrawString(countString, bounds, textFont);
		}
		
		
		// Draws the Badge with Quartz
		private void drawRoundedRect (CGContext context, RectangleF rect)
		{
			var frame = rect;
			frame.Y += 5;
			frame.X += 2;
			frame.Width -= 6;
			frame.Height -=6;
			
			context.SetFillColorWithColor(UIColor.Black.ColorWithAlpha(.7f).CGColor);
			context.FillEllipseInRect(frame);
			
			frame.Y -= 3;
			context.SetFillColorWithColor(BadgeColor.CGColor);
			context.FillEllipseInRect(frame);
			
		}

		// Draws the Badge Frame with Quartz
		private void drawFrame (CGContext context, RectangleF rect)
		{
			var frame = rect;
			frame.Y += 2;
			frame.X += 2;
			frame.Width -= 6;
			frame.Height -=6;
			
			context.SetFillColorWithColor(UIColor.White.CGColor);
			context.SetStrokeColorWithColor(UIColor.White.CGColor);
			context.SetLineWidth(2f);
			context.StrokeEllipseInRect(frame);
			
		}
		
		private void drawGradient(CGContext context, RectangleF rect)
		{
			
			var shineFrame = rect;
			shineFrame.Y += 2;
			shineFrame.X += 4;
			shineFrame.Width -= 8;
			shineFrame.Height = (shineFrame.Height / 2);
			
			// the colors
			var topColor = UIColor.White.ColorWithAlpha (0.5f).CGColor;
			var bottomColor = UIColor.White.ColorWithAlpha (0.10f).CGColor;
			List<float> colors = new List<float>();
			colors.AddRange(topColor.Components);
			colors.AddRange(bottomColor.Components);
			float[] locations = new float[]{0, 1};
			
			CGGradient gradient = new CGGradient(topColor.ColorSpace,colors.ToArray(),locations);
			
			context.SaveState();
			context.SetShouldAntialias(true);
			context.AddEllipseInRect(shineFrame);
			context.Clip();
		
		    var startPoint = new PointF(shineFrame.GetMidX(), shineFrame.GetMidY());
		    var endPoint = new PointF(shineFrame.GetMidX(), shineFrame.GetMaxY());
		
			context.DrawLinearGradient(gradient,startPoint,endPoint,CGGradientDrawingOptions.DrawsBeforeStartLocation);
			gradient.Dispose();
			context.RestoreState();
		}
		
	}
	
}

