using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
namespace ClanceysLib
{
	public class Graphics
	{
		static CGPath smallPath = MakeRoundedPath (48);
		static CGPath largePath = MakeRoundedPath (73);
		public static UIImage AdjustImage(RectangleF rect,UIImage template, CGBlendMode mode,UIColor color)
		{
			float red = new float();
			float green = new float();
			float blue = new float();
			float alpha = new float();
			if (color == null)
				color = UIColor.FromRGB(100,0,0);
			color.GetRGBA(out red,out green, out blue, out alpha);
			return 	AdjustImage(rect,template,mode,red,green,blue,alpha);
		}
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
	
		
		// Check for multi-tasking as a way to determine if we can probe for the "Scale" property,
		// only available on iOS4 
		public static bool HighRes = UIDevice.CurrentDevice.IsMultitaskingSupported && UIScreen.MainScreen.Scale > 1;
		
		// Child proof the image by rounding the edges of the image
		public static UIImage RemoveSharpEdges (UIImage image)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			
			float size = HighRes ? 73 : 48;
			
			UIGraphics.BeginImageContext (new SizeF (size, size));
			var c = UIGraphics.GetCurrentContext ();
			
			if (HighRes)
				c.AddPath (largePath);
			else 
				c.AddPath (smallPath);
			
			c.Clip ();
			
			image.Draw (new RectangleF (0, 0, size, size));
			var converted = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return converted;
		}
		
		internal static CGPath MakeRoundedPath (float size)
		{
			float hsize = size/2;
			
			var path = new CGPath ();
			path.MoveToPoint (size, hsize);
			path.AddArcToPoint (size, size, hsize, size, 4);
			path.AddArcToPoint (0, size, 0, hsize, 4);
			path.AddArcToPoint (0, 0, hsize, 0, 4);
			path.AddArcToPoint (size, 0, size, hsize, 4);
			path.CloseSubpath ();
			
			return path;
		}
	}
}

