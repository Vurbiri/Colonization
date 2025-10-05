using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(CurrenciesLite))]
    public class CurrenciesLiteDrawer : ACurrenciesDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) => base.OnGUI(position, property, label, CurrencyId.AllCount);
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyHeight(property.isExpanded, CurrencyId.AllCount);
    }
}
