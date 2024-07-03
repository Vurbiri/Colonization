using UnityEngine;

public abstract class ACrossroadBuildMenu : ACrossroadMenu
{
    [SerializeField] private CmButton _buttonBack;

    public virtual void Initialize(ACrossroadMenu mainMenu)
    {
        _players = Players.Instance;

        _buttonBack.onClick.AddListener(OnBack);

        #region Local: OnBack()
        //=================================
        void OnBack()
        {
            gameObject.SetActive(false);
            mainMenu.Open();
        }
        #endregion
    }
}