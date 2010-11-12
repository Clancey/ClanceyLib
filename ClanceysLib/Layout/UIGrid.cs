using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using MonoTouch.UIKit;
using System.Collections.Generic;
namespace ClanceysLib
{
	public class UIGrid : UIView
	{
		private float columnWidth;
		private float rowHeight;
		private int columns;
		private int rows;
		public float ColumnPadding = 5;
		public float RowPadding = 5;
		private List<ViewItem> views = new List<ViewItem>();
		private bool useCounts;
		public UIGrid (RectangleF rect,int Columns,int Rows) :base (rect)
		{
			useCounts = true;
			columns = Columns;
			rows = Rows;
			columnWidth = rect.Width / Columns;
			rowHeight = rect.Height / Rows;
		}
		
		public UIGrid (RectangleF rect,float ColumnWidth,float RowHeight) :base (rect)
		{
			columnWidth = ColumnWidth;
			rowHeight = RowHeight;
		}
		
		public override void AddSubview (UIView view)
		{
			int column = 1;
			int columnSpan = 1;
			int row = 1;
			int rowSpan = 1;
			var members = this.Superview.GetType().GetFields();
			var field = members.Where(x=> x.GetValue(this.Superview) == view).FirstOrDefault();
			if(field != null)
			{
				object[] layout = field.GetCustomAttributes(typeof(LayoutAttribute),false);
				if(layout.Count() == 1)
				{
					var l = (LayoutAttribute)layout[0];
					column = l.Column;
					row = l.Row;
				}
				object[] span = field.GetCustomAttributes(typeof(SpanAttribute),false);
				if(span.Count() == 1)
				{
					var s = (SpanAttribute)span[0];
					columnSpan = s.ColumnSpan;
					rowSpan = s.RowSpan;
				}
			}
			
			var frame = view.Frame;
			frame.Location = getLocation(column,row);
			frame.Size = getSize(columnSpan,rowSpan);
			
			view.Frame = frame;
			
			views.Add(new ViewItem(){View = view,Column = column,ColumnSpan = columnSpan,Row = row,RowSpan = rowSpan});
			
			base.AddSubview (view);
		}
		
		public override void WillRemoveSubview (UIView uiview)
		{
			views.Remove(views.Where(x=> x.View == uiview).FirstOrDefault());
			base.WillRemoveSubview (uiview);
		}
		public override RectangleF Frame {
			get {
				return base.Frame;
			}
			set {
				base.Frame = value;				
				reload();
			}
		}
		
		private void reload()
		{
			// Only reload if you used counts not specified widths/height
			if(!useCounts)
				return;
			
			columnWidth = this.Frame.Width / columns;
			rowHeight = this.Frame.Height / rows;
			
			foreach(var view in this.Subviews)
			{
				var theView = views.Where(x=> x.View == view).FirstOrDefault();
				var loc = getLocation(theView.Column,theView.Row);
				var size = getSize(theView.ColumnSpan,theView.RowSpan);
				view.Frame = new RectangleF(loc,size);
			}
		}
		
		private PointF getLocation(int column, int row)
		{
			var x = ((column - 1) * columnWidth) + ColumnPadding;
			
			var y = ((row - 1) * rowHeight) + RowPadding;
			return new PointF(x,y);
		}
		
		private SizeF getSize(int columnSpan,int rowSpan)
		{
			var width = (columnSpan * columnWidth) - (ColumnPadding * 2);
			var height = (rowSpan * rowHeight) - (RowPadding * 2);
			return new SizeF(width,height);
			
		}
		
		private class ViewItem
		{
			public int Column {get;set;}
			public int ColumnSpan {get;set;}
			public int Row {get;set;}
			public int RowSpan {get;set;}
			
			public UIView View {get;set;}
		}
		
	}
}

