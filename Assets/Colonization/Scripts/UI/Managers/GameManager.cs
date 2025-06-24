using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class GameManager
	{
        [SerializeField, Range(1f, 3f)] private float _landingDelay = 1.75f;
        [Space]
        [SerializeField] private ScreenLabel _label;

        private MonoBehaviour _mono;
        private GameLoop _game;
        private CameraController _camera;

        public void Init(GameLoop game, CameraController camera, MonoBehaviour mono)
		{
            _label.Init();

            _mono = mono;
            _game = game;
            _camera = camera;

            game.Subscribe(GameModeId.Landing, OnLanding);
            game.Subscribe(GameModeId.EndLanding, OnEndLanding);

            game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            game.Subscribe(GameModeId.StartTurn, OnStartTurn);
            game.Subscribe(GameModeId.WaitRoll, OnWaitRoll);
            game.Subscribe(GameModeId.Roll, OnRoll);
            game.Subscribe(GameModeId.Profit, OnProfit);
        }

        private void OnLanding(TurnQueue turnQueue, int hexId)
        {
            if(!turnQueue.IsSatan)
                _label.Landing(turnQueue.currentId.Value);
        }

        private void OnEndLanding(TurnQueue turnQueue, int hexId)
        {
            _mono.StartCoroutine(OnEndLanding_Cn(turnQueue.IsPlayer));

            //Local
            IEnumerator OnEndLanding_Cn(bool isPlayer)
            {
                if (isPlayer)
                {
                    WaitRealtime wait = new(_landingDelay);
                    yield return wait;
                    yield return _camera.ToDefaultPosition_Wait();
                    yield return wait.Restart(_landingDelay * 0.5f);
                }
                yield return null;
                yield return _game.Landing();
            }
        }

        private void OnEndTurn(TurnQueue turnQueue, int hexId)
        {
            _mono.StartCoroutine(OnEndTurn_Cn());

            //Local
            IEnumerator OnEndTurn_Cn()
            {
                yield return _camera.ToDefaultPosition_Wait();
                yield return _game.StartTurn();
            }
        }

        private void OnStartTurn(TurnQueue turnQueue, int hexId)
        {
            _mono.StartCoroutine(OnStartTurn_Cn(turnQueue.round, turnQueue.currentId.Value));

            //Local
            IEnumerator OnStartTurn_Cn(int turn, int id)
            {
                yield return _label.StartTurn_Wait(turn, id);
                yield return _game.WaitRoll();
            }
        }

        public void OnWaitRoll(TurnQueue turnQueue, int hexId)
        {
            _mono.StartCoroutine(_game.Roll(Random.Range(3, 16)));
            //_mono.StartCoroutine(_game.Roll(13));
        }

        public void OnRoll(TurnQueue turnQueue, int hexId)
        {
            _mono.StartCoroutine(_game.Profit());
        }

        private void OnProfit(TurnQueue turnQueue, int dice)
        {
            _mono.StartCoroutine(_game.Play());
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetObject(ref _label);
        }
#endif
    }
}
