using System;
using MonoTouch.UIKit;
using ClanceysLib;
using System.Drawing;
namespace ClanceySamples
{
	public class GridView : UIView
	{
		[Layout(4,1)]
		[Span(3,1)]
		public UITextField textInput;
		[Layout(1,1)]
		[Span(3,1)]
		public UILabel label;
		[Layout(1,2)]
		[Span(6,6)]
		public UIImageView imageView;
		private UIGrid grid;	
		
		public GridView (RectangleF rect) : base (rect)
		{
			this.BackgroundColor = UIColor.Gray;
			// pass in Number of collumns/rows
			grid = new UIGrid(this.Frame,6,10);
			
			//pass in collumn/row width/height
			//grid = new UIGrid(this.Frame,30f,30f);
			
			textInput = new UITextField(){BackgroundColor = UIColor.White};
			
			label = new UILabel(){Text = "label 1"};
			imageView = new UIImageView(Images.Featured);
			
			this.AddSubview(grid);
			
			grid.AddSubview(textInput);
			grid.AddSubview(label);
			grid.AddSubview(imageView);
			
			
		}
		//Override to pass the value to resize the grid as well.
		public override RectangleF Frame {
			get {
				return base.Frame;
			}
			set {
				if(grid != null)
					grid.Frame = value;
				base.Frame = value;
			}
		}
	}
}

