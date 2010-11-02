using System;
using System.Drawing;
using System.Collections;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
namespace ClanceysLib
{
	
	public class NavIcon :UIView
	{
		private SizeF Size  = new SizeF(58,85);
		private SizeF lblSize = new SizeF(58,20); 
		private UIImage _image;
		/// <summary>
		/// Auto Resize and round off the image corners.
		/// </summary>
		public bool RoundImage = true;
		/// <summary>
		/// Button Image
		/// </summary>
		
		public UIImage Image 
		{
			get{return _image;}
			set{_image = RoundImage ? Graphics.RemoveSharpEdges(value) : value;}
		}
		public Func<UIResponder> ModalView{get;set;}
		public string Title {get;set;}
		public int NotificationCount {get;set;}
		public NavPage parent;
		private UIButton button;
		private UILabel label;
		
		public NavIcon ()
		{
			this.Frame = new RectangleF(new Point(0,0),Size);
			
		}
		
		
		public void Refresh(PointF location)
		{
			var frame = this.Frame;
			frame.Location = location;
			this.Frame = frame;
			
			var x = (frame.Width - Image.Size.Width) /2;
			button = UIButton.FromType(UIButtonType.Custom);
			button.Frame = new RectangleF(x,0,Size.Width,Size.Width);
			button.SetImage(Image,UIControlState.Normal);	
			button.TouchDown += delegate {
				parent.parent.LaunchModal(ModalView == null ? null : ModalView());
				
			};
			//button.SetImage(Graphics.AdjustImage(button.Frame, Image,CGBlendMode.Normal,UIColor.Blue),UIControlState.Highlighted);
			
			
			this.AddSubview(button);
			var lblLoc = new PointF(0,Image.Size.Height + 5);
			label = new UILabel(new RectangleF(lblLoc,lblSize));
			label.Text = Title;
			label.Font = UIFont.FromName("Arial",11);
			label.TextAlignment = UITextAlignment.Center;
			this.AddSubview(label);
		}

		void HandleBtnTouchDown (object sender, EventArgs e)
		{
			
		}
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
		}
	}
}

