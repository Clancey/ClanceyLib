
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows;
using System;
using System.Drawing;

namespace XamlControls
{

	public partial class UIView : MonoTouch.UIKit.UIView , IDependencyObject
	{		
		public UIView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIActionSheet : MonoTouch.UIKit.UIActionSheet , IDependencyObject
	{		
		public UIActionSheet () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIAlertView : MonoTouch.UIKit.UIAlertView , IDependencyObject
	{		
		public UIAlertView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIControl : MonoTouch.UIKit.UIControl , IDependencyObject
	{		
		public UIControl () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIPickerView : MonoTouch.UIKit.UIPickerView , IDependencyObject
	{		
		public UIPickerView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UISegmentedControl : MonoTouch.UIKit.UISegmentedControl , IDependencyObject
	{		
		public UISegmentedControl () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UITableView : MonoTouch.UIKit.UITableView , IDependencyObject
	{		
		public UITableView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UITableViewCell : MonoTouch.UIKit.UITableViewCell , IDependencyObject
	{		
		public UITableViewCell () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UITextField : MonoTouch.UIKit.UITextField , IDependencyObject
	{		
		public UITextField () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UITextView : MonoTouch.UIKit.UITextView , IDependencyObject
	{		
		public UITextView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIWindow : MonoTouch.UIKit.UIWindow , IDependencyObject
	{		
		public UIWindow () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIActivityIndicatorView : MonoTouch.UIKit.UIActivityIndicatorView , IDependencyObject
	{		
		public UIActivityIndicatorView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIBezierPath : MonoTouch.UIKit.UIBezierPath , IDependencyObject
	{		
		public UIBezierPath () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIButton : MonoTouch.UIKit.UIButton , IDependencyObject
	{		
		public UIButton () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UILabel : MonoTouch.UIKit.UILabel , IDependencyObject
	{		
		public UILabel () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIImageView : MonoTouch.UIKit.UIImageView , IDependencyObject
	{		
		public UIImageView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIDatePicker : MonoTouch.UIKit.UIDatePicker , IDependencyObject
	{		
		public UIDatePicker () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UINavigationBar : MonoTouch.UIKit.UINavigationBar , IDependencyObject
	{		
		public UINavigationBar () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIPageControl : MonoTouch.UIKit.UIPageControl , IDependencyObject
	{		
		public UIPageControl () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIProgressView : MonoTouch.UIKit.UIProgressView , IDependencyObject
	{		
		public UIProgressView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIScrollView : MonoTouch.UIKit.UIScrollView , IDependencyObject
	{		
		public UIScrollView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UISearchBar : MonoTouch.UIKit.UISearchBar , IDependencyObject
	{		
		public UISearchBar () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UISlider : MonoTouch.UIKit.UISlider , IDependencyObject
	{		
		public UISlider () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UISwitch : MonoTouch.UIKit.UISwitch , IDependencyObject
	{		
		public UISwitch () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UITabBar : MonoTouch.UIKit.UITabBar , IDependencyObject
	{		
		public UITabBar () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIToolbar : MonoTouch.UIKit.UIToolbar , IDependencyObject
	{		
		public UIToolbar () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	

	public partial class UIWebView : MonoTouch.UIKit.UIWebView , IDependencyObject
	{		
		public UIWebView () : base ()
		{
		
		}
	
		private static Dictionary<Type,Dictionary<string,DependencyProperty>> propertyDeclarations = new Dictionary<Type,Dictionary<string,DependencyProperty>>();
		private Dictionary<DependencyProperty,object> properties = new Dictionary<DependencyProperty,object>();

		[MonoTODO]
		public bool IsSealed {
			get { return false; }
		}

		public DependencyObjectType DependencyObjectType { 
			get { return DependencyObjectType.FromSystemType (GetType()); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			properties[dp] = null;
		}
		
		public void ClearValue(DependencyPropertyKey key)
		{
			ClearValue (key.DependencyProperty);
		}

		public void CoerceValue (DependencyProperty dp)
		{
			PropertyMetadata pm = dp.GetMetadata (this);
			if (pm.CoerceValueCallback != null)
				pm.CoerceValueCallback (this, GetValue (dp));
		}

		public sealed override bool Equals (object obj)
		{
			throw new NotImplementedException("Equals");
		}

		public sealed override int GetHashCode ()
		{
			throw new NotImplementedException("GetHashCode");
		}

		[MonoTODO]
		public LocalValueEnumerator GetLocalValueEnumerator()
		{
			return new LocalValueEnumerator(properties);
		}
		
		public object GetValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? dp.DefaultMetadata.DefaultValue : val;
		}
		
		[MonoTODO]
		public void InvalidateProperty(DependencyProperty dp)
		{
			throw new NotImplementedException("InvalidateProperty(DependencyProperty dp)");
		}
		
		public void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			PropertyMetadata pm = e.Property.GetMetadata (this);
			if (pm.PropertyChangedCallback != null)
				pm.PropertyChangedCallback (this, e);
		}
		
		public object ReadLocalValue(DependencyProperty dp)
		{
			object val = properties[dp];
			return val == null ? DependencyProperty.UnsetValue : val;
		}
		
		public void SetValue(DependencyProperty dp, object value)
		{
			if (IsSealed)
				throw new InvalidOperationException ("Cannot manipulate property values on a sealed DependencyObject");

			if (!dp.IsValidType (value))
				throw new ArgumentException ("value not of the correct type for this DependencyProperty");

			ValidateValueCallback validate = dp.ValidateValueCallback;
			if (validate != null && !validate(value))
				throw new Exception("Value does not validate");
			else
				properties[dp] = value;
		}
		
		public void SetValue(DependencyPropertyKey key, object value)
		{
			SetValue (key.DependencyProperty, value);
		}

		public bool ShouldSerializeProperty (DependencyProperty dp)
		{
			throw new NotImplementedException ();
		}

		internal static void register(Type t, DependencyProperty dp)
		{
			if (!propertyDeclarations.ContainsKey (t))
				propertyDeclarations[t] = new Dictionary<string,DependencyProperty>();
			Dictionary<string,DependencyProperty> typeDeclarations = propertyDeclarations[t];
			if (!typeDeclarations.ContainsKey(dp.Name))
				typeDeclarations[dp.Name] = dp;
			else
				throw new ArgumentException("A property named " + dp.Name + " already exists on " + t.Name);
		}
	}
	
	
}


