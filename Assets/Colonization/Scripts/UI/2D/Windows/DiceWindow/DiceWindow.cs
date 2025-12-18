using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class DiceWindow : ASwitchableWindow
    {
        [Space]
        [SerializeField] private FloatRnd _delayAI;
        [SerializeField] private WaitRealtime _openTime;
        [SerializeField] private WaitRealtime _closeTime;
        [Space]
        [SerializeField] private VButton _stopButton;
        [SerializeField] private TextMeshProUGUI _result;
        [Space]
        [SerializeField] private FloatRnd _delayDice;
        [SerializeField] private TextMeshProUGUI[] _labelsDice;

        private readonly Dice[] _dices = new Dice[CONST.DICES_COUNT];
        private readonly WaitRealtime _waitAI = new();
        private readonly WaitSignal _waitPerson = new();

        public override Switcher Init()
        {
            _switcher.Init(this, false);

            for (int i = 0; i < CONST.DICES_COUNT; i++)
                _dices[i] = new(_labelsDice[i]);
            _labelsDice = null;

            _stopButton.Lock = true;
            _stopButton.AddListener(_waitPerson.Send);

            var game = GameContainer.GameEvents;
            game.Subscribe(GameModeId.WaitRoll, OnWaitRoll);
            if(game.GameMode == GameModeId.Roll)
                game.Subscribe(GameModeId.Roll, OnRoll);

            return _switcher;
        }

        private void OnWaitRoll(TurnQueue turnQueue, int hexId) => StartCoroutine(Roll_Cn(turnQueue.isPerson));

        private void OnRoll(TurnQueue turnQueue, int hexId)
        {
            GameContainer.GameEvents.Unsubscribe(GameModeId.Roll, OnRoll);
            StartCoroutine(OnRoll_Cn());

            // ======== Local ===========
            IEnumerator OnRoll_Cn()
            {
                yield return _openTime.Restart();
                GameContainer.GameLoop.Profit();
            }
        }

        private IEnumerator Roll_Cn(bool isPerson)
        {
            var switcher = _switcher;

            switcher.onOpen.Invoke(switcher);

            _result.text = string.Empty;

            for (int i = 0; i < CONST.DICES_COUNT; i++)
                _dices[i].Run(_delayDice.Roll);

            _stopButton.InteractableAndUnlock(isPerson, isPerson);

            yield return switcher.Show();
            yield return isPerson ? _waitPerson.Restart() : _waitAI.Restart(_delayAI);

            _stopButton.interactable = false;

            int result = 0;
            for (int i = 0; i < CONST.DICES_COUNT; i++)
                result += _dices[i].Stop();
            _result.text = result.ToStr();

            GameContainer.GameLoop.Roll(result);

            yield return _openTime.Restart();
            yield return switcher.Hide();
            yield return _closeTime.Restart();

            yield return GameContainer.CameraController.FromDefaultPosition(true);

            switcher.onClose.Invoke(switcher);
            GameContainer.GameLoop.Profit();
        }

#if UNITY_EDITOR

        [StartEditor]
        [SerializeField, Range(0.5f, 1f)] private float _panelsBrightness = 0.8f;
        [SerializeField, HideInInspector] private UnityEngine.UI.Image _mainImage, _resultImage;

        protected override void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            base.OnValidate();

            this.SetChildren(ref _stopButton);
            this.SetChildren(ref _result, "Result_TMP");

            this.SetComponent(ref _mainImage);
            this.SetChildren(ref _resultImage, "Result");

            _labelsDice ??= new TextMeshProUGUI[CONST.DICES_COUNT];
            if (_labelsDice.Length != CONST.DICES_COUNT)
                System.Array.Resize(ref _labelsDice, CONST.DICES_COUNT);
            if (_delayDice.Min == 0f && _delayDice.Max == 0f)
                _delayDice = new(0.175f, 0.25f);

            _resultImage.color = _mainImage.color.Brightness(_panelsBrightness);
        }

        public override void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
        {
            Color color = colors.PanelBack.SetAlpha(1f);

            _mainImage.color = color;
            _mainImage.pixelsPerUnitMultiplier = pixelsPerUnit;

            _resultImage.color = color.Brightness(_panelsBrightness);
        }
#endif
    }
}
