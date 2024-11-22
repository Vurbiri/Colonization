//Assets\Vurbiri\Runtime\Utilities\UI\Menu\MenuNavigation.cs
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    [Header("��������� ������"), Space]
    [SerializeField] private Buttons[] _buttons;

    protected virtual void Awake()
    {
        foreach (var button in _buttons)
            button.Setup(gameObject);
    }

    //public void SetButtonsActive(bool active)
    //{
    //    foreach (var button in _buttons)
    //        button.SetActive(active);
    //}

    [System.Serializable]
    private class Buttons
    {
        [SerializeField] private Button _button;
        [Space]
        [SerializeField] private bool _closeCurrentMenu = true;
        [SerializeField] private GameObject _openMenu;
        [SerializeField] private UnityEvent _otherActions;

        public void Setup(GameObject closeMenu)
        {
            if (!_button) return;

            if (_otherActions != null)
                _button.onClick.AddListener(_otherActions.Invoke);

            _button.onClick.AddListener(() =>
            {
                if (closeMenu && _closeCurrentMenu) closeMenu.SetActive(false);
                if (_openMenu) _openMenu.SetActive(true);
                //if (_openMenu) _openMenu.SetActive(!_openMenu.activeSelf);
            });
   
        }

        //public void SetActive(bool active) => _button.gameObject.SetActive(active);
    }
}
