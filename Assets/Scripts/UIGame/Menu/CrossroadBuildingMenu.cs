using UnityEngine;

public class CrossroadBuildingMenu : ACrossroadBuildMenu
{
    [SerializeField] private UnityDictionary<CityType, CmButton> _cityButtons;

    public override void Initialize(ACrossroadMenu mainMenu)
    {
        base.Initialize(mainMenu);

        foreach (var button in _cityButtons)
            button.Value.onClick.AddListener(() => OnBuild(button.Key));

        gameObject.SetActive(false);

        #region Local: OnBuild()
        //=================================
        void OnBuild(CityType cityType)
        {
            _players.Current.CityBuild(_currentCrossroad, cityType);

            _currentCrossroad = null;
            gameObject.SetActive(false);
        }
        #endregion
    }

    public override void Open(Crossroad crossroad)
    {
        Player player = _players.Current;
        Color color = player.Color;
        _currentCrossroad = crossroad;

        foreach (var button in _cityButtons)
        {
            button.Value.targetGraphic.color = color;
            button.Value.interactable = _currentCrossroad.CanBuyCity(button.Key, player.Resources);
        }

        gameObject.SetActive(true);
    }
}
