using System;
using MonoTouch.MapKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
namespace ClanceysLib
{
	public class PolygonOverlay : MKPolygonView
	{

		private MKPolygon _polygon;
		private List<MKPolygon> _polygons;
		public PolygonOverlay (MKPolygon polygon)
		{
			_polygon = polygon;
		}
		public PolygonOverlay (List<MKPolygon> polygons)
		{
			_polygons = polygons;
		}
		public CGPath polyPath (MKPolygon polygon,MKMapRect mapRect, float zoomScale, CGContext context)
		{
			MKMapPoint[] points = polygon.Points;
			Int32 pointCount = polygon.PointCount;
			Int32 i;
			
			if (pointCount < 3)
				return null;
			
			CGPath path = new CGPath ();
			
			PointF relativePoint = this.PointForMapPoint (points[0]);
			path.MoveToPoint (relativePoint.X, relativePoint.Y);
			for (i = 1; i < pointCount; i++)
			{
				relativePoint = this.PointForMapPoint (points[i]);
				path.AddLineToPoint(relativePoint);
				//path.CGPathAddLineToPoint (relativePoint.X, relativePoint.Y);
			}
			
			return path;
			
		}
		
		public void DrawPolygon(MKPolygon polygon,MKMapRect mapRect, float zoomScale, CGContext context)
		{
			CGPath path = this.polyPath (polygon,mapRect,zoomScale,context);
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
		
		public override void DrawMapRect (MKMapRect mapRect, float zoomScale, CGContext context)
		{
			if(_polygon != null)
				DrawPolygon(_polygon,mapRect,zoomScale,context);
			else 
				foreach(var polygon in _polygons)
				{
					DrawPolygon(polygon,mapRect,zoomScale,context);
				}
		}
		
	}
}

