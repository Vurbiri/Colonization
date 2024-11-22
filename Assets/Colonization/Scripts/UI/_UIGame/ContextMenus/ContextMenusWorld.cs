//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\ContextMenusWorld.cs
using System.Collections;
using UnityEngine;

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
        [SerializeField] private LookAtCamera _lookAtCamera;

        private GameObject _thisGO;
        private Camera _camera;
        private Players _players;
        private RectTransform _thisRectTransform;

        public void Init(ContextMenuSettings settings)
        {
            _thisGO = gameObject;
            _camera = settings.camera;
            _players = settings.players;
            _thisRectTransform = GetComponent<RectTransform>();

            _lookAtCamera.Init(_camera);

            Debug.Log("Заменить _playerCurrent на _player!!!");

            _crossroadMenu.Init(_roadsMenu, _recruitingMenu, settings);
            _recruitingMenu.Init(_crossroadMenu, settings);
            _roadsMenu.Init(_crossroadMenu, settings);
            _warriorsMenu.Init(settings);

            _crossroadMenu.EventEnabled += EnableLook;
            _recruitingMenu.EventEnabled += EnableLook;
            _roadsMenu.EventEnabled += EnableLook;
            _warriorsMenu.EventEnabled += EnableLook;

            settings.eventBus.EventCrossroadSelect += OnSelectCrossroad;
            settings.eventBus.EventCrossroadUnselect += OnUnselectCrossroad;

            settings.eventBus.EventActorSelect += OnSelectWarrior;
            settings.eventBus.EventActorUnselect += OnUnselectWarrior;
        }

        private void OnSelectCrossroad(Crossroad crossroad)
        {
            //if (_players.Current.Id != PlayerId.Player)
            //    return;

            _warriorsMenu.Close();

            ToPosition(crossroad.Position);

            _crossroadMenu.Open(crossroad);
        }

        private void OnUnselectCrossroad(Crossroad crossroad)
        {
            _lookAtCamera.enabled = false;

            CrossroadMenusClose();
        }

        private void OnSelectWarrior(Actors.Actor actor)
        {
            //if (_players.Current.Id != PlayerId.Player)
            //    return;

            CrossroadMenusClose();

            ToPosition(actor.Position);
            _warriorsMenu.Open(actor);
        }

        private void OnUnselectWarrior(Actors.Actor warrior)
        {
            _lookAtCamera.enabled = false;

            _warriorsMenu.Close();
        }


        private void ToPosition(Vector3 position)
        {
            _lookAtCamera.enabled = true;

            Vector3 screenPosition = _camera.WorldToScreenPoint(position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenPosition, _camera, out Vector2 localPoint))
                _thisRectTransform.anchoredPosition = localPoint;
        }

        private void CrossroadMenusClose()
        {
            _crossroadMenu.Close();
            _roadsMenu.Close();
            _recruitingMenu.Close();
        }

        private void EnableLook(bool value)
        {
            if (!_thisGO.activeInHierarchy)
                return;
            
            StopAllCoroutines();
            StartCoroutine(EnableLook_Coroutine(value));
        }

        private IEnumerator EnableLook_Coroutine(bool value)
        {
            yield return null;
            _lookAtCamera.enabled = value;
        }

        private void OnDestroy()
        {
            _crossroadMenu.EventEnabled -= EnableLook;
            _recruitingMenu.EventEnabled -= EnableLook;
            _roadsMenu.EventEnabled -= EnableLook;
            _warriorsMenu.EventEnabled -= EnableLook;
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
            if (_lookAtCamera == null)
                _lookAtCamera = GetComponent<LookAtCamera>();
        }
#endif
    }
}
