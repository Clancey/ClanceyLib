using System;
using MonoTouch.UIKit;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;

namespace ClanceysLib
{
	public class UIDataGridView 
	{
		public enum ColumnWidthType
		{
			Auto, Fixed
		}
		private object[] _items;
		public object[] Items {
			get{ return _items;}
			set{
				_items = value;
				UpdateGrid();
			}
		}
		public float RowHeight = 20f;
		public float RowHeaderWidth = 30f;
		private ScrollViewWithHeader _scrollView;
		private RectangleF _rect;
		
		private bool _includeHeader;
		public bool IncludeRowHeader {
			get {
				return _includeHeader;
			}
			set{
				_includeHeader = value;
			}
		}
		
		private bool _autoGenerateColumns;
		public bool AutoGenerateColumns {
			get{ return _autoGenerateColumns;}
			set{ 
				_autoGenerateColumns = value;
				UpdateGrid();
			}
		}
		
		private List<UIDataGridColumn> _columns;
		public List<UIDataGridColumn> Columns {
			get { return _columns;}
			set {
				_columns = value;
				UpdateGrid();
			}
		}
		private List<UIDataGridRow> _rows;
		public List<UIDataGridRow> Rows{
			get { return _rows;}
			set {
				_rows = value;
			}
		}
		private UIDataGridRow headerRow;
		
		private UIView header;
		private UIView content;
		private UIView rHeader;
		
		public UIDataGridView (RectangleF rect)
		{
			_rect = rect;
			header = new UIView();
			content = new UIView();
			rHeader = new UIView();
			AutoGenerateColumns = true;
			_scrollView = new ScrollViewWithHeader(rect,header,content,true);
		}
		
		public static implicit operator UIView (UIDataGridView control)
		{
			return control._scrollView;
		}
		
		public void UpdateGrid()
		{
			if(_items == null || _items.Length == 0)
				return;
			generateColumns();
			_rows = new List<UIDataGridRow>();
			int curIndex = 0;
			foreach(var item in _items)
			{
				_rows.Add(generateRow(item,curIndex));	
				curIndex ++;
			}
			var maxX = Rows.Max(x=> x.MaxX);
			var maxY = Rows.Max(y=> y.MaxY);
			
			header = new UIView(new RectangleF(0,0,maxX,RowHeight));
			rHeader = new UIView(new RectangleF(0,0,RowHeaderWidth,maxY));
			
			foreach( var col in headerRow.Cells)
			{
				header.AddSubview(col);
			}
			content = new UIView(new RectangleF(0,0,maxX, maxY));
			foreach(var row in _rows)
			{
				rHeader.AddSubview(new UILabel(new RectangleF(0,RowHeight * row.RowIndex,RowHeaderWidth,RowHeight)){Text = (row.RowIndex + 1).ToString()});
				foreach(var cell in row.Cells)
				{
					content.AddSubview(cell);	
				}
			}
			
			_scrollView = new ScrollViewWithHeader(_rect,header,rHeader,content,true);
			
			
		}
		
		private UIDataGridRow generateRow(object item, int rowIndex)
		{
			UIDataGridRow row = new UIDataGridRow() { RowIndex = rowIndex};
			var height = rowIndex * RowHeight;
			int curIndex = 0;
			float pading = 5f;
			float curWidth = pading;
			if(rowIndex == 0)
				headerRow = new UIDataGridRow();
			foreach(var col in _columns)
			{
				row.Cells.Add(new UIDataGridCell{
					Text = getPropertyValue(item,col.DisplayMember),
					Column = curIndex,
					Row  = rowIndex,
					Frame = new RectangleF(curWidth,height,col.Width,RowHeight),
				});
				if(rowIndex == 0)
					headerRow.Cells.Add(new UIDataGridCell() {
						Frame= new RectangleF(curWidth,height,col.Width,RowHeight),
						Column = curIndex,
						Text = col.Header});
				
				curWidth += col.Width + pading;
				row.MaxY = height * (rowIndex + 1);
				row.MaxX = curWidth;
				curIndex ++;
			}
			return row;
		}
		
		private void generateColumns()
		{
			if(!AutoGenerateColumns || _items == null || _items.Count() == 0)
				return;
			var item = _items[0];
			_columns = new List<UIDataGridColumn>();
			int colIndex = 0;
			foreach (var prop in  item.GetType().GetProperties())
			{
				var col = generateColumn(prop);
				if(col != null)
				{
					col.ColIndex = colIndex;
					_columns.Add(col);
				}
				colIndex ++;
			}
			
		}
		private UIDataGridColumn generateColumn(PropertyInfo prop)
		{
			var attribtues = prop.GetCustomAttributes(false);
			var visibleAtt = attribtues.Where(x=> x is HeaderVisibleAttribute).FirstOrDefault() as HeaderVisibleAttribute;
			if(visibleAtt != null && !visibleAtt.Visible)
				return null;
			var widthAtt = attribtues.Where(x=> x is ColumnWidthAttribute).FirstOrDefault() as ColumnWidthTypeAttribute;
			var widthAttType = attribtues.Where(x => x is ColumnWidthTypeAttribute).FirstOrDefault() as ColumnWidthTypeAttribute;
			var headerAtt = attribtues.Where(x=> x is HeaderTitleAttribute).FirstOrDefault() as HeaderTitleAttribute;
			
			var columnWidthType = widthAttType == null ? ColumnWidthType.Fixed : widthAttType.ColumnWidthType;
			var width = columnWidthType == UIDataGridView.ColumnWidthType.Fixed ? 100 : getMaxWidth(prop.Name);
			var header = headerAtt == null ? prop.Name : headerAtt.Title;
			
			
			return new UIDataGridColumn{
				ColumnWidthType = columnWidthType,
				Width = width,
				Header = header,
				DisplayMember = prop.Name,
				
			};
		}
		
		private float getMaxWidth(string propertyName)
		{
			// bad hack, should be fixed to account for the real drawing size
			var len = _items.Max(x=> getPropertyValue(x,propertyName).Length);
			var width = len * 10;
			return width;
		}
			                           
		private string getPropertyValue (object inObject, string propertyName)
		{
			PropertyInfo[] props = inObject.GetType ().GetProperties ();
			PropertyInfo prop = props.Select (p => p).Where (p => p.Name == propertyName).FirstOrDefault ();
			if (prop != null)
				return prop.GetValue (inObject, null).ToString ();
			return "";
		}
	}
	
	public class UIDataGridCell : UILabel
	{
		public int Column {get;set;}
		public int Row {get;set;}
	}
	
	public class UIDataGridRow 
	{
		public int RowIndex {get;set;}
		public List<UIDataGridCell> Cells {get;set;}	
		public float MaxY {get;set;}
		public float MaxX {get;set;}
		public UIDataGridRow()
		{
			Cells = new List<UIDataGridCell>();	
		}
	}
	
	public class UIDataGridColumn
	{
		public string Header {get;set;}
		public string DisplayMember {get;set;}
		public float Width {get;set;}
		public bool Visible {get;set;}
		public int ColIndex {get;set;}
		public UIDataGridView.ColumnWidthType ColumnWidthType {get;set;}
		
		public UIDataGridColumn()
		{
			ColumnWidthType = UIDataGridView.ColumnWidthType.Auto;
			Visible = true;
			Width = 100;
		}
		
	}
}

