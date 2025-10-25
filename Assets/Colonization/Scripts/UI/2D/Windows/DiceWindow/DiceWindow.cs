using System.Collections;
using TMPro;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class DiceWindow : MonoBehaviour
	{
        [SerializeField] private FloatRnd _delayAI;
        [SerializeField] private WaitRealtime _openTime;
        [Space]
        [SerializeField] private CanvasGroupSwitcher _canvasSwitcher;
        [Space]
        [SerializeField] private VButton _stopButton;
        [SerializeField] private TextMeshProUGUI _result;
        [Space]
        [SerializeField] private DiceSettings _diceSettings;

        private readonly Dice[] _dices = new Dice[CONST.DICES_COUNT];
        private readonly WaitRealtime _waitAI = new();
        private readonly WaitSignal _waitPerson = new();

        public void Init()
		{
            for (int i = 0; i < CONST.DICES_COUNT; i++)
                _dices[i] = new(_diceSettings.labels[i], _diceSettings.time);
            _diceSettings = null;

            _stopButton.Lock = true;
            _stopButton.AddListener(_waitPerson.Send);

            GameContainer.GameLoop.Subscribe(GameModeId.WaitRoll, Roll);

            _canvasSwitcher.Disable();
#if TEST_AI
            UnityEngine.Debug.LogWarning("[DiceWindow] TEST_AI");
#endif
        }
#if TEST_AI
        public void Roll(TurnQueue turnQueue, int hexId) => StartCoroutine(Roll_Cn(false));
#else
        public void Roll(TurnQueue turnQueue, int hexId) => StartCoroutine(Roll_Cn(turnQueue.IsPerson));
#endif

        private IEnumerator Roll_Cn(bool isPerson)
        {
            GameContainer.InputController.WindowMode(true);

            _result.text = string.Empty;

            for (int i = 0; i < CONST.DICES_COUNT; i++)
                _dices[i].Run();

            _stopButton.InteractableAndUnlock(isPerson, isPerson);

            yield return _canvasSwitcher.Show();
            yield return isPerson ? _waitPerson.Restart() : _waitAI.Restart(_delayAI);

            _stopButton.interactable = false;

            int result = 0;
            for (int i = 0; i < CONST.DICES_COUNT; i++)
                result += _dices[i].Stop();
            _result.text = CONST.NUMBERS_STR[result];

            GameContainer.GameLoop.Roll(result);

            yield return _openTime.Restart();
            yield return _canvasSwitcher.Hide();

            GameContainer.InputController.WindowMode(false);
            GameContainer.GameLoop.Profit();
        }

        // **** Nested ****
        [System.Serializable] private class DiceSettings
        {
            public FloatRnd time;
            public TextMeshProUGUI[] labels;

#if UNITY_EDITOR
            public void OnValidate()
            {
                labels ??= new TextMeshProUGUI[CONST.DICES_COUNT];
                if (labels.Length != CONST.DICES_COUNT)
                    System.Array.Resize(ref labels, CONST.DICES_COUNT);
            }
#endif
        }

#if UNITY_EDITOR

        [StartEditor]
        [SerializeField, Range(0.5f, 1f)] private float _panelsBrightness = 0.8f;
        [SerializeField, HideInInspector] private UnityEngine.UI.Image _mainImage, _resultImage, _stopImage, _buttonCenterImage;

        private void OnValidate()
        {
            _canvasSwitcher.OnValidate(this, 6);

            this.SetChildren(ref _stopButton);
            this.SetChildren(ref _result, "Result_TMP");

            this.SetComponent(ref _mainImage);
            this.SetChildren(ref _resultImage, "Result");
            this.SetChildren(ref _stopImage, "StopButton");
            this.SetChildren(ref _buttonCenterImage, "Center");

            _diceSettings.OnValidate();

            _resultImage.color = _stopImage.color = _mainImage.color.Brightness(_panelsBrightness);
        }

        public void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
        {
            Color color = colors.PanelBack.SetAlpha(1f);

            _mainImage.color = color;
            _mainImage.pixelsPerUnitMultiplier = pixelsPerUnit;

            _buttonCenterImage.color = color;

            _resultImage.color = _stopImage.color = color.Brightness(_panelsBrightness);
        }
#endif
    }
}
