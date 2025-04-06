//Assets\Vurbiri\Editor\Reactive\UniSigner\AListenerDrawer.cs
using System;
using System.Collections.Generic;
using System.Reflection;
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
        private const bool DRAW_STATIC = true;
        
        private const string L_OBJECT = "Game Object", L_TARGET = "Target Object", L_METHOD = "Method";

        private static readonly string[] excludeStart = { "set_", "<set_" };
        private static readonly string[] excludeEnd = { "Dirty" };
        private static readonly HashSet<string> excludeMethod = new(new string[] { "Awake", "Start", "OnEnable", "Update", "FixedUpdate", "LateUpdate", "OnDisable", "OnDestroy", "OnValidate", "Reset", "Finalize", "SendTransformChangedScale", "StopAnimation" });
        #endregion

        #region Consts
        private const string P_TARGET = "_target", P_METHOD_NAME = "_methodName";
        private const string F_NONE = "None", F_PARAM_OPEN = "(", F_PARAM = ", ", F_PARAM_CLOSE = ")";
        private const string M_VOID = "void "; 
        private const string M_PUBLIC = "public ", M_PRIVATE = "private ", M_PROTECTED = "protected ", M_INTERNAL = "internal ", M_STATIC = "static ";

        private static readonly Type _gameObjectType = typeof(GameObject), _voidType = typeof(void);
        #endregion

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        private readonly Dictionary<string, GameObject> _goDict = new();
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

            GameObject parentObject;
            SerializedProperty propertyName = mainProperty.FindPropertyRelative(P_METHOD_NAME);
            SerializedProperty propertyTarget = mainProperty.FindPropertyRelative(P_TARGET);
            Object targetObject = propertyTarget.objectReferenceValue;

            string key = label.text;

            if (targetObject is Component component)
                parentObject = component.gameObject;
            else if (targetObject is GameObject gameObject)
                parentObject = gameObject;
            else
                _goDict.TryGetValue(key, out parentObject);

            BeginProperty(_position, label, mainProperty);
            {
                parentObject = DrawGameObject(parentObject, L_OBJECT);

                if (parentObject != null)
                {
                    if (!(SetArguments() && _goDict.TryGetValue(key, out GameObject tempObject) & tempObject == parentObject))
                        CreateObjectsData(parentObject);

                    targetObject = propertyTarget.objectReferenceValue = DrawTargetObject(targetObject, out int index);
                    
                    if (targetObject != null)
                        DrawMethodsNames(propertyName, _methodsValues[index], _methodsNames[index]);
                    else
                        propertyName.stringValue = string.Empty;
                }
                else
                {
                    propertyTarget.objectReferenceValue = null;
                    propertyName.stringValue = string.Empty;
                }

                _goDict[key] = parentObject;
            }
            EndProperty();
        }

        sealed public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _height * 3f;
        }

        #region Drawers
        private GameObject DrawGameObject(GameObject obj, string displayName)
        {
            return ObjectField(_position, displayName, obj, _gameObjectType, true) as GameObject;
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

            _params = F_PARAM_OPEN;
            for(int i = 0; i < _argumentsCount; i++)
            {
                _params = _params.Concat(_arguments[i].Name);
                if(i < _argumentsCount - 1)
                    _params = _params.Concat(F_PARAM);
            }
            _params = _params.Concat(F_PARAM_CLOSE);

            return false;
        }

        private void CreateObjectsData(GameObject parent)
        {
            _targetsValues = new() { null };
            List<string> targetsNames = new() { F_NONE };
            _methodsValues = new() { new string[] { string.Empty } };
            _methodsNames = new() { new string[] { F_NONE } };
            
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

            _targetsNames = targetsNames.ToArray();
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
                    names.Add(GetName(method));
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
                if (!_arguments[i].IsAssignableFrom(parameters[i].ParameterType))
                    return false;

            return true;
        }

        private bool IsName(string name)
        {
            for (int i = excludeStart.Length - 1; i >= 0; i--)
                if (name.StartsWith(excludeStart[i])) return false;

            for (int i = excludeEnd.Length - 1; i >= 0; i--)
                if(name.EndsWith(excludeEnd[i])) return false;

            if(!Listener.flags.HasFlag(BindingFlags.NonPublic) | _argumentsCount > 0) 
                return true;

            return !excludeMethod.Contains(name);
        }

        private string GetName(MethodInfo method)
        {
            string methodName = method.Name;

            methodName = M_VOID.Concat(methodName);

            if (DRAW_STATIC & method.IsStatic)
                methodName = M_STATIC.Concat(methodName);

            if (Listener.flags.HasFlag(BindingFlags.NonPublic))
                methodName = AddAccess(method, methodName);

            return methodName.Concat(_params);
        }

        private string AddAccess(MethodInfo method, string methodName)
        {
            if (method.IsPublic)
                return M_PUBLIC.Concat(methodName);
            if (method.IsPrivate)
                return M_PRIVATE.Concat(methodName);
            if (method.IsFamily)
                return M_PROTECTED.Concat(methodName);
            if (method.IsAssembly)
                return M_INTERNAL.Concat(methodName);
            if (method.IsFamilyOrAssembly)
                return M_PROTECTED.Concat(M_INTERNAL, methodName);
            if (method.IsFamilyAndAssembly)
                return M_PRIVATE.Concat(M_PROTECTED, methodName);

            return methodName;
        }
        #endregion
    }
}
