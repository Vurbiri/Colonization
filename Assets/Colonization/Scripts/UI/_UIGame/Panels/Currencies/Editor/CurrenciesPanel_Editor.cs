//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\Editor\CurrenciesPanel_Editor.cs
#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    //[ExecuteAlways]
    public partial class CurrenciesPanel : ICanvasElement
    {
        [Space]
        [Header("Editor")]
        [SerializeField] private CurrenciesIconsScriptable _currenciesIcons;
        [SerializeField] private TextColorSettingsScriptable _settingsColor;
        [Space]
        [SerializeField] private Vector2 _padding = new(14f, 12f);
        [SerializeField] private float _space = 4f;

        public void UpdateVisuals()
        {
            RectTransform thisRectTransform = (RectTransform)transform;
            var icons = _currenciesIcons.Icons;
            var colors = _settingsColor.Colors;

            Vector2 cSize = _currencies[0].Size, aSize = _amount.Size;

            Vector2 size = new()
            {
                y = Mathf.Max(cSize.y, aSize.y) + _padding.y * 2f,
                x = cSize.x * 5f + aSize.x + _space * 5f + _padding.x * 2f
            };
            thisRectTransform.sizeDelta = size;

            Vector2 pivot = thisRectTransform.pivot;
            float posX = cSize.x * 0.5f + _padding.x - size.x * pivot.x;
            float posY = size.y * (0.5f - pivot.y);
            Vector3 pos = new(posX, posY, 0f);

            float offset = cSize.x + _space;
            for (int i = 0; i < CurrencyId.CountMain; i++)
            {
                _currencies[i].Init_Editor(pos, icons[i], colors);
                pos.x += offset;
            }

            pos.x -= (cSize.x - aSize.x) * 0.5f;
            _amount.Init_Editor(pos, colors);
        }

        public void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout)
                UpdateVisuals();
        }
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        public bool IsDestroyed() => this == null;

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetScriptable(ref _currenciesIcons);
                EUtility.SetScriptable(ref _settingsColor);

                if (_currencies == null || _currencies.Length != CurrencyId.CountMain)
                    _currencies = GetComponentsInChildren<Currency>();
                if (_amount == null)
                    _amount = GetComponentInChildren<Amount>();

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
    }
}
#endif
