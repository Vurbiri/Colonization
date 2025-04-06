//Assets\Vurbiri\Editor\Reactive\UniSigner\UniSignerDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Reactive;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Reactive
{
    [CustomPropertyDrawer(typeof(UniSigner))]
	public class UniSignerDrawer : PropertyDrawer
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
    [CustomPropertyDrawer(typeof(UniSigner<>))]
    sealed public class UniSignerDrawerT : UniSignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UniSigner<,>))]
    sealed public class UniSignerDrawerTT : UniSignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UniSigner<,,>))]
    sealed public class UniSignerDrawerTTT : UniSignerDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UniSigner<,,,>))]
    sealed public class UniSignerDrawerTTTT : UniSignerDrawer { }
}
