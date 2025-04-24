//Assets\Colonization\Scripts\UI\_UIGame\Panels\Editor\PlayerPanels_Editor.cs
#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : ICanvasElement
    {
        [Header("┌──────────── Editor ─────────────────────")]
        [SerializeField, ReadOnly] private CurrenciesIconsScriptable _currenciesIcons;
        [SerializeField, ReadOnly] private ColorSettingsScriptable _colorSettings;
        [Space]
        [SerializeField, Range(1f, 3f)] private float _pixelsPerUnit = 1.5f;
        [Header("Panels In")]
        [SerializeField] private Vector2 _paddingIn = new(14f, 12f);
        [SerializeField, Range(1f, 10f)] private float _spaceIn = 4f;
        [Header("Between")]
        [SerializeField] private Vector2 _paddingOut = new(15f, 15f);
        [SerializeField, Range(5f, 20f)] private float _spaceOut = 5f;
        [Header("└────────────────────────────────────")]
        #pragma warning disable 414
        [SerializeField, ReadOnly] private string _endEditor = "****************************************************************";
        #pragma warning restore 414

        public void UpdateVisuals()
        {
            var icons = _currenciesIcons.Icons;
            var colors = _colorSettings.Colors;

            RectTransform thisRectTransform = (RectTransform)transform;
            RectTransform rectCurrencies = _currencies.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, _spaceIn, icons, colors);
            RectTransform rectBlood = _blood.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, icons, colors);

            Vector3 position = -thisRectTransform.rect.size * 0.5f + _paddingOut;
            rectCurrencies.localPosition = position;

            position.x += rectCurrencies.rect.width + _spaceOut;
            rectBlood.localPosition = position;
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
                EUtility.SetScriptable(ref _colorSettings);

                EUtility.SetObject(ref _currencies);
                EUtility.SetObject(ref _blood);

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
    }
}

#endif
