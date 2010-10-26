using System;
using System.Collections.Generic;
using System.Threading;
using MonoTouch.Foundation;
using System.IO;
using MonoTouch.UIKit;

namespace ClanceysLib
{

	/// <summary>
	/// Static downloader class. 
	/// This class will download files one at a time. 
	/// If the app is closed or the lock screen is enabled, the app will continue to dowload the remaining files.
	/// If the app runs out of the alloted background time, It will alert the user to reopen the app to avoid disruption.
	/// 
	/// Use:
	/// To use just call Download.Add();
	/// </summary>
	public static class Downloader
	{
		private static bool _isBusy;
		private static List<string> remainingFiles = new List<string> ();
		private static int bgTask = 0;
		private static ProgressDialog progressDialog = null;
		private static bool _cancel = false;
		private static bool _ShowProgress = true;
		static readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim ();

		private static NSString invoker = new NSString ("");

		private static UIApplication app = UIApplication.SharedApplication;

		public static void StartDownload ()
		{
			
			// If already downloading do nothing...
			if (isBusy)
				return;
			//If the list is empty do nothing...
			if (Remaining == 0)
			{
				isBusy = false;
				return;
			}
			cancel = false;
			isBusy = true;
			
			Thread thread = new Thread (new ThreadStart (startDownloading));
			thread.Start ();
			
		}

		private static void startDownloading ()
		{
			//Thread gc...
			using (NSAutoreleasePool pool = new NSAutoreleasePool ())
			{
				Console.WriteLine ("Starting the downloading process");
				downloadAllFiles ();
			}
		}


		public static int Remaining {
			get {
				_locker.EnterReadLock ();
				try
				{
					return remainingFiles.Count;
				}
				finally
				{
					_locker.ExitReadLock ();
				}
			}
		}
		public static bool isBusy {
			get {
				_locker.EnterReadLock ();
				try
				{
					return _isBusy;
				}
				finally
				{
					_locker.ExitReadLock ();
				}
			}
			private set {
				_locker.EnterWriteLock ();
				try
				{
					_isBusy = value;
				}
				finally
				{
					_locker.ExitWriteLock ();
				}
				
			}
		}

		public static bool ShowProgress {
			get {
				_locker.EnterReadLock ();
				try
				{
					return _ShowProgress;
				}
				finally
				{
					_locker.ExitReadLock ();
				}
			}
			set {
				_locker.EnterWriteLock ();
				try
				{
					_ShowProgress = value;
				}
				finally
				{
					_locker.ExitWriteLock ();
				}
				
			}
		}

		private static bool cancel {
			get {
				_locker.EnterReadLock ();
				try
				{
					return _cancel;
				}
				finally
				{
					_locker.ExitReadLock ();
				}
			}
			set {
				_locker.EnterWriteLock ();
				try
				{
					_cancel = value;
				}
				finally
				{
					_locker.ExitWriteLock ();
				}
				
			}
		}
		private static void downloadAllFiles ()
		{
			if (Remaining == 0 || cancel)
			{
				downloadComplete ();
				return;
			}
			if (bgTask == 0)
				bgTask = app.BeginBackgroundTask (delegate {
					outOfTime ();
					Console.WriteLine ("Didnt update on time...");
				});
			
			downloadFile (getFilePath (0), delegate {
				if (Remaining == 0 || cancel)
				{
					downloadComplete ();
				}

				else
					downloadAllFiles ();
				
			});
		}

		private static void downloadComplete ()
		{
			Console.WriteLine ("Downloads Complete");
			setStatus (statusType.Completed, "");
			isBusy = false;
			app.EndBackgroundTask (bgTask);
			bgTask = 0;
		}

		private static void downloadFile (string filePath, Action completed)
		{
			
			Console.WriteLine ("Starting: " + filePath);
			
			try
			{
				var fileName = Path.GetFileNameWithoutExtension (filePath);
				setStatus (statusType.Update, fileName);
				//TODO: Remote Sleeper Add your code to download the file or process it in some way.
				
				// simulates the time it would take to download something
				Thread.Sleep (6000);
				
				//Remove after download is complete
				removeFile (filePath);
				
				
			}
			catch (Exception ex)
			{
				setStatus (statusType.Error, ex.Message);
				StopDownloading ();
			}
			//Tell the other thread your done....
			Console.WriteLine ("Completed: " + filePath);
			if (completed != null)
				completed ();
			
		}

		private enum statusType
		{
			Update,
			Error,
			Completed
		}

		private static void setStatus (statusType status, string inStatus)
		{
			if (!ShowProgress)
			{
				if (progressDialog != null)
				{
					progressDialog.dispose ();
					progressDialog = null;
				}
				return;
			}
			invoker.InvokeOnMainThread (delegate {
				if (progressDialog == null)
				{
					progressDialog = new ProgressDialog ("Downloading", delegate {
						progressDialog.setMessage ("Canceling...");
						StopDownloading ();
					});
				}
				if (status == Downloader.statusType.Completed)
				{
					// successful completion
					// tell the user all set
					progressDialog.dispose ();
					progressDialog = null;
					UIAlertView alert = new UIAlertView ("Success", "Finished update.", null, "OK");
					alert.Show ();
				}

				else if (status == statusType.Update)
				{
					// no errors - normal status update
					//this was s
					progressDialog.setMessage (Path.GetFileNameWithoutExtension (inStatus));
				}
				else
				{
					progressDialog.dispose ();
					progressDialog = null;
					// some error occurred
					// tell the user about it
					
					UIAlertView alert = new UIAlertView ("Error", inStatus, null, "OK");
					alert.Show ();
					
				}
			});
		}

		public static void StopDownloading ()
		{
			cancel = true;
		}

		public static void AddFile (string filePath)
		{
			_locker.EnterUpgradeableReadLock ();
			try
			{
				if (!remainingFiles.Contains (filePath))
				{
					_locker.EnterWriteLock ();
					try
					{
						remainingFiles.Add (filePath);
					}
					finally
					{
						_locker.ExitWriteLock ();
					}
				}
				StartDownload ();
			}
			finally
			{
				_locker.ExitUpgradeableReadLock ();
			}
			
		}

		private static void removeFile (string filePath)
		{
			_locker.EnterWriteLock ();
			try
			{
				remainingFiles.Remove (filePath);
			}
			finally
			{
				_locker.ExitWriteLock ();
			}
		}

		private static string getFilePath (int index)
		{
			_locker.EnterReadLock ();
			try
			{
				return remainingFiles[index];
			}
			finally
			{
				_locker.ExitReadLock ();
			}
			
		}
		/// <summary>
		/// This will send a local push notification warning the user the download 
		/// is not complete and will be canceled if they dont reopen the app, If
		/// opened on time the download will continue perfectly.
		/// </summary>
		private static void outOfTime ()
		{
			var notify = new UILocalNotification ();
			notify.AlertAction = "Ok";
			notify.AlertBody = "Your Download is about to be canceled!";
			notify.HasAction = true;
			notify.SoundName = UILocalNotification.DefaultSoundName;
			notify.FireDate = NSDate.Now;
			notify.TimeZone = NSTimeZone.DefaultTimeZone;
			
			//NSDictionary param = NSDictionary.FromObjectsAndKeys(objs,keys);
			//notify.UserInfo = param;
			app.ScheduleLocalNotification (notify);
			
			Console.WriteLine ("out of time:" + app.BackgroundTimeRemaining);
		}

		private class ProgressDialog
		{

			UIAlertView myAlert;

			public ProgressDialog (string title, EventHandler<MonoTouch.UIKit.UIButtonEventArgs> Clicked)
			{
				
				myAlert = new UIAlertView ();
				
				myAlert.Title = title;
				myAlert.Message = "In progress...";
				myAlert.Clicked += Clicked;
				
				myAlert.AddButton ("Cancel");
				
				myAlert.Show ();
			}

			public void dispose ()
			{
				
				// dismiss with button clicked so if invoked on another thread we don't die for some reason
				myAlert.DismissWithClickedButtonIndex (0, true);
				myAlert.Dispose ();
				myAlert = null;
			}

			public void setMessage (string msg)
			{
				myAlert.Message = msg;
				
				Console.WriteLine ("setting message: " + msg);
			}
		}
		
	}
}

