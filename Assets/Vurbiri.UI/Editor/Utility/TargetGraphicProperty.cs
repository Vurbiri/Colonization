//Assets\Vurbiri.UI\Editor\Utility\TargetGraphicProperty.cs
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VurbiriEditor.UI
{
    internal readonly struct TargetGraphicProperty
	{
        #region Consts
        private const string P_GRAPHIC = "_graphic", P_FILTER = "_stateFilter";
        private const string P_FLAG_VALUE = "_value";
        private const int COUNT_ENUM = 5, FILL = ~(-1 << COUNT_ENUM);
        #endregion

        private readonly SerializedProperty _graphic;
		private readonly SerializedProperty _stateFilter;

        public readonly bool IsNull => _graphic.objectReferenceValue == null;
        public readonly bool IsNotGraphic => _graphic.objectReferenceValue is not Graphic;

        public readonly Object ReferenceValue
        {
            get => _graphic.objectReferenceValue;
            set => _graphic.objectReferenceValue = value;
        }

        public TargetGraphicProperty(SerializedProperty parent)
        {
            _graphic = parent.FindPropertyRelative(P_GRAPHIC);
            _stateFilter = parent.FindPropertyRelative(P_FILTER).FindPropertyRelative(P_FLAG_VALUE);
        }

        public readonly void SetGraphic(SerializedProperty graphicProperty)
        {
            _graphic.objectReferenceValue = graphicProperty.objectReferenceValue;
            _stateFilter.intValue = FILL;
        }
    }
}
