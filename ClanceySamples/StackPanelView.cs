using System;
using MonoTouch.UIKit;
using System.Drawing;
using ClanceysLib;
namespace ClanceySamples
{
	public class StackPanelView : UIView
	{
		private UITextField textInput;
		private UILabel label;
		private UIImageView imageView;
		private StackPanel stackPanel;		
		
		public StackPanelView (RectangleF rect) : base (rect)
		{
			this.BackgroundColor = UIColor.Gray;
			stackPanel = new StackPanel(this.Frame);
			stackPanel.StretchWidth = true;
			textInput = new UITextField(new RectangleF(0,0,10,25)){BackgroundColor = UIColor.White};
			
			label = new UILabel(new RectangleF(0,0,10,25)){Text = "label 1"};
			imageView = new UIImageView(Images.Featured);
			
			this.AddSubview(stackPanel);
			
			stackPanel.AddSubview(textInput);
			stackPanel.AddSubview(label);
			stackPanel.AddSubview(imageView);
			
			
		}
	}
}

