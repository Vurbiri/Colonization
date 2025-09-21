using System.Collections;
using TMPro;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class DiceWindow : MonoBehaviour
	{
        private const int DICES_COUNT = 2;

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
            for(int i = 0; i < DICES_COUNT; i++)
                _dices[i].Init();

            _stopButton.interactable = false;
            _stopButton.AddListener(_waitPerson.Send);

            GameContainer.GameLoop.Subscribe(GameModeId.WaitRoll, StartRoll);

            _canvasSwitcher.Disable();
        }

        public void StartRoll(TurnQueue turnQueue, int hexId) => StartCoroutine(WaitStop(turnQueue.IsPerson));

        private IEnumerator WaitStop(bool isPerson)
        {
            GameContainer.InputController.WindowMode(true);

            _result.text = string.Empty;

            for (int i = 0; i < DICES_COUNT; i++)
                _dices[i].Run();

            _stopButton.CombineInteractable(isPerson, isPerson);

            yield return _canvasSwitcher.Show();
            yield return isPerson ? _waitPerson.Restart() : _waitAI.Restart(_delayAI);

            _stopButton.interactable = false;

            int result = 0;
            for (int i = 0; i < DICES_COUNT; i++)
                result += _dices[i].Stop();
            _result.text = CONST.NUMBERS_STR[result];

            yield return GameContainer.GameLoop.Roll_Cn(result);

            yield return _openTime.Restart();
            yield return _canvasSwitcher.Hide();

            StartCoroutine(GameContainer.GameLoop.Profit_Cn());
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
