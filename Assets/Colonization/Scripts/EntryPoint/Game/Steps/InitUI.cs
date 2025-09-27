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
        [SerializeField] private WorldHint _worldHint;
        [SerializeField] private CanvasHint _canvasHint;
        [SerializeField] private PlayerPanels _playerPanels;
        [SerializeField] private OpponentPanels _opponentPanels;
        [SerializeField] private ChaosPanel _chaosPanel;
        [SerializeField] private SatanPanel _satanPanel;

        public string Description => Localization.Instance.GetText(LangFiles.Main, "InitUIStep");
        public float Weight => 0.2f;

        public ILoadingStep Init(GameContent content)
        {
            content.worldHint = _worldHint;
            content.canvasHint = _canvasHint;

            return this;
        }

        public IEnumerator GetEnumerator()
        {
            _worldHint.Init();
            _canvasHint.Init(true);

            yield return null;

            _managers.Init();

            yield return null;

            _playerPanels.Init();

            yield return null;
            
            _opponentPanels.Init();
            _chaosPanel.Init();
            _satanPanel.Init();

            yield return null;

            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetObject(ref _managers);
                EUtility.SetObject(ref _worldHint);
                EUtility.SetObject(ref _canvasHint);
                EUtility.SetObject(ref _playerPanels);
                EUtility.SetObject(ref _opponentPanels);
                EUtility.SetObject(ref _chaosPanel);
                EUtility.SetObject(ref _satanPanel);
            }
        }
#endif
    }
}
