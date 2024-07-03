using UnityEngine;

public class CrossroadMenus : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasTransform;
    [Space]
    [SerializeField, GetComponentInChildren] private CrossroadMainMenu _mainMenu;
    [SerializeField, GetComponentInChildren] private CrossroadRoadsMenu _roadsMenu;
    [SerializeField, GetComponentInChildren] private CrossroadBuildingMenu _buildMenu;

    private RectTransform _thisTransform;
    private Camera _camera;
    private Vector2 _localPoint;

    private void Start()
    {
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;

        EventBus.Instance.EventCrossroadSelect += OnSelectCrossroad;

        _mainMenu.Initialize(_roadsMenu, _buildMenu);
        _roadsMenu.Initialize(_mainMenu);
        _buildMenu.Initialize(_mainMenu);

        #region Local: OnSelectCrossroad()
        //=================================
        void OnSelectCrossroad(Crossroad crossroad)
        {
            _roadsMenu.Close();
            _buildMenu.Close();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, _camera.WorldToScreenPoint(crossroad.Position), _camera, out _localPoint))
                _thisTransform.anchoredPosition = _localPoint;

            _mainMenu.Open(crossroad);
        }
        #endregion
    }
}
