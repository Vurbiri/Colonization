using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenusWorld : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasTransform;
        [Space]
        [SerializeField] private CrossroadMainMenu _crossroadMenu;
        [SerializeField] private CrossroadRoadsMenu _roadsMenu;

        private Camera _camera;
        private RectTransform _thisTransform;
        private Vector2 _localPoint;

        public void Init(Camera camera, PricesScriptable prices, GameplayEventBus eventBus)
        {
            _camera = camera;

            _thisTransform = GetComponent<RectTransform>();

            eventBus.EventCrossroadSelect += OnSelectCrossroad;

            _crossroadMenu.Init(_roadsMenu, prices);
            _roadsMenu.Init(_crossroadMenu, prices.Road);

            #region Local: OnSelectCrossroad()
            //=================================
            void OnSelectCrossroad(Crossroad crossroad)
            {
                _roadsMenu.Close();

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, _camera.WorldToScreenPoint(crossroad.Position), _camera, out _localPoint))
                    _thisTransform.anchoredPosition = _localPoint;

                _crossroadMenu.Open(crossroad);
            }
            #endregion
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_crossroadMenu == null)
                _crossroadMenu = GetComponentInChildren<CrossroadMainMenu>();
            if (_roadsMenu == null)
                _roadsMenu = GetComponentInChildren<CrossroadRoadsMenu>();
        }
#endif
    }
}
