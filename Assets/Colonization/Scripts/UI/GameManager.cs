using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class GameManager : MonoBehaviour
	{
        [SerializeField, Range(1f, 3f)] private float _landingDelay = 1.75f;
        [Space]
        [SerializeField] private ScreenLabel _label;
        [Space]
        [SerializeField] private Controlled _controlled;

        private GameLoop _game;
        private CameraController _camera;

        public void Init(GameLoop game, CameraController camera, Human player)
		{
            _label.Init();
            _controlled.Init(player);

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
            StartCoroutine(OnEndLanding_Cn(turnQueue.IsPlayer));

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
            _controlled.Disable();

            StartCoroutine(OnEndTurn_Cn());

            //Local
            IEnumerator OnEndTurn_Cn()
            {
                yield return _camera.ToDefaultPosition_Wait();
                yield return _game.StartTurn();
            }
        }

        private void OnStartTurn(TurnQueue turnQueue, int hexId)
        {
            StartCoroutine(OnStartTurn_Cn(turnQueue.round, turnQueue.currentId.Value));

            //Local
            IEnumerator OnStartTurn_Cn(int turn, int id)
            {
                yield return _label.StartTurn_Wait(turn, id);
                yield return _game.WaitRoll();
            }
        }

        public void OnWaitRoll(TurnQueue turnQueue, int hexId)
        {
            //StartCoroutine(_game.Roll(Random.Range(3, 16)));
            StartCoroutine(_game.Roll(13));
        }

        public void OnRoll(TurnQueue turnQueue, int hexId)
        {
            StartCoroutine(_game.Profit());
        }

        private void OnProfit(TurnQueue turnQueue, int dice)
        {
            if (turnQueue.IsPlayer)
                _controlled.Enable();

            StartCoroutine(_game.Play());
        }

        #region Nested class Controlled
        [System.Serializable]
        private class Controlled
        {
            [SerializeField] private CanvasHint _hint;

            [SerializeField] private PerksWindow _perksWindow;
            [SerializeField] private HintButton _perksButton;

            public void Init(Human player)
            {
                _perksWindow.Init(player, _hint);
                _perksButton.Init(_hint, _perksWindow.Switch/*, false*/);
            }
            public void Enable()
            {
                _perksButton.Interactable = true;
            }
            public void Disable()
            {
                _perksButton.Interactable = false;
                _perksWindow.Close();
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                EUtility.SetObject(ref _hint);

                EUtility.SetObject(ref _perksWindow);
                EUtility.SetObject(ref _perksButton, "PerksButton");
            }
#endif
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _label);

            _controlled.OnValidate();
        }
#endif
    }
}
