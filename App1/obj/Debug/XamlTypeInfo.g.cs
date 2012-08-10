﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml;
using App1.App1_XamlTypeInfo;

namespace App1
{
    public partial class App : IXamlMetadataProvider
    {
        private XamlTypeInfoProvider _provider;

        public IXamlType GetXamlType(Type type)
        {
            if(_provider == null)
            {
                _provider = new XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByType(type);
        }

        public IXamlType GetXamlType(String typeName)
        {
            if(_provider == null)
            {
                _provider = new XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByName(typeName);
        }

        public XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return new XmlnsDefinition[0];
        }
    }
}

namespace App1.App1_XamlTypeInfo
{
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal partial class XamlTypeInfoProvider
    {
        public IXamlType GetXamlTypeByType(Type type)
        {
            string standardName;
            IXamlType xamlType = null;
            if(_xamlTypeToStandardName.TryGetValue(type, out standardName))
            {
                xamlType = GetXamlTypeByName(standardName);
            }
            else
            {
                xamlType = GetXamlTypeByName(type.FullName);
            }
            return xamlType;
        }

        public IXamlType GetXamlTypeByName(string typeName)
        {
            if (String.IsNullOrEmpty(typeName))
            {
                return null;
            }
            IXamlType xamlType;
            if (_xamlTypes.TryGetValue(typeName, out xamlType))
            {
                return xamlType;
            }
            xamlType = CreateXamlType(typeName);
            if (xamlType != null)
            {
                _xamlTypes.Add(typeName, xamlType);
            }
            return xamlType;
        }

        public IXamlMember GetMemberByLongName(string longMemberName)
        {
            if (String.IsNullOrEmpty(longMemberName))
            {
                return null;
            }
            IXamlMember xamlMember;
            if (_xamlMembers.TryGetValue(longMemberName, out xamlMember))
            {
                return xamlMember;
            }
            xamlMember = CreateXamlMember(longMemberName);
            if (xamlMember != null)
            {
                _xamlMembers.Add(longMemberName, xamlMember);
            }
            return xamlMember;
        }

        Dictionary<string, IXamlType> _xamlTypes = new Dictionary<string, IXamlType>();
        Dictionary<string, IXamlMember> _xamlMembers = new Dictionary<string, IXamlMember>();
        Dictionary<Type, string> _xamlTypeToStandardName = new Dictionary<Type, string>();

        private void AddToMapOfTypeToStandardName(Type t, String str)
        {
            if(!_xamlTypeToStandardName.ContainsKey(t))
            {
                _xamlTypeToStandardName.Add(t, str);
            }
        }

        private object Activate_0_MainPage() { return new App1.MainPage(); }


        private IXamlType CreateXamlType(string typeName)
        {
            XamlSystemBaseType xamlType = null;
            XamlUserType userType;

            switch (typeName)
            {
            case "Windows.UI.Xaml.Controls.Page":
                xamlType = new XamlSystemBaseType(typeName, typeof(Windows.UI.Xaml.Controls.Page));
                break;

            case "Windows.UI.Xaml.Controls.UserControl":
                xamlType = new XamlSystemBaseType(typeName, typeof(Windows.UI.Xaml.Controls.UserControl));
                break;

            case "App1.MainPage":
                userType = new XamlUserType(this, typeName, typeof(App1.MainPage), GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_0_MainPage;
                xamlType = userType;
                break;

            }
            return xamlType;
        }



        private IXamlMember CreateXamlMember(string longMemberName)
        {
            XamlMember xamlMember = null;
            // No Local Properties
            return xamlMember;
        }

    }

    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlSystemBaseType : IXamlType
    {
        string _fullName;
        Type _underlyingType;

        public XamlSystemBaseType(string fullName, Type underlyingType)
        {
            _fullName = fullName;
            _underlyingType = underlyingType;
        }

        public string FullName { get { return _fullName; } }

        public Type UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        virtual public IXamlType BaseType { get { throw new NotImplementedException(); } }
        virtual public IXamlMember ContentProperty { get { throw new NotImplementedException(); } }
        virtual public IXamlMember GetMember(string name) { throw new NotImplementedException(); }
        virtual public bool IsArray { get { throw new NotImplementedException(); } }
        virtual public bool IsCollection { get { throw new NotImplementedException(); } }
        virtual public bool IsConstructible { get { throw new NotImplementedException(); } }
        virtual public bool IsDictionary { get { throw new NotImplementedException(); } }
        virtual public bool IsMarkupExtension { get { throw new NotImplementedException(); } }
        virtual public bool IsBindable { get { throw new NotImplementedException(); } }
        virtual public IXamlType ItemType { get { throw new NotImplementedException(); } }
        virtual public IXamlType KeyType { get { throw new NotImplementedException(); } }
        virtual public object ActivateInstance() { throw new NotImplementedException(); }
        virtual public void AddToMap(object instance, object key, object item)  { throw new NotImplementedException(); }
        virtual public void AddToVector(object instance, object item)  { throw new NotImplementedException(); }
        virtual public void RunInitializer()   { throw new NotImplementedException(); }
        virtual public object CreateFromString(String input)   { throw new NotImplementedException(); }
    }
    
    internal delegate object Activator();
    internal delegate void AddToCollection(object instance, object item);
    internal delegate void AddToDictionary(object instance, object key, object item);

    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlUserType : XamlSystemBaseType
    {
        XamlTypeInfoProvider _provider;
        IXamlType _baseType;
        bool _isArray;
        bool _isMarkupExtension;
        bool _isBindable;

        string _contentPropertyName;
        string _itemTypeName;
        string _keyTypeName;
        Dictionary<string, string> _memberNames;
        Dictionary<string, object> _enumValues;

        public XamlUserType(XamlTypeInfoProvider provider, string fullName, Type fullType, IXamlType baseType)
            :base(fullName, fullType)
        {
            _provider = provider;
            _baseType = baseType;
        }

        // --- Interface methods ----

        override public IXamlType BaseType { get { return _baseType; } }
        override public bool IsArray { get { return _isArray; } }
        override public bool IsCollection { get { return (CollectionAdd != null); } }
        override public bool IsConstructible { get { return (Activator != null); } }
        override public bool IsDictionary { get { return (DictionaryAdd != null); } }
        override public bool IsMarkupExtension { get { return _isMarkupExtension; } }
        override public bool IsBindable { get { return _isBindable; } }

        override public IXamlMember ContentProperty
        {
            get { return _provider.GetMemberByLongName(_contentPropertyName); }
        }

        override public IXamlType ItemType
        {
            get { return _provider.GetXamlTypeByName(_itemTypeName); }
        }

        override public IXamlType KeyType
        {
            get { return _provider.GetXamlTypeByName(_keyTypeName); }
        }

        override public IXamlMember GetMember(string name)
        {
            if (_memberNames == null)
            {
                return null;
            }
            string longName;
            if (_memberNames.TryGetValue(name, out longName))
            {
                return _provider.GetMemberByLongName(longName);
            }
            return null;
        }

        override public object ActivateInstance()
        {
            return Activator(); 
        }

        override public void AddToMap(object instance, object key, object item) 
        {
            DictionaryAdd(instance, key, item);
        }

        override public void AddToVector(object instance, object item)
        {
            CollectionAdd(instance, item);
        }

        override public void RunInitializer() 
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(UnderlyingType.TypeHandle);
        }

        override public Object CreateFromString(String input)
        {
            if (_enumValues != null)
            {
                Int32 value = 0;

                string[] valueParts = input.Split(',');

                foreach (string valuePart in valueParts) 
                {
                    object partValue;
                    Int32 enumFieldValue = 0;
                    try
                    {
                        if (_enumValues.TryGetValue(valuePart.Trim(), out partValue))
                        {
                            enumFieldValue = Convert.ToInt32(partValue);
                        }
                        else
                        {
                            try
                            {
                                enumFieldValue = Convert.ToInt32(valuePart.Trim());
                            }
                            catch( FormatException )
                            {
                                foreach( string key in _enumValues.Keys )
                                {
                                    if( String.Compare(valuePart.Trim(), key, System.StringComparison.OrdinalIgnoreCase) == 0 )
                                    {
                                        if( _enumValues.TryGetValue(key.Trim(), out partValue) )
                                        {
                                            enumFieldValue = Convert.ToInt32(partValue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        value |= enumFieldValue; 
                    }
                    catch( FormatException )
                    {
                        throw new ArgumentException(input, FullName);
                    }
                }

                return value; 
            }
            throw new ArgumentException(input, FullName);
        }

        // --- End of Interface methods

        public Activator Activator { get; set; }
        public AddToCollection CollectionAdd { get; set; }
        public AddToDictionary DictionaryAdd { get; set; }

        public void SetContentPropertyName(string contentPropertyName)
        {
            _contentPropertyName = contentPropertyName;
        }

        public void SetIsArray()
        {
            _isArray = true; 
        }

        public void SetIsMarkupExtension()
        {
            _isMarkupExtension = true;
        }

        public void SetIsBindable()
        {
            _isBindable = true;
        }

        public void SetItemTypeName(string itemTypeName)
        {
            _itemTypeName = itemTypeName;
        }

        public void SetKeyTypeName(string keyTypeName)
        {
            _keyTypeName = keyTypeName;
        }

        public void AddMemberName(string shortName)
        {
            if(_memberNames == null)
            {
                _memberNames =  new Dictionary<string,string>();
            }
            _memberNames.Add(shortName, FullName + "." + shortName);
        }

        public void AddEnumValue(string name, object value)
        {
            if (_enumValues == null)
            {
                _enumValues = new Dictionary<string, object>();
            }
            _enumValues.Add(name, value);
        }
    }

    internal delegate object Getter(object instance);
    internal delegate void Setter(object instance, object value);

    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlMember : IXamlMember
    {
        XamlTypeInfoProvider _provider;
        string _name;
        bool _isAttachable;
        bool _isDependencyProperty;
        bool _isReadOnly;

        string _typeName;
        string _targetTypeName;

        public XamlMember(XamlTypeInfoProvider provider, string name, string typeName)
        {
            _name = name;
            _typeName = typeName;
            _provider = provider;
        }

        public string Name { get { return _name; } }

        public IXamlType Type
        {
            get { return _provider.GetXamlTypeByName(_typeName); }
        }

        public void SetTargetTypeName(String targetTypeName)
        {
            _targetTypeName = targetTypeName;
        }
        public IXamlType TargetType
        {
            get { return _provider.GetXamlTypeByName(_targetTypeName); }
        }

        public void SetIsAttachable() { _isAttachable = true; }
        public bool IsAttachable { get { return _isAttachable; } }

        public void SetIsDependencyProperty() { _isDependencyProperty = true; }
        public bool IsDependencyProperty { get { return _isDependencyProperty; } }

        public void SetIsReadOnly() { _isReadOnly = true; }
        public bool IsReadOnly { get { return _isReadOnly; } }

        public Getter Getter { get; set; }
        public object GetValue(object instance)
        {
            if (Getter != null)
                return Getter(instance);
            else
                throw new InvalidOperationException("GetValue");
        }

        public Setter Setter { get; set; }
        public void SetValue(object instance, object value)
        {
            if (Setter != null)
                Setter(instance, value);
            else
                throw new InvalidOperationException("SetValue");
        }
    }
}


