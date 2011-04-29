using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using XamlControls;
namespace ClanceysLib
{
	public class UIGrid : UIView
	{
		
		public static readonly DependencyProperty RowProperty;
		public static readonly DependencyProperty RowSpanProperty;
		public static readonly DependencyProperty ColumnProperty;
		public static readonly DependencyProperty ColumnSpanProperty;
		public List<IDependencyObject> Children;
		static UIGrid()
		{
		// Register the property
			UIGrid.RowProperty = DependencyProperty.Register("Row",
			typeof(int), typeof(UIGrid),new PropertyMetadata(new PropertyChangedCallback(OnRowColumnChanged)));
			UIGrid.RowSpanProperty = DependencyProperty.Register("RowSpan",
			typeof(int), typeof(UIGrid),new PropertyMetadata(new PropertyChangedCallback(OnRowColumnChanged)));
			UIGrid.ColumnProperty = DependencyProperty.Register("Column",
			typeof(int), typeof(UIGrid),new PropertyMetadata(new PropertyChangedCallback(OnRowColumnChanged)));
			UIGrid.ColumnSpanProperty = DependencyProperty.Register("ColumnSpan",
			typeof(int), typeof(UIGrid),new PropertyMetadata(new PropertyChangedCallback(OnRowColumnChanged)));
			
		}
		
		public UIGrid ()
		{
			Children = new List<IDependencyObject>();	
		}
		// A .NET property wrapper (not must)
		public static int GetRow (IDependencyObject element){return (int)element.GetValue(UIGrid.RowProperty );	}
		public static void SetRow(IDependencyObject element, object value){ element.SetValue(UIGrid.RowProperty , value);}
		
		public static int GetRowSpan(IDependencyObject element) { return (int)element.GetValue(UIGrid.RowSpanProperty ); }
		public static void SetRowSpan(IDependencyObject element, object value) { element.SetValue(UIGrid.RowSpanProperty , value); }
		
		public static int GetColumn(IDependencyObject element) { return (int)element.GetValue(UIGrid.ColumnProperty ); }
		public static void SetColumn(IDependencyObject element, object value) { element.SetValue(UIGrid.ColumnProperty , value); }
		
		public static int GetColumnSpan(IDependencyObject element) { return (int)element.GetValue(UIGrid.ColumnSpanProperty ); }
		public static void  SetColumnSpan (IDependencyObject element, object value) { element.SetValue(UIGrid.ColumnSpanProperty , value); }

		private float columnWidth;
		private float rowHeight;
		private int columns;
		private int rows;
		public float ColumnPadding = 5;
		public float RowPadding = 5;
		private bool useCounts;
		public UIGrid ()
		{
			Frame = new RectangleF(0,0,300,480);
			columns = 5;
			rows = 5;
			
			columnWidth = rect.Width / Columns;
			rowHeight = rect.Height / Rows;
			Children = new List<IDependencyObject>();
		}
		public UIGrid (RectangleF rect,int Columns,int Rows) 
		{
			Frame = rect;
			useCounts = true;
			columns = Columns;
			rows = Rows;
			columnWidth = rect.Width / Columns;
			rowHeight = rect.Height / Rows;
			Children = new List<IDependencyObject>();	
		}
		
		public UIGrid (RectangleF rect,float ColumnWidth,float RowHeight)
		{
			Frame = rect;
			columnWidth = ColumnWidth;
			rowHeight = RowHeight;
			Children = new List<IDependencyObject>();	
		}

		public override  RectangleF Frame {
			get {
				return base.Frame;
			}
			set {
				base.Frame = value;	
				reload();
			}
		}
		public override void AddSubview (MonoTouch.UIKit.UIView view)
		{
			base.AddSubview (view);
			if(view is IDependencyObject)
			{
				Children.Add((IDependencyObject)view);
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
			
			foreach(var view in Children)
			{
				var loc = getLocation(GetColumn(view),GetRow(view));
				var size = getSize(GetColumnSpan(view),GetRowSpan(view));
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
		
		public class GridViewItem : MonoTouch.UIKit.UIView
		{
			public int Column {get;set;}
			public int ColumnSpan {get;set;}
			public int Row {get;set;}
			public int RowSpan {get;set;}			
			public MonoTouch.UIKit.UIView View {get ;set;}
		}
		
		private static void OnRowColumnChanged( IDependencyObject o, DependencyPropertyChangedEventArgs e) 
		{
		}
		
	}
}

