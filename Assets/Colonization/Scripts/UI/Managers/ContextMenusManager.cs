using UnityEngine;
using static Vurbiri.Colonization.GameContainer;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class ContextMenusManager
    {
        [SerializeField] private RectTransform _canvasTransform;
        [SerializeField] private RectTransform _menusTransform;
        [Space]
        [SerializeField] private CrossroadInitMenu _initMenu;
        [SerializeField] private CrossroadMainMenu _crossroadMenu;
        [SerializeField] private CrossroadWarriorsMenu _recruitingMenu;
        [SerializeField] private CrossroadRoadsMenu _roadsMenu;
        [Space]
        [SerializeField] private WarriorsMenu _warriorsMenu;
        [Space]
        [SerializeField] private ButtonCancel _buttonCancel;

        private bool _enable, _lookAtEnabled;
        private IMenu _currentOpenMenu;

        public void Init()
        {
            _buttonCancel  .Init().Add(OnActiveMenu);
            _crossroadMenu .Init(_roadsMenu, _recruitingMenu).Add(OnActiveMenu);
            _recruitingMenu.Init(_crossroadMenu).Add(OnActiveMenu);
            _roadsMenu     .Init(_crossroadMenu).Add(OnActiveMenu);
            _warriorsMenu  .Init().Add(OnActiveMenu);

            CameraTransform.Subscribe(LookAtCamera);

            var game = GameContainer.GameLoop;
            game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            game.Subscribe(GameModeId.Play, OnGamePlay);

            EventBus.EventUnselect.Add(OnClose);
            EventBus.EventActorRightSelect.Add(OnSelectWarrior);

            if (game.GameMode == GameModeId.Landing)
            {
                game.Subscribe(GameModeId.Landing, OnInit);
                game.Subscribe(GameModeId.EndLanding, OnEndInit);

                _initMenu.Init().Add(OnActiveMenu);
                EventBus.EventCrossroadSelect.Add(OnInitCrossroad);
            }
            else
            {
                EventBus.EventCrossroadSelect.Add(OnSelectCrossroad);
                Object.Destroy(_initMenu.gameObject);  _initMenu = null;
            }
        }

        private void OnSelectCrossroad(Crossroad crossroad)
        {
            if (_enable & !SpellBook.IsCasting & crossroad.Interactable & _currentOpenMenu == null)
            {
                CameraTransform.TransformToLocalPosition(_menusTransform, _canvasTransform, crossroad.Position);
                _crossroadMenu.Open(crossroad);
                _buttonCancel.Setup(crossroad);
            }
        }
        private void OnInitCrossroad(Crossroad crossroad)
        {
            if (_enable & !SpellBook.IsCasting & crossroad.Interactable & _currentOpenMenu == null)
            {
                CameraTransform.TransformToLocalPosition(_menusTransform, _canvasTransform, crossroad.Position);
                _initMenu.Open(crossroad);
                _buttonCancel.Setup(crossroad);
            }
        }

        private void OnSelectWarrior(Actor actor)
        {
            if (_enable & !SpellBook.IsCasting & actor.CanUseSkills & _currentOpenMenu == null)
            {
                CameraTransform.TransformToLocalPosition(_menusTransform, _canvasTransform, actor.Position);
                _warriorsMenu.Open(actor);
                _buttonCancel.Setup(actor);
            }
        }

        private void OnClose(bool isEquals)
        {
            if (!isEquals & _currentOpenMenu != null)
                _currentOpenMenu.CloseInstant();
        }
        private void OnEndTurn(TurnQueue turnQueue, int dice)
        {
            _enable = false;
            OnClose(false);
        }
        private void OnGamePlay(TurnQueue turnQueue, int dice)
        {
            _enable = turnQueue.IsPerson;
        }
        private void OnInit(TurnQueue turnQueue, int dice)
        {
            _enable = turnQueue.IsPerson;
        }

        private void OnEndInit(TurnQueue turnQueue, int dice)
        {
            _enable = false;

            GameContainer.GameEvents.Unsubscribe(GameModeId.Landing, OnInit);
            GameContainer.GameEvents.Unsubscribe(GameModeId.EndLanding, OnEndInit);

            EventBus.EventCrossroadSelect.Remove(OnInitCrossroad);
            EventBus.EventCrossroadSelect.Add(OnSelectCrossroad);

           _initMenu = null;
        }

        private void LookAtCamera(Transform cameraTransform)
        {
            if(_lookAtEnabled)
                _menusTransform.rotation = Quaternion.LookRotation(cameraTransform.forward);
        }

        private void OnActiveMenu(IMenu menu, bool value)
        {
            if (value)
            {
                _currentOpenMenu = menu;
                _lookAtEnabled = true;
            }
            else if (_currentOpenMenu == menu)
            {
                _currentOpenMenu = null;
                _lookAtEnabled = false;
            }
        }

#if UNITY_EDITOR
        [StartEditor]
        [SerializeField, Range(3f, 8f)] private float _buttonDistance = 5f;

        public void OnValidate()
        {
            if(Application.isPlaying) return;
            
            EUtility.SetObject(ref _canvasTransform, "CanvasWorld");

            _canvasTransform.SetChildren(ref _menusTransform, "ContextMenusWorld");

            _canvasTransform.SetChildren(ref _initMenu);
            _canvasTransform.SetChildren(ref _crossroadMenu);
            _canvasTransform.SetChildren(ref _recruitingMenu);
            _canvasTransform.SetChildren(ref _roadsMenu);
            _canvasTransform.SetChildren(ref _warriorsMenu);
            _canvasTransform.SetChildren(ref _buttonCancel);

            SetDistance();
        }

        private void SetDistance()
        {
            _initMenu.SetButtonPosition(_buttonDistance);
            _crossroadMenu.SetButtonPosition(_buttonDistance);
            _recruitingMenu.SetButtonPosition(_buttonDistance);
            _warriorsMenu.SetButtonPosition(_buttonDistance);

            _buttonCancel.transform.localScale = Vector3.zero;
        }
#endif
    }
}
