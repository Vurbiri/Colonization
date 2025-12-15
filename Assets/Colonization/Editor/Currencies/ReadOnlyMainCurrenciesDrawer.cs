using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(ReadOnlyLiteCurrencies), true)]
    public class ReadOnlyMainCurrenciesDrawer : ACurrenciesDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) => base.OnGUI(position, property, label, false);
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyHeight(property.isExpanded, CurrencyId.Count);
    }
}
