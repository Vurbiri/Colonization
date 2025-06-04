using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
	public class GameManager : MonoBehaviour
	{
        [SerializeField] private ScreenLabel _label;
        [Space]
        [SerializeField, Range(1f, 3f)] private float _initDelay = 1.75f;

        private Game _game;
        private CameraController _camera;

        public void Init(Game game, CameraController camera)
		{
            _label.Init();

            _game = game;
            _camera = camera;

            game.Subscribe(GameModeId.Landing, OnLanding);
            game.Subscribe(GameModeId.EndLanding, OnEndLanding);

            game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            game.Subscribe(GameModeId.StartTurn, OnStartTurn);

        }

        private void OnLanding(TurnQueue turnQueue, int hexId)
        {
            if(!turnQueue.IsSatan)
                _label.Landing(turnQueue.currentId.Value);
        }

        private void OnEndLanding(TurnQueue turnQueue, int hexId)
        {
            StartCoroutine(OnEndLanding_Cn(turnQueue.IsPlayer ? new(_initDelay) : null));

            //Local
            IEnumerator OnEndLanding_Cn(WaitRealtime pause)
            {
                yield return pause;
                _game.Landing();
            }
        }

        private void OnEndTurn(TurnQueue turnQueue, int hexId)
        {
            StartCoroutine(OnEndTurn_Cn());

            //Local
            IEnumerator OnEndTurn_Cn()
            {
                yield return _camera.ToDefaultPosition_Wait();
                _game.StartTurn();
            }
        }

        private void OnStartTurn(TurnQueue turnQueue, int hexId)
        {
            StartCoroutine(OnStartTurn_Cn(turnQueue.round, turnQueue.currentId.Value));

            //Local
            IEnumerator OnStartTurn_Cn(int turn, int id)
            {
                yield return _label.StartTurn_Wait(turn, id);
                _game.WaitRoll();
            }
        }

        private IEnumerator SetGameMode_Cn(Id<GameModeId> gameMode)
        {
            yield return null;

            
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _label);
        }
#endif
	}
}
