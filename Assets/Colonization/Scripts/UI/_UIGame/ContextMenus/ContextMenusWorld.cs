//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\ContextMenusWorld.cs
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;

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
        [Space]
        [SerializeField] private LookAtCamera _lookAtCamera;

        private Camera _camera;
        private RectTransform _thisRectTransform;
        private Unsubscriptions _unsubscribers = new(9);
        private bool _isPlayerTurn;
        private IMenu _currentOpenMenu;

        public void Init(ContextMenuSettings settings)
        {
            _camera = settings.camera;
            _thisRectTransform = GetComponent<RectTransform>();

            _lookAtCamera.Init(_camera);

            _unsubscribers += _buttonCancel.Init(settings.hint).Add(OnActiveMenu);
            _unsubscribers += _crossroadMenu.Init(_roadsMenu, _recruitingMenu, settings).Add(OnActiveMenu);
            _unsubscribers += _recruitingMenu.Init(_crossroadMenu, settings).Add(OnActiveMenu);
            _unsubscribers += _roadsMenu.Init(_crossroadMenu, settings).Add(OnActiveMenu);
            _unsubscribers += _warriorsMenu.Init(settings).Add(OnActiveMenu);

            _unsubscribers += settings.eventBus.EventCrossroadSelect.Add(OnSelectCrossroad);
            _unsubscribers += settings.eventBus.EventActorSelect.Add(OnSelectWarrior);

            _unsubscribers += settings.eventBus.EventUnselect.Add(OnClose);

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

        private void OnActiveMenu(IMenu menu, bool value)
        {
            if (value)
            {
                _currentOpenMenu = menu;
                _lookAtCamera.enabled = true;
            }
            else if (_currentOpenMenu == menu)
            {
                _currentOpenMenu = null;
                _lookAtCamera.enabled = false;
            }
        }

        private void OnDestroy()
        {
            _unsubscribers.Unsubscribe();
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
            if (_lookAtCamera == null)
                _lookAtCamera = GetComponent<LookAtCamera>();
        }
#endif
    }
}
