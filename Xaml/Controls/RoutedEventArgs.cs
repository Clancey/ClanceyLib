//
// RoutedEventArgs.cs
//
// Copyright 2008 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Mono;
using System.Collections.Generic;

namespace System.Windows {

	public class RoutedEventArgs : EventArgs, IRefContainer {

		//DependencyObjectHandle handle;
		object source;
		Dictionary<IntPtr,object> strongRefs;


		


		void IRefContainer.AddStrongRef (IntPtr id, object value)
		{
#if DEBUG
			if (id == IntPtr.Zero)
				Console.WriteLine ("Moon Error: RoutedEventArgs.AddStrongRef was called with an invalid ID with value: {0}", value);
#endif
			AddStrongRef (id, value);
		}

		internal virtual void AddStrongRef (IntPtr id, object value)
		{
		//	if (id == (IntPtr) WeakRefs.RoutedEventArgs_Source) {
		//		source = value;
		//	} else {
				if (strongRefs == null)
					strongRefs = new Dictionary<IntPtr, object> ();
				else if (strongRefs.ContainsKey (id))
					return;

				if (value != null) {
#if DEBUG_REF
					Console.WriteLine ("Adding ref from {0}/{1} to {2}/{3}", GetHashCode(), this, value.GetHashCode(), value);
#endif
					strongRefs.Add (id, value);
				}
		//	}
		}

		void IRefContainer.ClearStrongRef (IntPtr id, object value)
		{
#if DEBUG
			if (id == IntPtr.Zero)
				Console.WriteLine ("Moon Error: RoutedEventArgs.ClearStrongRef was called with an invalid ID with value: {0}", value);
#endif
			ClearStrongRef (id, value);
		}

		internal virtual void ClearStrongRef (IntPtr id, object value)
		{
			//if (id == (IntPtr) WeakRefs.RoutedEventArgs_Source) {
			//	source = null;
			//} else if (strongRefs != null) {
#if DEBUG_REF
				Console.WriteLine ("Clearing ref from {0}/{1} to {2}/{3}", GetHashCode(), this, value.GetHashCode(), value);
#endif
				strongRefs.Remove (id);
			//}
		}

#if HEAPVIZ
		System.Collections.ICollection IRefContainer.GetManagedRefs ()
		{
			List<HeapRef> refs = new List<HeapRef> ();
			if (strongRefs != null)
				foreach (var keypair in strongRefs)
					if (keypair.Value is INativeEventObjectWrapper)
						refs.Add (new HeapRef (true, (INativeEventObjectWrapper)keypair.Value, NativeDependencyObjectHelper.IdToName (keypair.Key)));
			return refs;
		}
#endif




		public object OriginalSource {
			get {
				return source;
			}

			internal set {
				if (value == null) {
					//NativeMethods.routed_event_args_set_source (NativeHandle, IntPtr.Zero);
				} else {
					DependencyObject v = value as DependencyObject;
					if (v == null)
						throw new ArgumentException ();

					//NativeMethods.routed_event_args_set_source (NativeHandle, v.native);
				}
			}
		}


	}
}
