using System.Collections;
using TMPro;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class DiceWindow : MonoBehaviour
	{
        private const int DICES_COUNT = 2, MIN = 2;

        [SerializeField] private FloatRnd _delayAI;
        [SerializeField] private WaitRealtime _openTime;
        [Space]
        [SerializeField] private CanvasGroupSwitcher _canvasSwitcher;
        [Space]
        [SerializeField] private Dice[] _dices;
        [SerializeField] private VButton _stopButton;
        [SerializeField] private TextMeshProUGUI _result;

        private readonly WaitRealtime _waitAI = new();
        private readonly WaitSignal _waitPerson = new();
        public void Init()
		{
            string[] numbers = new string[CONST.DICE];
            for (int i = 0; i < CONST.DICE; i++)
                numbers[i] = (i + 1).ToString();

            for(int i = 0; i < DICES_COUNT; i++)
                _dices[i].Init(numbers);

            _stopButton.Interactable = false;
            _stopButton.AddListener(_waitPerson.Send);

            GameContainer.GameLoop.Subscribe(GameModeId.WaitRoll, StartRoll);

            _canvasSwitcher.Disable();
        }

        public void StartRoll(TurnQueue turnQueue, int hexId) => StartCoroutine(WaitStop(turnQueue.IsPerson));

        private IEnumerator WaitStop(bool isPerson)
        {
            GameContainer.InputController.WindowMode(true);

            _result.text = string.Empty;

            yield return _canvasSwitcher.Show();

            for (int i = 0; i < DICES_COUNT; i++)
                _dices[i].Run();

            yield return null;

            _stopButton.Interactable = isPerson;

            IEnumerator wait = isPerson ? _waitPerson.Restart() : _waitAI.Restart(_delayAI);
            yield return wait;

            _stopButton.interactable = false;

            int result = MIN;
            for (int i = 0; i < DICES_COUNT; i++)
                result += _dices[i].Stop();
            _result.text = result.ToString();

            yield return GameContainer.GameLoop.Roll(result);

            yield return _openTime.Restart();
            yield return _canvasSwitcher.Hide();

            StartCoroutine(GameContainer.GameLoop.Profit());
            GameContainer.InputController.WindowMode(false);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _canvasSwitcher.OnValidate(this, 6);

            this.SetChildrens(ref _dices, DICES_COUNT);
            this.SetChildren(ref _stopButton);
            this.SetChildren(ref _result, "Result_TMP");
        }
#endif
	}
}
