// 
//  Copyright 2011  Clancey
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using MonoTouch.MediaPlayer;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;

namespace ClanceysLib
{
	[Register("MyMoviePlayer")]
	public class MyMoviePlayer : UIView
	{
		MPMoviePlayerController mp;
		UILabel lblLoading;
		public NSUrl MovieUrl {get;set;}
		public NSAction Done {get;set;}

		public void Play()
		{
			mp.Play();
		}

		private NSObject notificationObserver;

		public MyMoviePlayer (IntPtr handle) : base(handle)
		{
		}

		[Export("initWithCoder:")]
		public MyMoviePlayer (NSCoder coder) : base(coder)
		{
		}

		public MyMoviePlayer (RectangleF rect): base (rect) {
		}

		public MyMoviePlayer () {}



		public override void WillMoveToSuperview (UIView newsuper)
		{
			if (newsuper == null)
				return;
			this.BackgroundColor = UIColor.Black;
			lblLoading= new UILabel(new RectangleF(20,20,100,100));
			lblLoading.BackgroundColor = UIColor.Clear;
			lblLoading.Text = "Loading";
			lblLoading.TextColor = UIColor.White;
			lblLoading.Font = UIFont.FromName ("Helvetica", 17f);
			this.AddSubview(lblLoading);
			notificationObserver  = NSNotificationCenter.DefaultCenter
					.AddObserver("MPMoviePlayerPlaybackDidFinishNotification", WillExitFullScreen );
			mp = new MPMoviePlayerController (MovieUrl);
			mp.ControlStyle = MPMovieControlStyle.Fullscreen;
	        mp.View.Frame = this.Bounds;		
			mp.SetFullscreen(true,true);
			this.AddSubview(mp.View);	
		}

		private void WillExitFullScreen( NSNotification notification)
		{
			if (Done != null)
				Done();
		}

		public override void RemoveFromSuperview ()
		{
			lblLoading.RemoveFromSuperview();
			mp.View.RemoveFromSuperview();
			mp.Dispose();
			NSNotificationCenter.DefaultCenter.RemoveObserver (notificationObserver);
			base.RemoveFromSuperview ();
		}

	}
}



