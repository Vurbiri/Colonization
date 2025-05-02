//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\CurrencyPopup.cs
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class CurrencyPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text _countTMP;
        [SerializeField] private PopupWidgetUI _popup;

        private Unsubscriber _unsubscriber;

        public void Init(int id, ACurrenciesReactive count, ProjectColors settings, Direction2 offsetPopup)
        {
            _popup.Init(settings, offsetPopup);

            _countTMP.color = settings.TextPanel;

            _unsubscriber = count.Subscribe(id, SetValue);
        }

        private void SetValue(int count)
        {
            _popup.Run(count);
            _countTMP.text = count.ToString();
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter ");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit ");
        }

#if UNITY_EDITOR

        public Vector2 Size => ((RectTransform)transform).sizeDelta;
        public void Init_Editor(Vector3 position, CurrencyIcon icon, ProjectColors settings)
        {
            ((RectTransform)transform).localPosition = position;

            UnityEditor.SerializedObject serializedImage = new(GetComponentInChildren<Image>());
            serializedImage.Update();
            serializedImage.FindProperty("m_Color").colorValue = icon.Color;
            serializedImage.FindProperty("m_Sprite").objectReferenceValue = icon.Icon;
            serializedImage.ApplyModifiedProperties();

            _countTMP.color = settings.TextPanel;
        }

        private void OnValidate()
        {
            if (_countTMP == null)
                _countTMP = EUtility.GetComponentInChildren<TMP_Text>(this, "TextTMP");
            if (_popup == null)
                _popup = GetComponentInChildren<PopupWidgetUI>(true);

        }
#endif
    }
}
