using UnityEngine;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenusWorld : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasTransform;
        [Space]
        [SerializeField] private CrossroadMainMenu _crossroadMenu;
        [SerializeField] private CrossroadWarriorsMenu _recruitingMenu;
        [SerializeField] private CrossroadRoadsMenu _roadsMenu;
        [Space]
        [SerializeField] private WarriorsMenu _warriorsMenu;
        [Space]
        [SerializeField] private ButtonCancel _buttonCancel;

        private Camera _camera;
        private RectTransform _thisRectTransform;
        private bool _isPlayerTurn, _lookAtEnabled;
        private IMenu _currentOpenMenu;

        public void Init(ContextMenuSettings settings)
        {
            _camera = settings.camera;
            _thisRectTransform = GetComponent<RectTransform>();

            _buttonCancel  .Init(settings.hint).Add(OnActiveMenu);
            _crossroadMenu .Init(settings, _roadsMenu, _recruitingMenu).Add(OnActiveMenu);
            _recruitingMenu.Init(settings, _crossroadMenu).Add(OnActiveMenu);
            _roadsMenu     .Init(settings, _crossroadMenu).Add(OnActiveMenu);
            _warriorsMenu  .Init(settings).Add(OnActiveMenu);

            settings.eventBus.EventCrossroadSelect.Add(OnSelectCrossroad);
            settings.eventBus.EventActorSelect.Add(OnSelectWarrior);

            settings.eventBus.EventUnselect.Add(OnClose);

            settings.cameraTransform.Subscribe(LookAtCamera);

            settings.game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            settings.game.Subscribe(GameModeId.Play, OnGamePlay);
        }

        private void OnSelectCrossroad(Crossroad crossroad)
        {
            if (_isPlayerTurn & crossroad.Interactable & _currentOpenMenu == null)
            {
                ToPosition(crossroad.Position);
                _crossroadMenu.Open(crossroad);
                _buttonCancel.Setup(crossroad);
            }
        }

        private void OnSelectWarrior(Actor actor)
        {
            if (actor.Interactable & _currentOpenMenu == null)
            {
                ToPosition(actor.Position);
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
            _isPlayerTurn = false;
            OnClose(false);
        }
        private void OnGamePlay(TurnQueue turnQueue, int dice)
        {
            _isPlayerTurn = turnQueue.IsCurrentPlayer;
        }

        private void ToPosition(Vector3 position)
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenPosition, _camera, out Vector2 localPoint))
                _thisRectTransform.anchoredPosition = localPoint;
        }

        private void LookAtCamera(Transform cameraTransform)
        {
            if(_lookAtEnabled)
                _thisRectTransform.rotation = Quaternion.LookRotation(cameraTransform.forward);
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
        private void OnValidate()
        {
            if (_canvasTransform == null)
                _canvasTransform = transform.parent.GetComponent<RectTransform>();
            if (_crossroadMenu == null)
                _crossroadMenu = GetComponentInChildren<CrossroadMainMenu>();
            if (_recruitingMenu == null)
                _recruitingMenu = GetComponentInChildren<CrossroadWarriorsMenu>();
            if (_roadsMenu == null)
                _roadsMenu = GetComponentInChildren<CrossroadRoadsMenu>();
            if (_warriorsMenu == null)
                _warriorsMenu = GetComponentInChildren<WarriorsMenu>();
            if (_buttonCancel == null)
                _buttonCancel = GetComponentInChildren<ButtonCancel>();
        }
#endif
    }
}
