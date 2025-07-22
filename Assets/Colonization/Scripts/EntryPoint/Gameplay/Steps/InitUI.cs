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
        
        private GameplayInitObjects _init;

        public string Description => Localization.Instance.GetText(LangFiles.Main, "InitUIStep");
        public float Weight => 0.2f;

        public InitUI Init(GameplayInitObjects init)
        {
            _init = init;

            return this;
        }

        public IEnumerator GetEnumerator()
        {
            var colors = SceneContainer.Get<ProjectColors>();
            var player = _init.content.players.Person;

            _worldHint.Init();
            _canvasHint.Init();
            yield return null;
            _managers.Init(_init.content, _canvasHint, _init.GetContextMenuSettings(_worldHint));
            yield return null;
            _playerPanels.Init(player, colors, _init.content.inputController, _canvasHint);
            yield return null;

            Destroy(gameObject);
            _init = null;
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
