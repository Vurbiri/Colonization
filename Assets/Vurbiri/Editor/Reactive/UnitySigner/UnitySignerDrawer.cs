//Assets\Vurbiri\Editor\Reactive\UnitySigner\UnitySignerDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Reactive;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Reactive
{
    [CustomPropertyDrawer(typeof(UnitySigner))]
	public class UnitySignerDrawer : PropertyDrawer
	{
		#region Consts
		private const string P_NAME = "_listeners";
		#endregion
		
		sealed public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty propertyArray = mainProperty.FindPropertyRelative(P_NAME);
            
			propertyArray.isExpanded = true;

            BeginProperty(position, label, mainProperty);
			{
				PropertyField(position, propertyArray, label);
			}
			EndProperty();
		}

        sealed public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            float rate = 3.1f;
            SerializedProperty propertyArray = mainProperty.FindPropertyRelative(P_NAME);

			if(propertyArray.arraySize > 0)
				return height * propertyArray.arraySize * rate + height * 2.2f;

            return height * 3.2f;
        }
	}
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UnitySigner<>))]
    sealed public class UnitySignerDrawerT : UnitySignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UnitySigner<,>))]
    sealed public class UnitySignerDrawerTT : UnitySignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UnitySigner<,,>))]
    sealed public class UnitySignerDrawerTTT : UnitySignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UnitySigner<,,,>))]
    sealed public class UnitySignerDrawerTTTT : UnitySignerDrawer { }
}
