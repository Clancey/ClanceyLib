//
// System.Windows.UIElement.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2007 Novell, Inc.
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

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using Mono;
using System.Drawing;

namespace System.Windows {
	public abstract partial class UIElement : DependencyObject {

		static UIElement ()
		{
			UIElement.VisibilityProperty.AddPropertyChangeCallback (VisibilityPropertyChanged);
		}

		static void VisibilityPropertyChanged (DependencyObject sender,
		                                       DependencyPropertyChangedEventArgs args)
		{
			((UIElement) sender).RaiseUIAVisibilityChanged (args);
		}

		internal event EventHandler VisualParentChanged;



		public bool CaptureMouse ()
		{
			return NativeMethods.uielement_capture_mouse (native);
		}

		public void ReleaseMouseCapture ()
		{
			NativeMethods.uielement_release_mouse_capture (native);
		}

		public void Arrange (RectangleF finalRect)
		{
			if (finalRect.IsEmpty)
				throw new InvalidOperationException ("Empty Rect");

			if (Double.IsInfinity (finalRect.Width) || Double.IsInfinity (finalRect.Height) || Double.IsInfinity (finalRect.X) || Double.IsInfinity (finalRect.Y))
				throw new InvalidOperationException ("Infinite Rect");
			if (Double.IsNaN (finalRect.Width) || Double.IsNaN (finalRect.Height) || Double.IsNaN (finalRect.X) || Double.IsNaN (finalRect.Y))
				throw new InvalidOperationException ("NaN Rect");

			NativeMethods.uielement_arrange(native, finalRect);
		}

		public void InvalidateArrange ()
		{
			NativeMethods.uielement_invalidate_arrange(native);
		}

		public void Measure (Size availableSize)
		{
			NativeMethods.uielement_measure (native, availableSize);
		}

		internal DependencyObject SubtreeObject {
			get; private set;
		}

		internal UIElement VisualParent {
			get; set;
		}

		public void InvalidateMeasure ()
		{
			NativeMethods.uielement_invalidate_measure (native);
		}

		public void UpdateLayout ()
		{
			NativeMethods.uielement_update_layout (native);
		}


		public Size DesiredSize {
			get {
				return NativeMethods.uielement_get_desired_size (native);
			}
		}

		public Size RenderSize {
			get {
				return NativeMethods.uielement_get_render_size (native);
			}
		}

		static void EnsureDesignMode ()
		{
			// NOTE:
			// If you ended up here from drt 539 you should know
			// it doesn't pass on Silverlight but MobileStubsTests
			// does so tread lightly

			if (//Int32.Parse (Deployment.Current.RuntimeVersion.Split('.')[0]) < 4 &&
			    !DesignerProperties.GetIsInDesignMode (Application.Current.RootVisual))
				throw new NotImplementedException ();
		}



		#region UIA Events

		internal void RaiseUIAVisibilityChanged (DependencyPropertyChangedEventArgs args)
		{
			if (UIAVisibilityChanged != null)
				UIAVisibilityChanged (this, args);
		}

		#endregion
	}
}
