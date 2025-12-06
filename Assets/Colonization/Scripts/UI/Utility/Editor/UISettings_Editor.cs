using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri;
using Vurbiri.Colonization;
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
        [SerializeField, HideInInspector] private ColorSettingsScriptable _colorSettings;
        [SerializeField, HideInInspector] private PlayerVisualSetScriptable _playerVisual;
        [Space]
        [SerializeField] private PlayerColors _playerColors;
        [SerializeField] private ProjectColors _projectColors;
        [Space]
        [SerializeField, HideInInspector] private AHint[] _hints;
        [SerializeField, HideInInspector] private PlayerPanels _playerPanels;
        [SerializeField, HideInInspector] private PerksWindow _perksWindow;
        [SerializeField, HideInInspector] private ExchangeWindow _exchangeWindow;
        [SerializeField, HideInInspector] private OpponentPanels _opponentPanels;
        [SerializeField, HideInInspector] private GiftWindow _giftWindow;
        [SerializeField, HideInInspector] private DiceWindow _diceWindow;
        [SerializeField, HideInInspector] private SettingsWindow _settingsWindow;
        [SerializeField, HideInInspector] private HelpWindow _helpWindow;
        [SerializeField, HideInInspector] private GameOverWindow _gameOverWindow;
        [SerializeField, HideInInspector] private ChaosPanel _chaosPanel;
        [SerializeField, HideInInspector] private SatanPanel _satanPanel;
        [EndEditor] public bool endEditor;

        private void Awake()
		{
			Destroy(gameObject);
		}

        public void UpdateVisual()
        {
            for (int i = 0; i < _hints.Length; i++)
                _hints[i].UpdateVisuals_Ed(_projectColors.HintBack, _projectColors.HintText);

            _playerPanels.UpdateVisuals_Ed(_panelsPixelsPerUnit, _projectColors, _screenPadding);
            _perksWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _projectColors);
            _exchangeWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _projectColors);

            var transform = _opponentPanels.UpdateVisuals_Ed(_panelsPixelsPerUnit, _projectColors, _screenPadding);
            _giftWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _panelsPixelsPerUnit, _projectColors, transform);

            _diceWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _projectColors);
            _settingsWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _projectColors);
            _helpWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _projectColors);
            _gameOverWindow.UpdateVisuals_Ed(_windowsPixelsPerUnit, _projectColors);

            _chaosPanel.UpdateVisuals_Ed(_screenPadding);
            _satanPanel.UpdateVisuals_Ed(_playerColors, _screenPadding);
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
                EUtility.SetScriptable(ref _playerVisual);

                EUtility.SetObjects(ref _hints, 2);
                EUtility.SetObject(ref _playerPanels);
                EUtility.SetObject(ref _perksWindow);
                EUtility.SetObject(ref _exchangeWindow);
                EUtility.SetObject(ref _opponentPanels);
                EUtility.SetObject(ref _giftWindow);
                EUtility.SetObject(ref _diceWindow);
                EUtility.SetObject(ref _settingsWindow);
                EUtility.SetObject(ref _helpWindow);
                EUtility.SetObject(ref _gameOverWindow);
                EUtility.SetObject(ref _chaosPanel);
                EUtility.SetObject(ref _satanPanel);

                if (!PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);

                _projectColors = _colorSettings.Colors;
                _playerColors = _playerVisual.Colors_Ed;
            }
        }
    }
}
