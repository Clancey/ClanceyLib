using System;
namespace ClanceysLib
{
	[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class HeaderVisibleAttribute : System.Attribute
	{
		public bool Visible { get; set; }

		public HeaderVisibleAttribute (bool visible)
		{
			Visible = visible;
		}
	}
	
	[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class ColumnWidthAttribute : System.Attribute
	{
		public float Width { get; set; }

		public ColumnWidthAttribute (float width )
		{
			Width = width;
		}
	}
	
	[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class HeaderTitleAttribute : System.Attribute
	{
		public string Title { get; set; }

		public HeaderTitleAttribute (string title )
		{
			Title = title;
		}
	}
	
	[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class ColumnWidthTypeAttribute : System.Attribute
	{
		public UIDataGridView.ColumnWidthType ColumnWidthType { get; set; }

		public ColumnWidthTypeAttribute (UIDataGridView.ColumnWidthType columnWidthType )
		{
			ColumnWidthType = columnWidthType;
		}
	}
	
	
	

}

