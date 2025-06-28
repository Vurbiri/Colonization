using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace VurbiriEditor.Colonization
{
	public class UISettings_Editor : MonoBehaviour, ICanvasElement
    {
        [StartEditor]
        [SerializeField, Range(1f, 3f)] private float _pixelsPerUnitForPanels = 2f;
        [SerializeField] private Colors _colors;
        [Space]
        [SerializeField, HideInInspector] private ColorSettingsScriptable _colorSettings;
        [SerializeField, HideInInspector] private AHint[] _hints;
        [SerializeField, HideInInspector] private PlayerPanels _playerPanels;
        [SerializeField, HideInInspector] private PerksWindow _perksWindow;
        [SerializeField, HideInInspector] private ExchangeWindow _exchangeWindow;
        [EndEditor] public bool endEditor;

        private void Awake()
		{
			Destroy(gameObject);
		}

        public void UpdateVisual()
        {
            var colors = _colorSettings.Colors;

            for (int i = 0; i < _hints.Length; i++)
                _hints[i].UpdateVisuals_Editor(_colors.hintBack, _colors.hintText);

            _playerPanels.UpdateVisuals_Editor(_pixelsPerUnitForPanels, colors);
            _perksWindow.UpdateVisuals_Editor(_pixelsPerUnitForPanels, colors);
            _exchangeWindow.UpdateVisuals_Editor(_pixelsPerUnitForPanels, colors);
        }

        public void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout && !Application.isPlaying)
                UpdateVisual();
        }
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        public bool IsDestroyed() => this == null;


        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetScriptable(ref _colorSettings);

                EUtility.SetObjects(ref _hints, 2);
                EUtility.SetObject(ref _playerPanels);
                EUtility.SetObject(ref _perksWindow);
                EUtility.SetObject(ref _exchangeWindow);

                if (!PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);

                _colorSettings.SetColors_Editor(_colors);
            }
        }

        #region Nested class Colors
        [System.Serializable]
        public class Colors
        {
            [Header("┌──────────── Panel ─────────────────────")]
            public Color panelBack = new(0.3764706f, 0.2117647f, 0.682353f, 0.9803922f);
            public Color panelText = new(0.2352941f, 0.9019608f, 1f);
            [Header("├──────────── Hint ─────────────────────")]
            public Color hintBack = new(0.099f, 0.8786411f, 1f);
            public Color hintText = new(0.8705882f, 0.8705882f, 1f);
        }
        #endregion
    }
    
}
