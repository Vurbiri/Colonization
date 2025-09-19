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
        [SerializeField] private Vector2 _screenPadding = new(16f, 16f);
        [SerializeField, Range(1f, 3f)] private float _windowsPixelsPerUnit = 1.6f;
        [SerializeField, Button("UpdateVisual"), Range(1f, 3f)] private float _panelsPixelsPerUnit = 2.1f;
        [Space]
        [SerializeField] private ProjectColors _colors;
        [Space]
        [SerializeField, HideInInspector] private ColorSettingsScriptable _colorSettings;
        [SerializeField, HideInInspector] private AHint[] _hints;
        [SerializeField, HideInInspector] private PlayerPanels _playerPanels;
        [SerializeField, HideInInspector] private PerksWindow _perksWindow;
        [SerializeField, HideInInspector] private ExchangeWindow _exchangeWindow;
        [SerializeField, HideInInspector] private OpponentPanels _opponentPanels;
        [SerializeField, HideInInspector] private GiftWindow _giftWindow;
        [EndEditor] public bool endEditor;

        private void Awake()
		{
			Destroy(gameObject);
		}

        public void UpdateVisual()
        {
            for (int i = 0; i < _hints.Length; i++)
                _hints[i].UpdateVisuals_Editor(_colors.HintBack, _colors.HintText);

            _playerPanels.UpdateVisuals_Editor(_panelsPixelsPerUnit, _colors, _screenPadding);
            _perksWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _colors);
            _exchangeWindow.UpdateVisuals_Editor(_windowsPixelsPerUnit, _colors);

            var transform = _opponentPanels.UpdateVisuals_Editor(_panelsPixelsPerUnit, _colors, _screenPadding);
            _giftWindow.UpdateVisuals_Editor(_windowsPixelsPerUnit, _panelsPixelsPerUnit, _colors, transform);
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
                EUtility.SetObject(ref _opponentPanels);
                EUtility.SetObject(ref _giftWindow);

                if (!PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);

                _colors = _colorSettings.Colors;
            }
        }
    }
}
