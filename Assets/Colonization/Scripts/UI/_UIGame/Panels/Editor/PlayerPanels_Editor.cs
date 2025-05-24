#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : ICanvasElement
    {
        [Header("┌──────────── Editor ─────────────────────")]
        [SerializeField, ReadOnly] private ColorSettingsScriptable _colorSettings;
        [Space]
        [SerializeField, Range(1f, 3f)] private float _pixelsPerUnit = 1.5f;
        [Header("Panels In")]
        [SerializeField] private Vector2 _paddingIn = new(14f, 12f);
        [SerializeField, Range(1f, 10f)] private float _spaceIn = 4f;
        [Header("Between")]
        [SerializeField] private Vector2 _paddingOut = new(15f, 15f);
        [SerializeField, Range(1f, 20f)] private float _spaceOut = 7f;
        [SerializeField, Range(0.1f, 1f)] private float _advRatioSpaceOut = 0.4f;
        [Header("└────────────────────────────────────")]
        #pragma warning disable 414
        [SerializeField, ReadOnly] private string _endEditor = "****************************************************************";
        #pragma warning restore 414

        public void UpdateVisuals()
        {
            var colors = _colorSettings.Colors;
            var advPadding = _spaceOut * _advRatioSpaceOut;

            RectTransform thisRectTransform = (RectTransform)transform;

            RectTransform rectWarriors   = _warriors.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);
            RectTransform rectColonies   = _colonies.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);
            RectTransform rectPorts      = _ports.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);
            RectTransform rectShrines    = _shrines.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);
            RectTransform rectRoads      = _roads.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);
            RectTransform rectCurrencies = _currencies.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, _spaceIn, colors);
            RectTransform rectBlood      = _blood.UpdateVisuals_Editor(_pixelsPerUnit, _paddingIn, colors);

            //=======

            Vector3 position = -thisRectTransform.rect.size * 0.5f + _paddingOut;

            rectWarriors.localPosition   = position;
            rectColonies.localPosition   = position = NextPosition(position, rectWarriors, advPadding);
            rectPorts.localPosition      = position = NextPosition(position, rectColonies);
            rectShrines.localPosition    = position = NextPosition(position, rectPorts);
            rectRoads.localPosition      = position = NextPosition(position, rectShrines, advPadding * .7f);
            rectCurrencies.localPosition = position = NextPosition(position, rectRoads, advPadding);
            rectBlood.localPosition      = position = NextPosition(position, rectCurrencies);

            // Local function
            Vector3 NextPosition(Vector3 current, RectTransform prevPanel, float advOffset = 0f)
            {
                current.x += prevPanel.rect.width + _spaceOut + advOffset;
                return current;
            }
        }

        public void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout && !Application.isPlaying)
                UpdateVisuals();
        }
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        public bool IsDestroyed() => this == null;

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetScriptable(ref _colorSettings);

                EUtility.SetObject(ref _warriors);
                EUtility.SetObject(ref _colonies);
                EUtility.SetObject(ref _ports);
                EUtility.SetObject(ref _shrines);
                EUtility.SetObject(ref _roads);
                EUtility.SetObject(ref _currencies);
                EUtility.SetObject(ref _blood);

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);

                for (int i = 0; i < EdificeId.Count; i++)
                {
                    if (i > 0 && _sprites[i] == null)
                        _sprites[i] = EUtility.FindAnyAsset<Sprite>($"SP_Icon{EdificeId.GetName(i)}");
                }
            }
        }
    }
}

#endif
