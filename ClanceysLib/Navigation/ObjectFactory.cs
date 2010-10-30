using System;
using System.Reflection;
using System.Linq;
namespace ClanceysLib
{
	public sealed class ObjectFactory
	{
		private ObjectFactory ()
		{
		}
		public static object Create (string typeName, object[] parameters)
		{
// resolve the type
			Type targetType = ResolveType (typeName);
			if (targetType == null)
				throw new ArgumentException ("Can't load type " + typeName);
			
// get the default constructor and instantiate
			Type[] types = parameters == null ? new Type[0] : parameters.Select(x=> x.GetType()).ToArray();
			ConstructorInfo info = targetType.GetConstructor (types);			
			if (info == null)
				throw new ArgumentException ("Can't instantiate type " + typeName);
			object targetObject = info.Invoke (parameters);
			if (targetObject == null)
				throw new ArgumentException ("Can't instantiate type " + typeName);
			
			return targetObject;
		}

		private static Type ResolveType (string typeString)
		{
			int commaIndex = typeString.IndexOf (",");
			string className = typeString.Substring (0, commaIndex).Trim ();
			string assemblyName = typeString.Substring (commaIndex + 1).Trim ();
			
// Get the assembly containing the handler
			Assembly assembly = null;
			try
			{
				assembly = Assembly.Load (assemblyName);
			}
			catch
			{
				try
				{
					assembly = Assembly.LoadWithPartialName (assemblyName);
				}
				catch
				{
					throw new ArgumentException ("Can't load assembly " + assemblyName);
				}
			}
			
// Get the handler
			return assembly.GetType (className, false, true);
		}
	}
}
