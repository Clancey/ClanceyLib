//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
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
using System.Windows;
using Mono;
using System.Drawing;
using System.Collections.Generic;

namespace System.Windows.Controls {

	public partial class Grid : Panel {
		
		public static readonly DependencyProperty RowProperty;
		public static readonly DependencyProperty RowSpanProperty;
		public static readonly DependencyProperty ColumnProperty;
		public static readonly DependencyProperty ColumnSpanProperty;
		public List<IDependencyObject> Children;
		static Grid()
		{
		// Register the property
			Grid.RowProperty = DependencyProperty.Register("Row",
			typeof(int), typeof(Grid),new PropertyMetadata());
			Grid.RowSpanProperty = DependencyProperty.Register("RowSpan",
			typeof(int), typeof(Grid),new PropertyMetadata());
			Grid.ColumnProperty = DependencyProperty.Register("Column",
			typeof(int), typeof(Grid),new PropertyMetadata());
			Grid.ColumnSpanProperty = DependencyProperty.Register("ColumnSpan",
			typeof(int), typeof(Grid),new PropertyMetadata());
			
		}
		
		public static int GetColumn (IDependencyObject element)
		{
			return (int) element.GetValue (ColumnProperty);
		}

		public static int GetColumnSpan (IDependencyObject element)
		{
			return (int) element.GetValue (ColumnSpanProperty);
		}

		public static int GetRow (IDependencyObject element)
		{
			return (int) element.GetValue (RowProperty);
		}

		public static int GetRowSpan (IDependencyObject element)
		{
			return (int) element.GetValue (RowSpanProperty);
		}

		public static void SetColumn (IDependencyObject element, int value)
		{
			element.SetValue (ColumnProperty, value);
		}

		public static void SetColumnSpan (IDependencyObject element, int value)
		{
			element.SetValue (ColumnSpanProperty, value);
		}

		public static void SetRow (IDependencyObject element, int value)
		{
			element.SetValue (RowProperty, value);
		}

		public static void SetRowSpan (IDependencyObject element, int value)
		{
			element.SetValue (RowSpanProperty, value);
		}

		protected virtual Size ArrangeOverride (Size arrangeSize)
		{
			return Size.Empty;
		}

		protected virtual Size MeasureOverride (Size constraint)
		{
			return Size.Empty;
		}
	}
}
