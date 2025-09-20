using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class GameManager
    {
        [SerializeField, Range(1f, 3f)] private float _landingDelay = 1.75f;
        [Space]
        [SerializeField] private VButton _endTurn;
        [SerializeField] private ScreenLabel _label;

        private MonoBehaviour _mono;
        private GameLoop _game;
        private CameraController _camera;

        public void Init(MonoBehaviour mono)
		{
            _label.Init();

            _mono = mono;
            _game = GameContainer.GameLoop;
            _camera = GameContainer.CameraController;

            _endTurn.AddListener(EndTurn);
            GameContainer.Players.Person.Interactable.Subscribe(_endTurn.GetSetor<bool>(nameof(_endTurn.Interactable)));

            _game.Subscribe(GameModeId.Landing, OnLanding);
            _game.Subscribe(GameModeId.EndLanding, OnEndLanding);

            _game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            _game.Subscribe(GameModeId.StartTurn, OnStartTurn);
            _game.Subscribe(GameModeId.WaitRoll, OnWaitRoll);
            _game.Subscribe(GameModeId.Roll, OnRoll);
            _game.Subscribe(GameModeId.Profit, OnProfit);
        }

        private void EndTurn()
        {
            Run(_game.EndTurn());
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
                    yield return _camera.ToDefaultPosition(true);
                    yield return wait.Restart(_landingDelay * 0.5f);
                }
                yield return null;
                yield return _game.Landing();
            }
        }

        private void OnEndTurn(TurnQueue turnQueue, int hexId)
        {
            Run(OnEndTurn_Cn());

            //Local
            IEnumerator OnEndTurn_Cn()
            {
                yield return _camera.ToDefaultPosition(true);
                yield return _game.StartTurn();
            }
        }

        private void OnStartTurn(TurnQueue turnQueue, int hexId)
        {
            Run(OnStartTurn_Cn(turnQueue.turn, turnQueue.currentId.Value));

            //Local
            IEnumerator OnStartTurn_Cn(int turn, int id)
            {
                yield return _label.StartTurn_Wait(turn, id);
                yield return _game.WaitRoll();
            }
        }

        public void OnWaitRoll(TurnQueue turnQueue, int hexId)
        {
            Run(_game.Roll(Random.Range(3, 16)));
            //Run(_game.Roll(13));
        }

        public void OnRoll(TurnQueue turnQueue, int hexId)
        {
            Run(_game.Profit());
        }

        private void OnProfit(TurnQueue turnQueue, int dice)
        {
            Run(_game.Play());
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
