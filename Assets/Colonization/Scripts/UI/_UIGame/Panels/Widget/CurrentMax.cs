//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrentMax.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class CurrentMax : MonoBehaviour
    {
        private const string COUNT = "{0,2}<space=0.05em>|<space=0.05em>{1,-2}";

        [SerializeField] private TMP_Text _countTMP;

        private ReactiveCombination<int, int> _reactiveCurrentMax;

        public void Init(IReactiveValue<int> current, IReactiveValue<int> max, ProjectColors colors)
        {
            _countTMP.color = colors.TextPanel;
            _reactiveCurrentMax = new(current, max, SetCurrentMax);
        }

        private void SetCurrentMax(int current, int max)
        {
            _countTMP.text = string.Format(COUNT, current, max);
        }

        private void OnDestroy()
        {
            _reactiveCurrentMax.Dispose();
        }

#if UNITY_EDITOR
        public Vector2 Size => ((RectTransform)transform).rect.size;
        public void Init_Editor(Sprite icon, Color colorIcon, ProjectColors settings)
        {
            _countTMP.color = settings.TextPanel;
            SetCurrentMax(12, 13);

            UnityEditor.SerializedObject serializedImage = new(GetComponentInChildren<Image>());
            serializedImage.Update();
            serializedImage.FindProperty("m_Color").colorValue = colorIcon;
            serializedImage.FindProperty("m_Sprite").objectReferenceValue = icon;
            serializedImage.ApplyModifiedProperties();
        }

        protected virtual void OnValidate()
        {
            if (_countTMP == null)
                _countTMP = EUtility.GetComponentInChildren<TMP_Text>(this, "TextTMP");
        }
#endif
    }
}
