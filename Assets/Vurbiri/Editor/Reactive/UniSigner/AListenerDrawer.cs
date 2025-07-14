using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Reactive;
using static UnityEditor.EditorGUI;
using Object = UnityEngine.Object;

namespace VurbiriEditor.Reactive
{
    [CustomPropertyDrawer(typeof(AListener<>), true)]
	sealed public class AListenerDrawer : PropertyDrawer
    {
        #region Settings
        private readonly bool DRAW_STATIC = true;
        
        private readonly string L_OBJECT = "Game Object", L_TARGET = "Target Object", L_METHOD = "Method";

        private const float OFFSET = 2f;
        private readonly Color _color = new(.25f, .25f, .25f);

        private readonly string[] _excludeStart = { /*"set_", */"<set_" };
        private readonly string[] _excludeEnd = { "Dirty" };
        private readonly HashSet<string> _excludeVoidMethod = new(new string[] { "Awake", "Start", "OnEnable", "Update", "FixedUpdate", "LateUpdate", "OnDisable", "OnDestroy", "OnValidate", "Reset", "OnBeforeSerialize", "OnAfterDeserialize", "Finalize", "SendTransformChangedScale", "StopAnimation" });
        #endregion

        #region Consts
        private const string F_NONE = "None", F_PARAM_OPEN = "(", F_PARAM_CLOSE = ")", F_PARAM_SEPARATOR = ", ";
        private const string M_VOID = "void ";
        private const string M_PUBLIC = "public ", M_PRIVATE = "private ", M_PROTECTED = "protected ", M_INTERNAL = "internal ", M_STATIC = "static ";
        private readonly string P_TARGET = "_target", P_METHOD_NAME = "_methodName";
        private readonly string F_GENERIC_OPEN = "<", F_GENERIC_CLOSE = ">";
        private readonly char F_GENERIC_SEPARATOR = '`';
       
        private readonly int s_preNameMaxLength = M_PROTECTED.Length + M_INTERNAL.Length + M_STATIC.Length + M_VOID.Length;
        private readonly Type s_gameObjectType = typeof(GameObject), _voidType = typeof(void);
        #endregion

        #region Cache
        string _key;
        private readonly Dictionary<string, GameObject> _gameObjectCache = new();
        private readonly Dictionary<string, List<Object>> _targetsValuesCache = new();
        private readonly Dictionary<string, string[]> _targetsNamesCache = new();
        private readonly Dictionary<string, List<string[]>> _methodsValuesCache = new();
        private readonly Dictionary<string, List<string[]>> _methodsNamesCache = new();
        #endregion

        #region ConvertNames
        private readonly Dictionary<string, string> _converter = new() {{"Boolean", "bool"}, { "Byte", "byte" }, { "SByte", "sbyte" }, { "Int16", "short" }, { "UInt16", "ushort" }, { "Int32", "int" }, { "UInt32", "uint" }, { "Int64", "long" }, { "UInt64", "ulong" }, { "Single", "float" }, { "Double", "double" }, { "Decimal", "decimal" }, { "Char", "char" }, { "String", "string" }, { "Object", "object" }};
        #endregion

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        
        private Type _type;
        private Type[] _arguments;
        private int _argumentsCount;
        private string _params;
        private Rect _position;
               
        private List<Object> _targetsValues;
        private string[] _targetsNames;
        List<string[]> _methodsValues;
        List<string[]> _methodsNames;

        sealed public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            _position = position;
            _key = label.text;

            SerializedProperty propertyTarget = mainProperty.FindPropertyRelative(P_TARGET);
            SerializedProperty propertyName = mainProperty.FindPropertyRelative(P_METHOD_NAME);
            
            GameObject parentObject;
            Object targetObject = propertyTarget.objectReferenceValue;

            if (targetObject is Component component)
                parentObject = component.gameObject;
            else if (targetObject is GameObject gameObject)
                parentObject = gameObject;
            else
                _gameObjectCache.TryGetValue(_key, out parentObject);
                        
            BeginProperty(_position, label, mainProperty);
            {
                bool isArray = fieldInfo.FieldType.IsArray;
                if (isArray || DrawFoldout(mainProperty, label))
                {
                    bool isParent, isTarget = false;
                    DrawBackground(isArray);

                    parentObject = DrawGameObject(parentObject, L_OBJECT);

                    if (isParent = parentObject != null)
                    {
                        bool update = !(SetArguments() && _gameObjectCache.TryGetValue(_key, out GameObject tempObject) & tempObject == parentObject);
                        CreateObjectsData(parentObject, update);

                        targetObject = propertyTarget.objectReferenceValue = DrawTargetObject(targetObject, out int index);
                        if (isTarget = targetObject != null)
                            DrawMethodsNames(propertyName, _methodsValues[index], _methodsNames[index]);
                    }

                    if(!isParent)
                    {
                        propertyTarget.objectReferenceValue = null;
                        DrawLabelNone(propertyTarget.displayName);
                    }
                    if (!isTarget)
                    {
                        propertyName.stringValue = string.Empty;
                        DrawLabelNone(propertyName.displayName);
                    }

                    _gameObjectCache[_key] = parentObject;
                }
                else
                {
                    DrawLabel(targetObject, propertyName.stringValue);
                }
            }
            EndProperty();
        }

        sealed public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType.IsArray)
                return _height * 3f;
            if (property.isExpanded)
                return _height * 4f;
            return _height;
        }

        #region Drawers
        private void DrawBackground(bool isArray)
        {
            if (isArray)
                return;

            Rect position = _position;
            position.height = _height * 3f;
            position.x -= OFFSET; position.width += OFFSET * 2f;
            position.y -= OFFSET; position.height += OFFSET * 2f;
            DrawRect(position, _color);
        }
        private bool DrawFoldout(SerializedProperty property, GUIContent label)
        {
            property.isExpanded = Foldout(_position, property.isExpanded, label);
            _position.y += _height;
            return property.isExpanded;
        }
        private GameObject DrawGameObject(GameObject obj, string displayName)
        {
            return ObjectField(_position, displayName, obj, s_gameObjectType, true) as GameObject;
        }
        private Object DrawTargetObject(Object obj, out int index)
        {
            _position.y += _height;
            index = Popup(_position, L_TARGET, _targetsValues.IndexOf(obj), _targetsNames);

            if (index < 0 | index >= _targetsValues.Count) return null;
            return _targetsValues[index];
        }
        private void DrawMethodsNames(SerializedProperty property, string[] values, string[] names)
        {
            int index = Math.Max(Array.IndexOf(values, property.stringValue), 0);

            _position.y += _height;
            index = Popup(_position, L_METHOD, index, names);

            if (index < 0 | index >= names.Length)
                property.stringValue = string.Empty;
            else
                property.stringValue = values[index];
        }
        private void DrawLabelNone(string name)
        {
            _position.y += _height;
            LabelField(_position, name, "---");
        }
        private void DrawLabel(Object obj, string method)
        {
            string msg = "[None]";
            if (obj != null)
            {
                msg = obj.name.Concat(".", obj.GetType().Name);
                if (!string.IsNullOrEmpty(method))
                {
                    if (string.IsNullOrEmpty(_params)) SetArguments();
                    msg = msg.Concat(".", method, _params);
                }
                else
                {
                    msg = msg.Concat(".[None]");
                }
            }
            _position.y -= _height;
            _position.x += EditorGUIUtility.labelWidth; _position.width -= EditorGUIUtility.labelWidth;
            DropShadowLabel(_position, msg, EditorStyles.boldLabel);
        }
        #endregion

        #region Create
        private bool SetArguments()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray) type = type.GetElementType();

            if (_type == type & _arguments != null & _params != null) return true;

            _type = type;

            _arguments = type.GetGenericArguments();
            _argumentsCount = _arguments.Length;

            _params = GetParams(_arguments, _argumentsCount);

            return false;
        }

        private string GetParams(Type[] arguments, int argumentsCount, string open = F_PARAM_OPEN, string close = F_PARAM_CLOSE)
        {
            Type[] argumentsGeneric; Type argument;
            StringBuilder sb = new(2 + 8 * argumentsCount);
            
            sb.Append(open);
            for (int i = 0; i < argumentsCount; i++)
            {
                argument = arguments[i];
                argumentsGeneric = argument.GetGenericArguments();
                if (argumentsGeneric.Length > 0)
                {
                    sb.Append(argument.Name.Split(F_GENERIC_SEPARATOR)[0]);
                    sb.Append(GetParams(argumentsGeneric, argumentsGeneric.Length, F_GENERIC_OPEN, F_GENERIC_CLOSE));
                }
                else
                {
                    sb.Append(ConvertName(argument.Name));
                }
                if (i < argumentsCount - 1) sb.Append(F_PARAM_SEPARATOR);
            }
            sb.Append(close);

            return sb.ToString();
        }

        private void CreateObjectsData(GameObject parent, bool update)
        {
            if (!update && _targetsValuesCache.TryGetValue(_key, out _targetsValues) & _targetsNamesCache.TryGetValue(_key, out _targetsNames)
                & _methodsValuesCache.TryGetValue(_key, out _methodsValues) & _methodsNamesCache.TryGetValue(_key, out _methodsNames))
                return;

            List<string> targetsNames = new() { F_NONE };
            _targetsValuesCache[_key] = _targetsValues = new() { null };
            _methodsValuesCache[_key] = _methodsValues = new() { new string[] { string.Empty } };
            _methodsNamesCache[_key] = _methodsNames = new() { new string[] { F_NONE } };
            
            {
                if (TryGetObjectData(parent, out string name, out string[] methodValues, out string[] methodNames))
                {
                    _targetsValues.Add(parent); targetsNames.Add(name); _methodsValues.Add(methodValues); _methodsNames.Add(methodNames);
                }
            }

            foreach (var obj in parent.GetComponents<Component>())
            {
                if (TryGetObjectData(obj, out string name, out string[] methodValues, out string[] methodNames))
                {
                    _targetsValues.Add(obj); targetsNames.Add(name); _methodsValues.Add(methodValues); _methodsNames.Add(methodNames);
                }
            }

            _targetsNamesCache[_key] = _targetsNames = targetsNames.ToArray();
        }

        private bool TryGetObjectData(Object obj, out string objName, out string[] methodValues, out string[] methodNames)
        {
            Type type = obj.GetType();
            MethodInfo[] methods = type.GetMethods(Listener.flags);

            List<string> values = new() { string.Empty };
            List<string> names = new() { F_NONE };
            
            string methodName;
            foreach (MethodInfo method in methods)
            {
                methodName = method.Name;
                if (method.ReturnType == _voidType && IsName(methodName) && IsParams(method))
                {
                    values.Add(methodName);
                    names.Add(GetMethodName(method));
                }
            }

            if (names.Count > 1)
            {
                objName = type.Name;
                methodValues = values.ToArray();
                methodNames = names.ToArray();
                return true;
            }

            objName = null;
            methodValues = null;
            methodNames = null;
            return false;
        }
        #endregion

        #region Utility
        private bool IsParams(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length != _argumentsCount)
                return false;

            for (int i = 0; i < _argumentsCount; i++)
                if (_arguments[i] != parameters[i].ParameterType)
                    return false;

            return true;
        }

        private bool IsName(string name)
        {
            for (int i = _excludeStart.Length - 1; i >= 0; i--)
                if (name.StartsWith(_excludeStart[i])) return false;

            for (int i = _excludeEnd.Length - 1; i >= 0; i--)
                if(name.EndsWith(_excludeEnd[i])) return false;

            if(_argumentsCount > 0) 
                return true;

            return !_excludeVoidMethod.Contains(name);
        }

        private string ConvertName(string name)
        {
            if (_converter.TryGetValue(name, out string outName))
                return outName;

            return name;
        }

        private string GetMethodName(MethodInfo method)
        {
            string methodName = method.Name;
            StringBuilder sb = new(s_preNameMaxLength + methodName.Length + _params.Length);

            if (Listener.flags.HasFlag(BindingFlags.NonPublic))
                sb.Append(GetAccess(method));

            if (DRAW_STATIC & method.IsStatic)
                sb.Append(M_STATIC);

            sb.Append(M_VOID);
            sb.Append(methodName);
            sb.Append(_params);

            return sb.ToString();
        }

        private string GetAccess(MethodInfo method)
        {
            if (method.IsPublic)            return M_PUBLIC;
            if (method.IsPrivate)           return M_PRIVATE;
            if (method.IsFamily)            return M_PROTECTED;
            if (method.IsAssembly)          return M_INTERNAL;
            if (method.IsFamilyOrAssembly)  return string.Concat(M_PROTECTED, M_INTERNAL);
            if (method.IsFamilyAndAssembly) return string.Concat(M_PRIVATE, M_PROTECTED);

            return string.Empty;
        }
        #endregion
    }
}
