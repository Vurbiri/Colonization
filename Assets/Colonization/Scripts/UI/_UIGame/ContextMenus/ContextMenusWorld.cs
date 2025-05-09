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
        private bool _isNotPlayerTurn;
        private RectTransform _thisRectTransform;
        private Unsubscribers _unsubscribers = new(9);
        private GameObject _currentOpenMenu;

        public void Init(ContextMenuSettings settings)
        {
            _camera = settings.camera;
            _thisRectTransform = GetComponent<RectTransform>();

            _lookAtCamera.Init(_camera);

            _unsubscribers += _buttonCancel.Init(settings.hint).Add(EnableLook);
            _unsubscribers += _crossroadMenu.Init(_roadsMenu, _recruitingMenu, settings).Add(EnableLook);
            _unsubscribers += _recruitingMenu.Init(_crossroadMenu, settings).Add(EnableLook);
            _unsubscribers += _roadsMenu.Init(_crossroadMenu, settings).Add(EnableLook);
            _unsubscribers += _warriorsMenu.Init(settings).Add(EnableLook);

            _unsubscribers += settings.eventBus.EventCrossroadSelect.Add(OnSelectCrossroad);
            _unsubscribers += settings.eventBus.EventActorSelect.Add(OnSelectWarrior);

            _unsubscribers += settings.eventBus.EventUnselect.Add(CloseAll);

            _unsubscribers += settings.turn.Subscribe(OnNextTurn);
        }

        private void OnSelectCrossroad(Crossroad crossroad)
        {
            CloseAll();

            if (_isNotPlayerTurn)
                return;

            ToPosition(crossroad.Position);
            _crossroadMenu.Open(crossroad);
            _buttonCancel.Setup(crossroad);
        }

        private void OnSelectWarrior(Actor actor)
        {
            CloseAll();

            if (!actor.Interactable)
                return;

            ToPosition(actor.Position);
            _warriorsMenu.Open(actor);
            _buttonCancel.Setup(actor);
        }

        public void CloseAll()
        {
            _crossroadMenu.CloseInstant();
            _roadsMenu.CloseInstant();
            _recruitingMenu.CloseInstant();
            _warriorsMenu.CloseInstant();
            _buttonCancel.Disable();
        }

        private void ToPosition(Vector3 position)
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenPosition, _camera, out Vector2 localPoint))
                _thisRectTransform.anchoredPosition = localPoint;
        }

        private void OnNextTurn(TurnQueue turn)
        {
            _isNotPlayerTurn = turn.CurrentId != PlayerId.Player;
            CloseAll();
        }

        private void EnableLook(GameObject signaling, bool value)
        {
            if (value)
            {
                _currentOpenMenu = signaling;
                _lookAtCamera.enabled = true;
            }
            else if (_currentOpenMenu == signaling)
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
