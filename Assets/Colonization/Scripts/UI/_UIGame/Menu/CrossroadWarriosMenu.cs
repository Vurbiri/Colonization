using System.Collections.Generic;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadWarriorsMenu : ACrossroadMenuBuild
    {
        [SerializeField] private HintGlobal _hint;
        [Space]
        [SerializeField] private CmButton _buttonBack;
        [Space]
        [SerializeField] private ButtonHiring _buttonHiringPrefab;
        [SerializeField] private IdArray<WarriorsId, ButtonView> _warriorsView;

        private CrossroadMainMenu _mainMen;
        private readonly IdArray<WarriorsId, ButtonHiring> _buttons = new();

        public void Init(CrossroadMainMenu mainMenu, Players players, IReadOnlyList<ACurrencies> warriorPrices)
        {
            _mainMen = mainMenu;
            _players = players;

            _buttonBack.onClick.AddListener(OnBack);

            Transform thisTransform = transform;
            float angle = 360 / WarriorsId.Count;
            for (int i = 0; i < WarriorsId.Count; i++)
            {
                _buttons[i] = Instantiate(_buttonHiringPrefab, thisTransform).Init(i, _hint, warriorPrices[i], _warriorsView[i]);
                _buttons[i].transform.localPosition = Quaternion.Euler(0f, 0f, -angle * i) * Vector3.up * 4.7f;
            }

            _hint = null;
            _warriorsView = null;

            _thisGO.SetActive(false);

            #region Local: OnBack()
            //=================================
            void OnBack()
            {
                _thisGO.SetActive(false);
                _mainMen.Open();
            }
            #endregion
        }

        public override void Open(Crossroad crossroad)
        {
            base.Open(crossroad);

            for (int i = 0; i < WarriorsId.Count; i++)
            {
                _buttons[i].Color = _currentColor;
                _buttons[i].SetupHint(_playerCurrent.Resources);
            }


             _thisGO.SetActive(true);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            for (int i = 0; i < WarriorsId.Count; i++)
            {
                if (string.IsNullOrEmpty(_warriorsView[i].keyHint))
                    _warriorsView[i].keyHint = WarriorsId.Names[i];

            }

            if (_buttonHiringPrefab == null)
                _buttonHiringPrefab = VurbiriEditor.Utility.FindAnyPrefab<ButtonHiring>();

            if (_hint == null)
                _hint = FindAnyObjectByType<HintGlobal>();
        }
#endif        
    }

}
