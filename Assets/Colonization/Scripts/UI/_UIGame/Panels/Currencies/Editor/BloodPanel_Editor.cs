//Assets\Colonization\Scripts\UI\_UIGame\Panels\Currencies\Editor\BloodPanel_Editor.cs
#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class BloodPanel : ICanvasElement
    {
        [Space]
        [Header("Editor")]
        [SerializeField] private CurrenciesIconsScriptable _currenciesIcons;
        [SerializeField] private TextColorSettingsScriptable _settingsColor;
        [Space]
        [SerializeField] private Vector2 _padding = new(14f, 12f);

        public void UpdateVisuals()
        {
            ((RectTransform)transform).sizeDelta = _blood.Size + _padding * 2f;
            _blood.Init_Editor(_currenciesIcons.Icons[CurrencyId.Blood], _settingsColor.Colors);
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

                if (_blood == null)
                    _blood = GetComponentInChildren<Blood>();

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
    }
}
#endif
