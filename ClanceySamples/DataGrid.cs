using System;
using MonoTouch.UIKit;
using ClanceysLib;
using System.Drawing;
using System.Collections.Generic;
namespace ClanceySamples
{
	public class DataGrid : UIView
	{
		private UIDataGridView gridView;
		private List<MyDataRow> items;
		public DataGrid(RectangleF rect) : base (rect)
		{
			var random = new Random();
			items = new List<MyDataRow>();
			for(int i = 0;i <= 100;i++)
			{
				items.Add(new MyDataRow{
					PrimaryKey = i,	
					ColumnA = "I am column a",
					ColumnB = random.NextDouble(),
					Description = "my random " + random.Next() + " row description",
				});
			}
			
			gridView = new UIDataGridView(rect);
			gridView.Items = items.ToArray();
			this.BackgroundColor = UIColor.White;
			this.AddSubview(gridView);
		}
	}
	
	public class MyDataRow
	{
		[HeaderVisible(true)]
		[ColumnWidth(50)]
		[ColumnWidthTypeAttribute(UIDataGridView.ColumnWidthType.Fixed)]
		public int PrimaryKey {get;set;}
		[HeaderTitle("A")]
		public string ColumnA {get;set;}
		[HeaderTitle("Cost")]
		[ColumnWidthType(UIDataGridView.ColumnWidthType.Fixed)]
		public double ColumnB {get;set;}
		[ColumnWidthType(UIDataGridView.ColumnWidthType.Auto)]
		public string Description {get;set;} 
	}
	
	
}