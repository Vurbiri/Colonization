using UnityEngine;

public class BuildingMenus : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasTransform;
    [Space]
    [SerializeField] private Crossroads _crossroads;
    [Space]
    [SerializeField] private BuildingMainMenu _mainMenu;
    [SerializeField] private BuildingRoadsMenu _roadsMenu;

    private RectTransform _thisTransform;
   
    private Camera _camera;
    private Vector2 _localPoint;

    private void Start()
    {
        _thisTransform = GetComponent<RectTransform>();
        _camera = Camera.main;

        _crossroads.EventSelectCrossroad += OnSelectCrossroad;

        _mainMenu.Initialize(_roadsMenu);
        _roadsMenu.Initialize(_mainMenu);


        #region Local: OnSelectCrossroad()
        //=================================
        void OnSelectCrossroad(Crossroad crossroad)
        {
            //_mainMenu.Close();
            _roadsMenu.Close();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, _camera.WorldToScreenPoint(crossroad.Position), _camera, out _localPoint))
                _thisTransform.anchoredPosition = _localPoint;

            _mainMenu.Open(crossroad);
        }
        #endregion
    }
}
