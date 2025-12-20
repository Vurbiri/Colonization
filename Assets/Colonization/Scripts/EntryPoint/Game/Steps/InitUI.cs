using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    public class InitUI : MonoBehaviour, ILoadingStep
    {
        [SerializeField] private UIManagers _managers;
        [Space]
        [SerializeField] private PlayerPanels _playerPanels;
        [SerializeField] private OpponentPanels _opponentPanels;
        [SerializeField] private ChaosPanel _chaosPanel;
        [SerializeField] private SatanPanel _satanPanel;
        [SerializeField] private ButtonsPanel _buttonsPanel;
        [Space]
        [SerializeField] private MessageBoxColors _messageBoxColors;

        public string Description => Localization.Instance.GetText(LangFiles.Main, "InitUIStep");
        public float Weight => 0.2f;

        public IEnumerator GetEnumerator()
        {
            MessageBox.SetColors(_messageBoxColors);

            _managers.Init();

            yield return null;

            _playerPanels.Init();

            yield return null;
            
            _opponentPanels.Init();
            _chaosPanel.Init();
            _satanPanel.Init();

            yield return null;

            _buttonsPanel.Init();

            yield return null;

            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetObject(ref _managers);
                EUtility.SetObject(ref _playerPanels);
                EUtility.SetObject(ref _opponentPanels);
                EUtility.SetObject(ref _chaosPanel);
                EUtility.SetObject(ref _satanPanel);
                EUtility.SetObject(ref _buttonsPanel);
            }
        }

        public void SetColors_Ed(SceneColorsEd colors)
        {
            _messageBoxColors.window = colors.panelBack;
            _messageBoxColors.text = colors.panelText;
            _messageBoxColors.button = colors.elements;
        }
#endif
    }
}
