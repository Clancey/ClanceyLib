using System;
using MonoTouch.MapKit;
using System.Collections.Generic;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;
namespace ClanceysLib
{
	public class PolylineOverlay: MKPolylineView
	{
		private MKPolyline _polyline;
		private List<MKPolyline> _polylines;
		public PolylineOverlay (MKPolyline polyline)
		{
			_polyline = polyline;
		}
		public PolylineOverlay (List<MKPolyline> polylines)
		{
			_polylines = polylines;
		}
		public CGPath polyPath (MKPolyline polyline,MKMapRect mapRect, float zoomScale, CGContext context)
		{
			MKMapPoint[] points = polyline.Points;
			Int32 pointCount = polyline.PointCount;
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
			}
			
			return path;
			
		}
		public static MKMapSize MKMapSizeWorld = new MKMapSize (0x10000000, 0x10000000);

		
		public void DrawPolyline(MKPolyline polygon,MKMapRect mapRect, float zoomScale, CGContext context)
		{
			CGPath path = this.polyPath (polygon,mapRect,zoomScale,context);
			if (path != null)
			{				
				this.ApplyFillProperties (context, zoomScale);
				context.BeginPath ();
				context.AddPath (path);
				context.DrawPath (CGPathDrawingMode.Stroke);
				this.ApplyStrokeProperties (context, zoomScale);
				context.BeginPath ();
				context.AddPath (path);
				context.StrokePath ();
			}
		}
		
		public override void DrawMapRect (MKMapRect mapRect, float zoomScale, CGContext context)
		{
			Console.WriteLine("Drawing paths");
			Console.WriteLine(zoomScale);
			 Console.WriteLine( MKRoadWidthAtZoomScale(zoomScale));
			//this.LineDashPattern = new NSNumber[]{new NSNumber(2),new NSNumber(0),new NSNumber(1)};
			this.LineJoin = CGLineJoin.Round;
			if(_polyline != null)
				DrawPolyline(_polyline,mapRect,zoomScale,context);
			else 
				foreach(var polyline in _polylines)
				{
					DrawPolyline(polyline,mapRect,zoomScale,context);
				}
		}
		
	}
}

