using UnityEditor;
using UnityEngine;
using Vurbiri.Reactive;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Reactive
{
    [CustomPropertyDrawer(typeof(UniSubscription))]
	public class UniSignerDrawer : PropertyDrawer
	{
        private readonly string F_NAME = "_listeners";

        sealed public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty propertyArray = mainProperty.FindPropertyRelative(F_NAME);

            BeginProperty(position, label, mainProperty);
			{
				PropertyField(position, propertyArray, label);
			}
			EndProperty();
		}

        sealed public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            float rate = 3.2f;
            SerializedProperty propertyArray = mainProperty.FindPropertyRelative(F_NAME);

            if(!propertyArray.isExpanded)
                return height;

            if (propertyArray.arraySize > 0)
				return height * propertyArray.arraySize * rate + height * 2.4f;

            return height * rate;
        }
	}
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UniSubscription<>))]
    sealed public class UniSignerDrawerT : UniSignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UniSubscription<,>))]
    sealed public class UniSignerDrawerTT : UniSignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UniSubscription<,,>))]
    sealed public class UniSignerDrawerTTT : UniSignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UniSubscription<,,,>))]
    sealed public class UniSignerDrawerTTTT : UniSignerDrawer { }
}
