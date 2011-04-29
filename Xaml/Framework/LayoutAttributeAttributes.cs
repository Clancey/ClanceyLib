using System;
namespace ClanceysLib
{
	[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class LayoutAttribute : System.Attribute
	{
		public int Column { get; set; }
		public int Row { get; set; }

		public LayoutAttribute (int column, int row)
		{
			Column = column;
			Row = row;
		}
	}
	[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class SpanAttribute : System.Attribute
	{
		public int ColumnSpan { get; set; }
		public int RowSpan { get; set; }

		public SpanAttribute (int Columns, int Rows)
		{
			ColumnSpan = Columns;
			RowSpan = Rows;
		}
	}
}

