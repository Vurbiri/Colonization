using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenusWorld : MonoBehaviour
    {
        private RectTransform _canvasTransform;
        private CrossroadMainMenu _crossroadMenu;
        private CrossroadWarriorsMenu _warriorsMenu;
        private CrossroadRoadsMenu _roadsMenu;

        private Camera _camera;
        private RectTransform _thisTransform;
        private Vector2 _localPoint;

        public void Init(Players players, Camera camera, PricesScriptable prices, GameplayEventBus eventBus)
        {
            _camera = camera;

            _crossroadMenu = GetComponentInChildren<CrossroadMainMenu>();
            _warriorsMenu = GetComponentInChildren<CrossroadWarriorsMenu>();
            _roadsMenu = GetComponentInChildren<CrossroadRoadsMenu>();
            _thisTransform = GetComponent<RectTransform>();
            _canvasTransform = _thisTransform.parent.GetComponent<RectTransform>();

            eventBus.EventCrossroadSelect += OnSelectCrossroad;

            _crossroadMenu.Init(_roadsMenu, _warriorsMenu, players,  prices);
            _warriorsMenu.Init(_crossroadMenu, players, prices.Warriors);
            _roadsMenu.Init(_crossroadMenu, players, camera, prices.Road);

            #region Local: OnSelectCrossroad()
            //=================================
            void OnSelectCrossroad(Crossroad crossroad)
            {
                _roadsMenu.Close();
                _warriorsMenu.Close();

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, _camera.WorldToScreenPoint(crossroad.Position), _camera, out _localPoint))
                    _thisTransform.anchoredPosition = _localPoint;

                _crossroadMenu.Open(crossroad);
            }
            #endregion
        }
    }
}
