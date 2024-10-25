using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenusWorld : MonoBehaviour
    {
        [SerializeField] private PricesScriptable _prices;
        [Space]
        [SerializeField] private RectTransform _thisRectTransform;
        [SerializeField] private RectTransform _canvasTransform;
        [Space]
        [SerializeField] private CrossroadMainMenu _crossroadMenu;
        [SerializeField] private CrossroadWarriorsMenu _warriorsMenu;
        [SerializeField] private CrossroadRoadsMenu _roadsMenu;
        [Space]
        [SerializeField] private LookAtCamera _lookAtCamera;

        protected GameObject _thisGO;
        private Camera _camera;

        public void Init(Players players, Camera camera, GameplayEventBus eventBus)
        {
            _thisGO = gameObject;
            _camera = camera;

            _lookAtCamera.Init(camera);

            _crossroadMenu.Init(_roadsMenu, _warriorsMenu, players,  _prices);
            _warriorsMenu.Init(_crossroadMenu, players, _prices.Warriors);
            _roadsMenu.Init(_crossroadMenu, players, camera, _prices.Road);

            _crossroadMenu.EventEnabled += EnableLook;
            _warriorsMenu.EventEnabled += EnableLook;
            _roadsMenu.EventEnabled += EnableLook;

            eventBus.EventCrossroadSelect += OnSelectCrossroad;
            eventBus.EventCrossroadUnselect += OnUnselectCrossroad;
        }

        private void OnSelectCrossroad(Crossroad crossroad)
        {
            _lookAtCamera.enabled = true;

            Vector3 screenPosition = _camera.WorldToScreenPoint(crossroad.Position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenPosition, _camera, out Vector2 localPoint))
                _thisRectTransform.anchoredPosition = localPoint;

            _crossroadMenu.Open(crossroad);
        }

        private void OnUnselectCrossroad(Crossroad crossroad)
        {
            _lookAtCamera.enabled = false;

            _crossroadMenu.Close();
            _roadsMenu.Close();
            _warriorsMenu.Close();
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
            _warriorsMenu.EventEnabled -= EnableLook;
            _roadsMenu.EventEnabled -= EnableLook;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_prices == null)
                _prices = VurbiriEditor.Utility.FindAnyScriptable<PricesScriptable>();
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();
            if (_canvasTransform == null)
                _canvasTransform = _thisRectTransform.parent.GetComponent<RectTransform>();
            if (_crossroadMenu == null)
                _crossroadMenu = GetComponentInChildren<CrossroadMainMenu>();
            if (_warriorsMenu == null)
                _warriorsMenu = GetComponentInChildren<CrossroadWarriorsMenu>();
            if (_roadsMenu == null)
                _roadsMenu = GetComponentInChildren<CrossroadRoadsMenu>();
            if (_lookAtCamera == null)
                _lookAtCamera = GetComponent<LookAtCamera>();
        }
#endif
    }
}
