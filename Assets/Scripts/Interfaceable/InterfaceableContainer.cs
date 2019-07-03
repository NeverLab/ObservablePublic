using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public abstract class InterfaceableContainer<TResult> : NeverLab.Interfaceable.InterfaceableContainerBase
    where TResult : class
{
    public TResult Result
    {
        get
        {
            #if UNITY_EDITOR
                if(ObjectField == null && string.IsNullOrEmpty(ResultType))
                {
                    return _result = null;
                }
                if(string.IsNullOrEmpty(ResultType))
                {
                    _result = null;
                }
            #endif
			
        	//Using the null coalescing operator will break web player execution
            return _result != null ? _result : (_result = ObjectField as TResult);
        }
        set
        {
            _result = value;
            ObjectField = _result as Object;

            #if UNITY_EDITOR
                if(!Application.isPlaying)
                {
                    if(_result != null && ObjectField == null)
                    {
                        Debug.LogWarning("InterfaceableContainer: Cannot set Result property to non UnityEngine.Object derived types while application is not running.");
                        _result = null;
                    }
                }
                ResultType = _result != null ? ConstructResolvedName(_result.GetType()) : "";
            #endif
        }
    }

    public Object Object
    {
        //Using the null coalescing operator will break web player execution
        get { return ObjectField != null ? ObjectField : (ObjectField = _result as Object); }
    }
    
    private TResult _result;
}

namespace NeverLab.Interfaceable
{
	/// <summary>
	/// Used to enable a single CustomPropertyDrawer for all derivatives.
	/// Do not derive from this class, use the generic InterfaceableContainer&lt;TResult&gt; class instead.
	/// </summary>
	[Serializable]
    public abstract class InterfaceableContainerBase
    {
        [SerializeField]
        [HideInInspector]
        protected Object ObjectField;

        //#if UNITY_EDITOR - Excluding this from the build seems to freak the serializer out and somehow result in prefab references coming through null - nonprefabs seem to continue working though.
        //Used internally to display properly in drawer.
#pragma warning disable 414
        [SerializeField]
        [HideInInspector]
        protected string ResultType;
#pragma warning restore 414
        //#endif

        private static readonly Regex TypeArgumentsReplace = new Regex(@"`[0-9]+");
        public static string ConstructResolvedName(Type type)
        {
            var typeName = type.Name;

            if(!type.IsGenericType)
            {
                return typeName;
            }

            var argumentsString = type.GetGenericArguments().Aggregate((string)null, (s, t) => s == null ? (ConstructResolvedName(t)) : string.Format("{0}, {1}", s, (ConstructResolvedName(t))));
            return TypeArgumentsReplace.Replace(typeName, string.Format("<{0}>", argumentsString));
        }
    }
}