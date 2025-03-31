//Assets\Vurbiri\Editor\Reactive\UnitySubscriber\AListenerDrawer.cs
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
        #region Consts
        private const string P_TARGET = "_target", P_METHOD_NAME = "_methodName";
        private const string F_NONE = "None", F_PARAM_OPEN = "(", F_PARAM = ", ", F_PARAM_CLOSE = ")";
        private const string M_VOID = "void ", M_PUBLIC = "public ", M_PRIVATE = "private ", M_PROTECTED = "protected ", M_STATIC = "static ";
        private const string L_OBJECT = "Game Object", L_TARGET = "Target object", L_METHOD = "Method";

        private static readonly Type _gameObjectType = typeof(GameObject), _voidType = typeof(void);

        private static readonly string[] excludeStart = { "set_", "<set_" };
        private static readonly string[] excludeEnd = { "Dirty" };
        private static readonly HashSet<string> excludeMethod = new(new string[] { "Awake", "Start", "OnEnable", "Update", "FixedUpdate", "LateUpdate", "OnDisable", "OnDestroy", "OnValidate", "Reset", "Finalize", "SendTransformChangedScale", "StopAnimation" });
        #endregion

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        private readonly Dictionary<string, GameObject> _objects = new();
        private Type[] _arguments;
        private int _argumentsCount;
        private string _params;
        private Rect _position;

        sealed public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            _position = position;

            GameObject parentObject;
            SerializedProperty propertyName = mainProperty.FindPropertyRelative(P_METHOD_NAME);
            SerializedProperty propertyTarget = mainProperty.FindPropertyRelative(P_TARGET);
            Object targetObject = propertyTarget.objectReferenceValue;

            if (targetObject is Component component)
                parentObject = component.gameObject;
            else if (targetObject is GameObject gameObject)
                parentObject = gameObject;
            else
                _objects.TryGetValue(label.text, out parentObject);

            BeginProperty(_position, label, mainProperty);
            {
                parentObject = _objects[label.text] = DrawGameObject(parentObject, L_OBJECT);

                if (parentObject != null)
                {
                    SetArguments();
                    var (objects, objectNames, methodsValues, methodsNames) = CreateObjectsData(parentObject);
                    targetObject = propertyTarget.objectReferenceValue = DrawTargetObject(targetObject, objects, objectNames, out int index);
                    
                    if (targetObject != null)
                        DrawMethodsNames(propertyName, methodsValues[index], methodsNames[index]);
                    else
                        propertyName.stringValue = string.Empty;
                }
                else
                {
                    propertyTarget.objectReferenceValue = null;
                    propertyName.stringValue = string.Empty;
                }
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
        private Object DrawTargetObject(Object obj, List<Object> objects, List<string> obgNames, out int index)
        {
            _position.y += _height;
            index = Popup(_position, L_TARGET, objects.IndexOf(obj), obgNames.ToArray());

            if (index < 0 | index >= objects.Count) return null;
            return objects[index];
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
        private void SetArguments()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray) type = type.GetElementType();
            
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
        }

        private (List<Object>, List<string>, List<string[]>, List<string[]>) CreateObjectsData(GameObject parent)
        {
            List<Object> objects = new() { null };
            List<string> objectsNames = new() { F_NONE };
            List<string[]> methodsValues = new() { new string[] { string.Empty } };
            List<string[]> methodsNames = new() { new string[] { F_NONE } };

            {
                if (TryGetObjectData(parent, out string name, out string[] methodValues, out string[] methodNames))
                {
                    objects.Add(parent); objectsNames.Add(name); methodsValues.Add(methodValues); methodsNames.Add(methodNames);
                }
            }

            foreach (var obj in parent.GetComponents<Component>())
            {
                if (TryGetObjectData(obj, out string name, out string[] methodValues, out string[] methodNames))
                {
                    objects.Add(obj); objectsNames.Add(name); methodsValues.Add(methodValues); methodsNames.Add(methodNames);
                }
            }

            return (objects, objectsNames, methodsValues, methodsNames);
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

            objName = type.Name;
            methodValues = values.ToArray();
            methodNames = names.ToArray();

            return names.Count > 1;
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

            if(_argumentsCount > 0) return true;

            return !excludeMethod.Contains(name);
        }

        private string GetName(MethodInfo method)
        {
            string methodName = method.Name;

            methodName = M_VOID.Concat(methodName);

            if (method.IsStatic)
                methodName = M_STATIC.Concat(methodName);

            if (method.IsPublic)
                methodName = M_PUBLIC.Concat(methodName);
            else if (method.IsPrivate)
                methodName = M_PRIVATE.Concat(methodName);
            else
                methodName = M_PROTECTED.Concat(methodName);

            return methodName.Concat(_params);
        }
        #endregion
    }
}