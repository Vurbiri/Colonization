//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\ContextMenusWorld.cs
using System.Collections;
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

        private GameObject _thisGO;
        private Camera _camera;
        private bool _isNotPlayer;
        private RectTransform _thisRectTransform;
        private Unsubscriber _inTurn;

        public void Init(ContextMenuSettings settings)
        {
            _thisGO = gameObject;
            _camera = settings.camera;
            _thisRectTransform = GetComponent<RectTransform>();

            _lookAtCamera.Init(_camera);

            _buttonCancel.Init(settings.hint);
            _crossroadMenu.Init(_roadsMenu, _recruitingMenu, settings);
            _recruitingMenu.Init(_crossroadMenu, settings);
            _roadsMenu.Init(_crossroadMenu, settings);
            _warriorsMenu.Init(settings);

            _buttonCancel.EventEnabled += EnableLook;
            _crossroadMenu.EventEnabled += EnableLook;
            _recruitingMenu.EventEnabled += EnableLook;
            _roadsMenu.EventEnabled += EnableLook;
            _warriorsMenu.EventEnabled += EnableLook;
            
            settings.eventBus.EventCrossroadSelect += OnSelectCrossroad;
            settings.eventBus.EventActorSelect += OnSelectWarrior;

            settings.eventBus.EventUnselect += CloseAll;

            _inTurn = settings.turn.Subscribe(OnNextTurn);
        }

        private void OnSelectCrossroad(Crossroad crossroad)
        {
            CloseAll();

            if (_isNotPlayer)
                return;

            ToPosition(crossroad.Position);
            _crossroadMenu.Open(crossroad);
            _buttonCancel.Setup(crossroad);
        }

        private void OnSelectWarrior(Actor actor)
        {
            CloseAll();

            if (!actor.IsIdle | _isNotPlayer)
                return;

            ToPosition(actor.Position);
            _warriorsMenu.Open(actor);
            _buttonCancel.Setup(actor);
        }

        public void CloseAll()
        {
            _crossroadMenu.Close();
            _roadsMenu.Close();
            _recruitingMenu.Close();
            _warriorsMenu.Close();
            _buttonCancel.Disable();
        }

        private void ToPosition(Vector3 position)
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenPosition, _camera, out Vector2 localPoint))
                _thisRectTransform.anchoredPosition = localPoint;
        }

        private void OnNextTurn(ITurn turn)
        {
            _isNotPlayer = turn.CurrentId != PlayerId.Player;
            CloseAll();
        }

        private void EnableLook(bool value)
        {
            if (!_thisGO.activeInHierarchy)
                return;
            
            StopAllCoroutines();
            StartCoroutine(EnableLook_Cn(value));
        }

        private IEnumerator EnableLook_Cn(bool value)
        {
            yield return null;
            _lookAtCamera.enabled = value;
        }

        private void OnDestroy()
        {
            _buttonCancel.EventEnabled -= EnableLook;
            _crossroadMenu.EventEnabled -= EnableLook;
            _recruitingMenu.EventEnabled -= EnableLook;
            _roadsMenu.EventEnabled -= EnableLook;
            _warriorsMenu.EventEnabled -= EnableLook;
            _inTurn.Unsubscribe();
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
