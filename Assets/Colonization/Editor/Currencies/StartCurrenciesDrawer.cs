using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(StartCurrencies))]
    public class StartCurrenciesDrawer : ACurrenciesDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) => base.OnGUI(position, property, label, true);
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyHeight(property.isExpanded, CurrencyId.Count + 1);
    }
}
