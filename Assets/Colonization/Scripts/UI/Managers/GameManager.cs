using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class GameManager
    {
        [SerializeField, Range(1f, 3f)] private float _landingDelay = 1.75f;
        [Space]
        [SerializeField] private VButton _endTurn;
        [SerializeField] private ScreenLabel _label;

        private MonoBehaviour _mono;

        public void Init(MonoBehaviour mono)
		{
            _mono = mono;

            var game = GameContainer.GameLoop;
            game.Subscribe(GameModeId.Landing, OnLanding);
            game.Subscribe(GameModeId.EndLanding, OnEndLanding);
            game.Subscribe(GameModeId.StartTurn, OnStartTurn);

            _endTurn.AddListener(game.EndTurn);
            GameContainer.Players.Person.Interactable.Subscribe(_endTurn.GetSetor<bool>(nameof(_endTurn.Unlock)));
        }

        private void OnLanding(TurnQueue turnQueue, int hexId)
        {
            if (!turnQueue.IsSatan)
                _label.Landing(turnQueue.currentId.Value);
        }

        private void OnEndLanding(TurnQueue turnQueue, int hexId)
        {
            _mono.StartCoroutine(OnEndLanding_Cn(turnQueue.IsPerson));

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
                GameContainer.GameLoop.Landing();
            }
        }

        private void OnStartTurn(TurnQueue turnQueue, int hexId)
        {
            _mono.StartCoroutine(OnStartTurn_Cn(turnQueue.turn, turnQueue.currentId.Value));

            //Local
            IEnumerator OnStartTurn_Cn(int turn, int id)
            {
                yield return _label.StartTurn_Cn(turn, id);
                GameContainer.GameLoop.WaitRoll();
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetObject(ref _endTurn, "EndTurnButton");
            EUtility.SetObject(ref _label);
        }
#endif
    }
}
