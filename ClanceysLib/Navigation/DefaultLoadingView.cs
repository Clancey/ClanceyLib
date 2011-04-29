using System;
using MonoTouch.UIKit;
namespace ClanceysLib
{
	public class DefaultLoadingView : UIViewController
	{
		MBProgressHUD loading;
		public DefaultLoadingView ()
		{
			loading = new MBProgressHUD();
			loading.TitleText = "Loading";
		}
		public override void ViewDidLoad ()
		{
			loading.Show(false);
		}
	}
}

