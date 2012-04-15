using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using MonoTouch.ObjCRuntime;
namespace ClanceysLib
{
	public class Graphics
	{
		static CGPath smallPath = MakeRoundedPath (48);
		static CGPath largePath = MakeRoundedPath (73);
		public static UIImage AdjustImage (RectangleF rect, UIImage template, CGBlendMode mode, UIColor color)
		{
			float red = new float ();
			float green = new float ();
			float blue = new float ();
			float alpha = new float ();
			if (color == null)
				color = UIColor.FromRGB (100, 0, 0);
			color.GetRGBA (out red, out green, out blue, out alpha);
			return AdjustImage (rect, template, mode, red, green, blue, alpha);
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
		static Selector sscale;
		public static void ConfigLayerHighRes (CALayer layer)
		{
			if (!HighRes)
				return;
			
			if (sscale == null)
				sscale = new Selector ("setContentsScale:");
			
			Messaging.void_objc_msgSend_float (layer.Handle, sscale.Handle, 2.0f);
		}
		
		
		public static UIImage ResizeImage (SizeF size, UIImage image, bool KeepRatio)
		{
			var curSize = image.Size;
			SizeF newSize;
			if (KeepRatio)
			{
				var ratio = Math.Min (size.Width / curSize.Width, size.Height / curSize.Height);
				newSize = new SizeF (curSize.Width * ratio, curSize.Height * ratio);
			}

			else
			{
				newSize = size;
			}
			
			return image.Scale (newSize);
			
			
		}

		// Check for multi-tasking as a way to determine if we can probe for the "Scale" property,
		// only available on iOS4 
		public static bool HighRes = UIDevice.CurrentDevice.IsMultitaskingSupported && UIScreen.MainScreen.Scale > 1;

		// Child proof the image by rounding the edges of the image
		public static UIImage RemoveSharpEdges (UIImage image)
		{
			if (image == null)
			{
				Console.WriteLine("Remove sharp edges image is null");
				throw new ArgumentNullException ("image");
			}
			
			
			UIGraphics.BeginImageContext (image.Size);
			var c = UIGraphics.GetCurrentContext ();
			
			c.AddPath (MakeRoundedPath (image.Size.Height));
			
			image.Draw (new RectangleF (PointF.Empty, image.Size));
			var converted = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return converted;
		}

		static internal CGPath MakeRoundedPath (float size)
		{
			float hsize = size / 2;
			
			var path = new CGPath ();
			path.MoveToPoint (size, hsize);
			path.AddArcToPoint (size, size, hsize, size, 4);
			path.AddArcToPoint (0, size, 0, hsize, 4);
			path.AddArcToPoint (0, 0, hsize, 0, 4);
			path.AddArcToPoint (size, 0, size, hsize, 4);
			path.CloseSubpath ();
			
			return path;
		}

		public static CALayer MakeBackgroundLayer (UIImage image, RectangleF frame)
		{
			
			var textureColor = UIColor.FromPatternImage (image);
			
			UIGraphics.BeginImageContext (frame.Size);
					
			var c = UIGraphics.GetCurrentContext ();
			image.DrawAsPatternInRect (frame);
			
			//Images.MenuShadow.Draw (frame);
			var result = UIGraphics.GetImageFromCurrentImageContext ();
			
			UIGraphics.EndImageContext ();
			
			var back = new CALayer { Frame = frame };
			//TODO:
			//Graphics.ConfigLayerHighRes (back);
			back.Contents = result.CGImage;
			return back;
		}
	}
}

