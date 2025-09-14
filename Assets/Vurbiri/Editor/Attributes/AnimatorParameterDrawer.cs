using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Vurbiri;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(AnimatorParameterAttribute))]
    public class AnimatorParameterDrawer : PropertyDrawer
    {
        private static readonly Dictionary<int, Parameters> s_cache = new();

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            if (TryGetParameters(mainProperty, out Parameters param))
            {
                label = BeginProperty(position, label, mainProperty);
                {
                    mainProperty.intValue = IntPopup(position, label.text, mainProperty.intValue, param.names, param.values);
                }
                EndProperty();
            }
            else
            {
                PropertyField(position, mainProperty, label, true);
            }

            #region Local TryGetParameters(..)
            //=======================================================================================
            static bool TryGetParameters(SerializedProperty mainProperty, out Parameters parameters)
            {
                parameters = null;
                if (mainProperty.propertyType == SerializedPropertyType.Integer)
                {
                    var target = mainProperty.serializedObject.targetObject as Component;
                    if (target != null)
                    {
                        var animator = target.GetComponentInChildren<Animator>();
                        if (animator != null)
                            parameters = Parameters.Get(animator);
                    }
                }
                return parameters != null;
            }
            #endregion
        }

        #region Nested Parameters
        //****************************************************
        private class Parameters
        {
            private static readonly Parameters s_empty = new();

            public readonly string[] names;
            public readonly int[] values;

            public static Parameters Get(Animator animator)
            {
                var controller = animator.runtimeAnimatorController as AnimatorController;
                if (controller == null)
                {
                    var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                    if (overrideController != null)
                        controller = overrideController.runtimeAnimatorController as AnimatorController;
                }

                Parameters param;
                if (controller != null)
                {
                    if (!s_cache.TryGetValue(controller.GetInstanceID(), out param))
                        param = new(controller);
                }
                else
                {
                    param = s_empty;
                }
                return param;
            }

            private Parameters()
            {
                names = new string[0];
                values = new int[0];
            }

            private Parameters(AnimatorController controller)
            {
                var parameters = controller.parameters;
                int count = parameters.Length;

                names = new string[count];
                values = new int[count];

                for (int i = 0; i < count; i++)
                {
                    names[i] = parameters[i].name;
                    values[i] = parameters[i].nameHash;
                }

                s_cache[controller.GetInstanceID()] = this;
            }
        }
        #endregion
    }
}