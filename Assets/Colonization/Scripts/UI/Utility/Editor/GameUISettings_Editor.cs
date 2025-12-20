using UnityEngine;
using UnityEngine.UI;
using Vurbiri;
using Vurbiri.Colonization;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace VurbiriEditor.Colonization.UI
{
	public class GameUISettings_Editor : MonoBehaviour, ICanvasElement
    {
        [StartEditor]
        [SerializeField] private Vector2 _screenPadding = new(13f, 13f);
        [SerializeField, MinMax(1f, 2f)] private RefFloat _windowsPixelsPerUnit;
        [SerializeField, Button(nameof(UpdateVisual)), Range(1f, 3f)] private float _panelsPixelsPerUnit = 2.4f;
        [Space]
        [SerializeField, HideInInspector] private ColorSettingsScriptable _colorSettings;
        [SerializeField, HideInInspector] private PlayerVisualSetScriptable _playerVisual;
        [Space]
        [SerializeField] private PlayerColors _playerColors;
        [SerializeField] private ProjectColors _projectColors;
        [SerializeField] private SceneColorsEd _gameColors;
        [Space]
        [SerializeField, HideInInspector] private Hint[] _hints;
        [SerializeField, HideInInspector] private PlayerPanels _playerPanels;
        [SerializeField, HideInInspector] private PerksWindow _perksWindow;
        [SerializeField, HideInInspector] private ExchangeWindow _exchangeWindow;
        [SerializeField, HideInInspector] private OpponentPanels _opponentPanels;
        [SerializeField, HideInInspector] private GiftWindow _giftWindow;
        [SerializeField, HideInInspector] private DiceWindow _diceWindow;
        [SerializeField, HideInInspector] private GameSettingsWindow _settingsWindow;
        [SerializeField, HideInInspector] private HelpWindow _helpWindow;
        [SerializeField, HideInInspector] private GameOverWindow _gameOverWindow;
        [SerializeField, HideInInspector] private ChaosPanel _chaosPanel;
        [SerializeField, HideInInspector] private SatanPanel _satanPanel;
        [SerializeField, HideInInspector] private InitUI _initUI;
        [EndEditor] public bool endEditor;

        private void Awake()
		{
			Destroy(gameObject);
		}

        public void UpdateVisual()
        {
            float windowsPixelsPerUnit = _windowsPixelsPerUnit;

            for (int i = 0; i < _hints.Length; i++)
                _hints[i].UpdateVisuals_Ed(_gameColors.hintBack, _gameColors.hintText);

            _playerPanels.UpdateVisuals_Ed(_panelsPixelsPerUnit, _projectColors, _gameColors, _screenPadding);
            _perksWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _gameColors);
            _exchangeWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _projectColors, _gameColors);

            var transform = _opponentPanels.UpdateVisuals_Ed(_panelsPixelsPerUnit, _gameColors, _screenPadding);
            _giftWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _panelsPixelsPerUnit, _gameColors, transform);

            _diceWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _gameColors);
            _settingsWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _gameColors);
            _helpWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _gameColors);
            _gameOverWindow.UpdateVisuals_Ed(windowsPixelsPerUnit, _gameColors);

            _chaosPanel.UpdateVisuals_Ed(_screenPadding);
            _satanPanel.UpdateVisuals_Ed(_playerColors, _screenPadding);

            _initUI.SetColors_Ed(_gameColors);
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
                EUtility.SetObject(ref _initUI);

                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);

                _projectColors = _colorSettings.Colors;
                _gameColors = _colorSettings.game;
                _playerColors = _playerVisual.Colors_Ed;

                _windowsPixelsPerUnit = _colorSettings.windowsPixelsPerUnit;
            }
        }
    }
}
