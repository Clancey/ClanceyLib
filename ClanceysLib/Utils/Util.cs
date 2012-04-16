//   Licensed to the Apache Software Foundation (ASF) under one
//        or more contributor license agreements.  See the NOTICE file
//        distributed with this work for additional information
//        regarding copyright ownership.  The ASF licenses this file
//        to you under the Apache License, Version 2.0 (the
//        "License"); you may not use this file except in compliance
//        with the License.  You may obtain a copy of the License at
// 
//          http://www.apache.org/licenses/LICENSE-2.0
// 
//        Unless required by applicable law or agreed to in writing,
//        software distributed under the License is distributed on an
//        "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
//        KIND, either express or implied.  See the License for the
//        specific language governing permissions and limitations
//        under the License.
using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;
using MonoTouch.Foundation;


namespace ClanceysLib
{
	public static class Util
	{	
		public static UIApplication MainApp = UIApplication.SharedApplication;
		static object networkLock = new object ();
		static int active;
			
		public static string GetPropertyValue (object inObject, string propertyName)
		{
			PropertyInfo[] props = inObject.GetType ().GetProperties ();
			PropertyInfo prop = props.Select (p => p).Where (p => p.Name == propertyName).FirstOrDefault ();
			if (prop != null)
				return prop.GetValue (inObject, null).ToString ();
			return "";
		}
		
		public static object[] GetPropertyArray (object inObject, string propertyName)
		{
			PropertyInfo[] props = inObject.GetType ().GetProperties ();
			PropertyInfo prop = props.Select (p => p).Where (p => p.Name == propertyName).FirstOrDefault ();
			if (prop != null)
			{
				var currentObject = prop.GetValue (inObject, null);
				if (currentObject.GetType ().GetGenericTypeDefinition () == typeof(List<>))
				{
					return (new ArrayList ((IList)currentObject)).ToArray ();
				}

				else if (currentObject is Array)
				{
					return (object[])currentObject;
				}
				else
				{
					return new object[1];
				}
			}
			return new object[1];
		}

		public static void PushNetworkActive ()
		{
			lock (networkLock) {
				active++;
				MainApp.NetworkActivityIndicatorVisible = true;
			}
		}

		public static void PopNetworkActive ()
		{
			lock (networkLock) {
				active--;
				if (active == 0)
					MainApp.NetworkActivityIndicatorVisible = false;
			}
		}

		
		public static UIImage FromResource (Assembly assembly, string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			assembly = Assembly.GetCallingAssembly ();
			var stream = assembly.GetManifestResourceStream (name);
			if (stream == null)
				return null;
			
			IntPtr buffer = Marshal.AllocHGlobal ((int) stream.Length);
			if (buffer == IntPtr.Zero)
				return null;
			
			var copyBuffer = new byte [Math.Min (1024, (int) stream.Length)];
			int n;
			IntPtr target = buffer;
			while ((n = stream.Read (copyBuffer, 0, copyBuffer.Length)) != 0){
				Marshal.Copy (copyBuffer, 0, target, n);
				target = (IntPtr) ((int) target + n);
			}
			try {
				var data = NSData.FromBytes (buffer, (uint) stream.Length);
				return UIImage.LoadFromData (data);
			} finally {
				Marshal.FreeHGlobal (buffer);
				stream.Dispose ();
			}
		}
	
	}
}

