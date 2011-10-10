// An Example of a Glass like button custom drawn using Layers.
// 
// Author: Robert Kozak

using System;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Drawing;

namespace ClanceysLib
{
	public class UIGlassyButton : UIButton
	{
		private bool _Initialized;

		public UIColor Color { get; set; }	
		public UIColor TitleColor {get;set;}
		public UIColor HighlightColor { get; set; }

		public string _Title = string.Empty;
		public new string Title 
		{ 
			get { return _Title; } 
			set 
			{ 
				_Title = value;

				SetNeedsDisplay();
			} 
		}
		
		public UIGlassyButton(RectangleF rect): base(rect)
		{
			Color = UIColor.FromRGB(88f, 170f, 34f);
			HighlightColor = UIColor.Black;
		}
		
		public void Init(RectangleF rect)
		{
			if(TitleColor == null)
				TitleColor = UIColor.White;
			Layer.MasksToBounds = true;
			Layer.CornerRadius = 8;
			
			var gradientFrame = rect;
			
			var shineFrame = gradientFrame;
			shineFrame.Y += 1;
			shineFrame.X += 1;
			shineFrame.Width -= 2;
			shineFrame.Height = (shineFrame.Height / 2);

			var shineLayer = new CAGradientLayer();
			shineLayer.Frame = shineFrame;
			shineLayer.Colors = new MonoTouch.CoreGraphics.CGColor[] { UIColor.White.ColorWithAlpha (0.75f).CGColor, UIColor.White.ColorWithAlpha (0.10f).CGColor };
			shineLayer.CornerRadius = 8;
			
			var backgroundLayer = new CAGradientLayer();
			backgroundLayer.Frame = gradientFrame;
			backgroundLayer.Colors = new MonoTouch.CoreGraphics.CGColor[] { Color.ColorWithAlpha(0.99f).CGColor, Color.ColorWithAlpha(0.80f).CGColor };

			var highlightLayer = new CAGradientLayer();
			highlightLayer.Frame = gradientFrame;
			
			Layer.AddSublayer(backgroundLayer);
			Layer.AddSublayer(highlightLayer);
			Layer.AddSublayer(shineLayer);
		
			VerticalAlignment = UIControlContentVerticalAlignment.Center;
			Font = UIFont.BoldSystemFontOfSize (15f);
			SetTitle (Title, UIControlState.Normal);
			SetTitleColor (TitleColor, UIControlState.Normal);
			
			_Initialized = true;
		}

		public override void Draw(RectangleF rect)
		{
			base.Draw(rect);

			if(!_Initialized)
				Init(rect);

			var highlightLayer = Layer.Sublayers[1] as CAGradientLayer;
			
			if (Highlighted)
			{
				if (HighlightColor == UIColor.Blue) 
				{
					highlightLayer.Colors = new MonoTouch.CoreGraphics.CGColor[] { HighlightColor.ColorWithAlpha(0.60f).CGColor, HighlightColor.ColorWithAlpha(0.95f).CGColor };
				} 
				else 
				{
					highlightLayer.Colors = new MonoTouch.CoreGraphics.CGColor[] { HighlightColor.ColorWithAlpha(0.10f).CGColor, HighlightColor.ColorWithAlpha(0.40f).CGColor };
				}
				
			}
			
			highlightLayer.Hidden = true;
		}
		
		public override bool BeginTracking(UITouch uitouch, UIEvent uievent)
		{
			if (uievent.Type == UIEventType.Touches)
			{
				SetNeedsDisplay();
			}

			return base.BeginTracking(uitouch, uievent); 
		}
		
		public override void EndTracking(UITouch uitouch, UIEvent uievent)
		{
			if (uievent.Type == UIEventType.Touches)
			{
				SetNeedsDisplay();
			}

			base.EndTracking(uitouch, uievent);
		}
	}
}