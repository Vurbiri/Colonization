using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class ACrossroadBuildMenu : ACrossroadMenu
    {
        [SerializeField] private CmButton _buttonBack;

        public virtual void Init(ACrossroadMenu mainMenu)
        {
            _players = SceneObjects.Get<Players>();

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
}
