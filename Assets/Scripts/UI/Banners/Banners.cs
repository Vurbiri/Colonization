using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class Banners : ASingleton<Banners>
{
    [SerializeField] private Banner _prefab;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _repository;
    [Space]
    [SerializeField] int _sizePool = 3;
    [Space]
    [SerializeField] private Color[] _colors;
    [Header("Desktop")]
    [SerializeField] private float _fontSizeDesktop = 14;
    [SerializeField] private Vector2 _resolutionDesktop = new(1600, 1000);
    [SerializeField] private ScreenMatchMode _matchModeDesktop = ScreenMatchMode.Expand;
    [Header("Mobile")]
    [SerializeField] private float _fontSizeMobile = 14;
    [SerializeField] private Vector2 _resolutionMobile = new(1000, 1600);
    [SerializeField] private ScreenMatchMode _matchModeMobile = ScreenMatchMode.Expand;
   
    private Pool<Banner> _banners;

    public Color[] Colors => _colors;
    public float FontSize { get; private set; }

    public void Initialize()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (SettingsGame.InstanceF.IsDesktop)
        {
            FontSize = _fontSizeDesktop;
            scaler.referenceResolution = _resolutionDesktop;
            scaler.screenMatchMode = _matchModeDesktop;
        }
        else
        {
            FontSize = _fontSizeMobile;
            scaler.referenceResolution = _resolutionMobile;
            scaler.screenMatchMode = _matchModeMobile;
        }

        Canvas.ForceUpdateCanvases();

        _banners = new(_prefab, _repository, _sizePool);
    }

    public void Message(string message, MessageType messageType, float time, bool isThrough)
    {
        _banners.GetObject(_container).Setup(message, messageType, time, isThrough);
    }

    public void Clear()
    {
        Transform child;
        while (_container.childCount > 0) 
        {
            child = _container.GetChild(0);
            child.GetComponent<Banner>().Deactivate();
            child.SetParent(_repository);
        }
    }
}
