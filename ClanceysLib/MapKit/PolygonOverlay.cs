using System;
using MonoTouch.MapKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using System.Linq;
namespace ClanceysLib
{
	public class PolygonOverlay : MKPolygonView
	{

		private MKPolygon _polygon;
		public PolygonOverlay (MKPolygon polygon)
		{
			_polygon = polygon;
		}
		public CGPath polyPath (MKPolygon polygon)
		{
			
			
			MKMapPoint[] points = polygon.Points;
			Int32 pointCount = polygon.PointCount;
			Int32 i;
			
			if (pointCount < 3)
				return null;
			
			CGPath path = new CGPath ();
			
			//TODO: Fix so it draws all inerior polygons as well.
			/*
			foreach (MKPolygon interiorPolygon in polygon.InteriorPolygons)
			{
				CGPath interiorPath = this.polyPath (interiorPolygon);
				CGPathAddPath (path, NULL, interiorPath);
				CGPathRelease (interiorPath);
			}
			*/
			
			PointF relativePoint = this.PointForMapPoint (points[0]);
			path.MoveToPoint (relativePoint.X, relativePoint.Y);
			for (i = 1; i < pointCount; i++)
			{
				relativePoint = this.PointForMapPoint (points[i]);
				path.CGPathAddLineToPoint (relativePoint.X, relativePoint.Y);
			}
			
			return path;
			
		}
		public override void DrawMapRect (MKMapRect mapRect, float zoomScale, CGContext context)
		{
			CGPath path = this.polyPath (_polygon);
			if (path != null)
			{
				this.ApplyFillProperties (context, zoomScale);
				context.BeginPath ();
				context.AddPath (path);
				// CGContextAddPath(context, path);
				context.DrawPath (CGPathDrawingMode.EOFill);
				//  CGContextDrawPath(context, kCGPathEOFill);
				this.ApplyStrokeProperties (context, zoomScale);
				context.BeginPath ();
				// CGContextBeginPath(context);
				context.AddPath (path);
				// CGContextAddPath(context, path);
				context.StrokePath ();
				// CGContextStrokePath(context);
			}
		}
		
	}
}

