using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class InitUI : MonoBehaviour, ILoadingStep
    {
        [SerializeField] private UIManagers _managers;
        [Space]
        [SerializeField] private WorldHint _worldHint;
        [SerializeField] private ContextMenusWorld _contextMenusWorld;
        [Space]
        [SerializeField] private CanvasHint _canvasHint;
        [SerializeField] private PlayerPanels _playerPanelsUI;
        

        private GameplayInitObjects _init;

        public string Description => Localization.Instance.GetText(Files.Main, "InitUIStep");
        public float Weight => 0.2f;

        public InitUI Init(GameplayInitObjects init)
        {
            _init = init;

            return this;
        }

        public IEnumerator GetEnumerator()
        {
            var colors = SceneContainer.Get<ProjectColors>();
            var player = _init.players.Player;

            _managers.Init(_init.game, _init.cameraController, player, _canvasHint);
            yield return null;
            _worldHint.Init(colors.HintBack ,colors.HintDefault);
            _contextMenusWorld.Init(_init.GetContextMenuSettings(_worldHint));
            yield return null;
            _canvasHint.Init(colors.HintBack, colors.HintDefault);
            _playerPanelsUI.Init(player, colors, _init.inputController, _canvasHint);
            yield return null;

            Destroy(gameObject);
            _init = null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _managers);

            EUtility.SetObject(ref _worldHint);
            EUtility.SetObject(ref _contextMenusWorld);

            EUtility.SetObject(ref _canvasHint);
            EUtility.SetObject(ref _playerPanelsUI);
        }
#endif
    }
}
