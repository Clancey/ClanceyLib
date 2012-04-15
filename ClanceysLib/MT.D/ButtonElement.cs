using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
namespace ClanceysLib
{
	public class ButtonElement : Element
	{
		static NSString skey = new NSString ("ButtonElement");
		public UITextAlignment Alignment = UITextAlignment.Center;
		public event NSAction Tapped;
		public UIColor Color;
		public UIColor TitleColor;
		public ButtonElement (string caption,UIColor color) : base (caption)
		{
			Color = color;
		}
		public ButtonElement (string caption,UIColor color, NSAction tapped) : base (caption)
		{
			Color = color;
			Tapped += tapped;
		}
				
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (skey) as ButtonCellView;
			if (cell == null)
				cell = new ButtonCellView(this);
			else
				cell.UpdateFrom(this);
			return cell;
		}

		public override string Summary ()
		{
			return Caption;
		}
		
		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, true);
		}
		
		public class ButtonCellView : UITableViewCell {
			UIGlassyButton btn;		
			ButtonElement parent;
					
			public ButtonCellView (ButtonElement element) : base (UITableViewCellStyle.Value1, skey)
			{
				parent = element;
				this.BackgroundColor = UIColor.Clear;
				btn = new UIGlassyButton(RectangleF.Empty);
				btn.Color = parent.Color;
				btn.TitleColor = parent.TitleColor;
				btn.Title = element.Caption;
				btn.TouchUpInside += delegate{
					if(parent.Tapped != null)
						parent.Tapped();
				};
				ContentView.Add (btn);
			} 
			public override void LayoutSubviews ()
			{
				var frame = ContentView.Bounds.Add(new PointF(2,2),new SizeF(-4,-4));
				
				btn.Frame = frame;
				base.LayoutSubviews ();
			}
			
			public void UpdateFrom (ButtonElement element)
			{
				btn.Title = element.Caption;
				parent = element;
			}
			
		}
	}
}

