using System;
using System.Drawing;
using System.Collections;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using System.Threading;
namespace ClanceysLib
{
	
	public class NavIcon :UIView
	{
		//private SizeF Size  = new SizeF(58,85);
		//private SizeF lblSize = new SizeF(58,20); 
		private float padding = 5f;
		private float TextFontSize =11f;
		/// <summary>
		/// Auto Resize and round off the image corners.
		/// </summary>
		public bool RoundImage = false;
		public bool KeepImageAspectRation = true;
		public float ColumnWidth{get;set;}
		public float RowHeight {get;set;}
		public UIImage Image {get;set;}
		public Func<UIResponder> ModalView{get;set;}
		public string Title {get;set;}
		public NavPage parent;
		private UIButton button;
		private UILabel label;
		private NavIconBadge badge;
		private MBProgressHUD loading;
		public bool ShowLoadingMessage {get;set;}
		
		public int NotificationCount
		{
			get{return badge.BadgeNumber;}
			set {badge.BadgeNumber = value;}
		}
		
		public NavIcon ()
		{
			this.Frame = new RectangleF(0,0,0,0);
			badge = new NavIconBadge();
			badge.TouchDown += delegate {				
				loadModal();
			};
			//badge.BadgeColor = UIColor.FromRGBA(129f, 0f, 0f, 1.000f);
			//loading.TitleText = "Loading";
		}
		
		
		public void Refresh(PointF location)
		{
			ClearView();
			//Console.WriteLine("refreshing " + this.Title);
			var frame = this.Frame;
			frame.Location = location;
			frame.Height = RowHeight;
			frame.Width = ColumnWidth;
			this.Frame = frame;
			var imageH = RowHeight - padding - (TextFontSize + 5);
			var image = Graphics.ResizeImage(new SizeF(ColumnWidth,imageH),Image,KeepImageAspectRation);
			//var image = Image;
			if(RoundImage)
				image = Graphics.RemoveSharpEdges(image);
			
			var x = (frame.Width - image.Size.Width) /2;
			button = UIButton.FromType(UIButtonType.Custom);
			//Console.WriteLine("imageH :" + imageH);
			//Console.WriteLine("imageSize : " + image.Size);
			//Console.WriteLine("row : " + RowHeight);
			var y = (float)(Math.Round(imageH) - image.Size.Height);
			//Console.WriteLine("y:" + y);
			button.Frame = new RectangleF(x,y,image.Size.Width,image.Size.Width);
			badge.SetLocation(new PointF(button.Frame.Width - badge.Frame.Width,y),false);
			button.SetImage(Image,UIControlState.Normal);	
			//button.Layer.MasksToBounds = false;
			//self.layer.cornerRadius = 8; // if you like rounded corners
			//button.Layer.ShadowOffset = new SizeF(5f, 5f);
			//button.Layer.ShadowRadius = 3;
			//button.Layer.ShadowOpacity = 0.5f;
			button.TouchDown += delegate {
				loadModal();
			};
			//button.SetImage(Graphics.AdjustImage(button.Frame, Image,CGBlendMode.Normal,UIColor.Blue),UIControlState.Highlighted);
			
			
			this.AddSubview(button);
			var lblLoc = new PointF(0,image.Size.Height + 5);
			label = new UILabel(new RectangleF(lblLoc,new SizeF(ColumnWidth,TextFontSize)));
			label.BackgroundColor = UIColor.Clear;
			label.Text = Title;
			label.Font = UIFont.FromName("Arial",TextFontSize);
			label.TextAlignment = UITextAlignment.Center;
			this.AddSubview(label);
			this.AddSubview(badge);
		}
		
		private void ClearView()
		{
			foreach(var view in this.Subviews)
			{
				view.RemoveFromSuperview();	
			}
		}
		UIResponder modalview;
		private void loadModal()
		{
			Thread thread = new Thread(loadOnThread);
			thread.Start();
		
		}
		private void loadOnThread()
		{
			using(new NSAutoreleasePool())
			{
				if(ShowLoadingMessage)
				{
					loading = new MBProgressHUD();
					loading.Show(true);
				}
				modalview = ModalView == null ? null : ModalView();
				InvokeOnMainThread(delegate {loadComplete();});
			}
		}
		
		private void loadComplete()
		{
		
			parent.parent.LaunchModal(modalview);
			if(ShowLoadingMessage)
				loading.Hide(true);
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

