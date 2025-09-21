using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class GameManager
    {
        [SerializeField, Range(1f, 3f)] private float _landingDelay; // = 1.75f;
        [Space]
        [SerializeField] private VButton _endTurn;
        [SerializeField] private ScreenLabel _label;

        private MonoBehaviour _mono;

        public void Init(MonoBehaviour mono)
		{
            _label.Init();

            _mono = mono;

            _endTurn.AddListener(EndTurn);
            GameContainer.Players.Person.Interactable.Subscribe(_endTurn.GetSetor<bool>(nameof(_endTurn.Interactable)));

            var game = GameContainer.GameEvents;
            game.Subscribe(GameModeId.Landing, OnLanding);
            game.Subscribe(GameModeId.EndLanding, OnEndLanding);
            game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            game.Subscribe(GameModeId.StartTurn, OnStartTurn);
            game.Subscribe(GameModeId.Profit, OnProfit);
        }

        private void EndTurn()
        {
            Run(GameContainer.GameLoop.EndTurn());
        }

        private void OnLanding(TurnQueue turnQueue, int hexId)
        {
            if (!turnQueue.IsSatan)
                _label.Landing(turnQueue.currentId.Value);
        }

        private void OnEndLanding(TurnQueue turnQueue, int hexId)
        {
            Run(OnEndLanding_Cn(turnQueue.IsPerson));

            //Local
            IEnumerator OnEndLanding_Cn(bool isPlayer)
            {
                if (isPlayer)
                {
                    WaitRealtime wait = new(_landingDelay);
                    yield return wait;
                    yield return GameContainer.CameraController.ToDefaultPosition(true);
                    yield return wait.Restart(_landingDelay * 0.5f);
                }
                yield return null;
                yield return GameContainer.GameLoop.Landing();
            }
        }

        private void OnEndTurn(TurnQueue turnQueue, int hexId)
        {
            Run(OnEndTurn_Cn());

            //Local
            static IEnumerator OnEndTurn_Cn()
            {
                yield return GameContainer.CameraController.ToDefaultPosition(true);
                yield return GameContainer.GameLoop.StartTurn();
            }
        }

        private void OnStartTurn(TurnQueue turnQueue, int hexId)
        {
            Run(OnStartTurn_Cn(turnQueue.turn, turnQueue.currentId.Value));

            //Local
            IEnumerator OnStartTurn_Cn(int turn, int id)
            {
                yield return _label.StartTurn_Wait(turn, id);
                yield return GameContainer.GameLoop.WaitRoll();
            }
        }

        private void OnProfit(TurnQueue turnQueue, int dice)
        {
            Run(GameContainer.GameLoop.Play());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Coroutine Run(IEnumerator routine) => _mono.StartCoroutine(routine);

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetObject(ref _endTurn, "EndTurnButton");
            EUtility.SetObject(ref _label);
        }
#endif
    }
}
