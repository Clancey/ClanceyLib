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
	public class NavIconBadge : UIButton
	{
		static float height;
		int Width {get;set;}
		SizeF numberSize;
		UIFont font = UIFont.BoldSystemFontOfSize(15f);
		string countString;
		const float badgeCornerRoundness = .4f;
		static UIImage bgimage = UIImage.FromFile("Images/SBBadgeBG.png");
		public UIColor BadgeColor {get;set;}
		public UIColor BadgeColorHighlighted {get;set;}
		UILabel lbl;
		public bool DockLeft {get;set;}
		
		private int _badgeNumber;
		
		public int BadgeNumber
		{
			get {return _badgeNumber;}
			set {
				if(_badgeNumber == value)
					return;
				_badgeNumber = value;
				CalculateSize();
			}
			
		}
		
		public NavIconBadge () : base ()
		{
			//this.BackgroundColor = UIColor.Clear;
			//BadgeColor = UIColor.Red;
			
			height = bgimage.Size.Height;
			lbl = new UILabel();
			lbl.TextAlignment = UITextAlignment.Center;
			lbl.Font = font;
			lbl.TextColor = UIColor.White;
			lbl.BackgroundColor = UIColor.Clear;
			lbl.ContentMode = UIViewContentMode.ScaleToFill;
			lbl.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
			this.AddSubview(lbl);
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

			Width = Convert.ToInt32(numberSize.Width) + 20;
			Width = (int)(Width < bgimage.Size.Width ? bgimage.Size.Width : Width);
			
			if(!DockLeft)
				origin = new PointF(origin.X + Width, origin.Y);
			
			this.Frame = new RectangleF(origin, new SizeF(Width,height));
			lbl.Frame = new RectangleF(((Width - numberSize.Width + 1) /2) ,0,numberSize.Width,height -7.1f);
		}
		
		private void CalculateSize()
		{
			var number = BadgeNumber.ToString();
			if(_badgeNumber > 0)
			{
				this.SetBackgroundImage(bgimage.StretchableImage(12,0),UIControlState.Normal);
				lbl.Text = number;
				//this.SetTitle(number,UIControlState.Normal);
			}
			else
			{
				this.SetBackgroundImage(new UIImage(),UIControlState.Normal);
				lbl.Text = "";
				//this.SetTitle("",UIControlState.Normal);
			}
			CalculateSize(new PointF(Frame.X + Frame.Width,Frame.Y));
		}
		
	}
	
}

