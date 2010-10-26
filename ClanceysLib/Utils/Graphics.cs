using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
namespace ClanceysLib
{
	public class Graphics
	{
		public static UIImage AdjustImage (RectangleF rect, UIImage template, CGBlendMode mode, float red, float green, float blue, float alpha)
		{
			using (var cs = CGColorSpace.CreateDeviceRGB ())
			{
				using (var context = new CGBitmapContext (IntPtr.Zero, (int)rect.Width, (int)rect.Height, 8, (int)rect.Height * 4, cs, CGImageAlphaInfo.PremultipliedLast))
				{
					
					context.TranslateCTM (0.0f, 0f);
					//context.ScaleCTM(1.0f,-1.0f);
					context.DrawImage (rect, template.CGImage);
					context.SetBlendMode (mode);
					context.ClipToMask (rect, template.CGImage);
					context.SetRGBFillColor (red, green, blue, alpha);
					context.FillRect (rect);
					
					return UIImage.FromImage (context.ToImage ());
				}
			}
		}
	}
}

