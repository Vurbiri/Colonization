//Assets\Vurbiri\Editor\Reactive\UnitySubscriber\UnitySubscriberDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Reactive;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Reactive
{
    [CustomPropertyDrawer(typeof(UnitySubscriber))]
	public class UnitySubscriberDrawer : PropertyDrawer
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
    [CustomPropertyDrawer(typeof(UnitySubscriber<>))]
    sealed public class UnitySubscriberDrawerT : UnitySubscriberDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UnitySubscriber<,>))]
    sealed public class UnitySubscriberDrawerTT : UnitySubscriberDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UnitySubscriber<,,>))]
    sealed public class UnitySubscriberDrawerTTT : UnitySubscriberDrawer { }
    //=======================================================================================
    [CustomPropertyDrawer(typeof(UnitySubscriber<,,,>))]
    sealed public class UnitySubscriberDrawerTTTT : UnitySubscriberDrawer { }
}