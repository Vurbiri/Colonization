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
        [SerializeField] private ContextMenusManager _contextMenusWorld;
        [Space]
        [SerializeField] private CanvasHint _canvasHint;
        [SerializeField] private PlayerPanels _playerPanels;
        
        private GameContent _content;

        public string Description => Localization.Instance.GetText(LangFiles.Main, "InitUIStep");
        public float Weight => 0.2f;

        public ILoadingStep Init(GameContent content)
        {
            _content = content;

            content.worldHint = _worldHint;
            content.canvasHint = _canvasHint;

            return this;
        }

        public IEnumerator GetEnumerator()
        {
            _worldHint.Init();
            _canvasHint.Init();
            yield return null;
            _managers.Init(_content);
            _playerPanels.Init();
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
            }
        }
#endif
    }
}
