//Assets\Colonization\Scripts\EntryPoint\Gameplay\Steps\InitUI.cs
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
        [SerializeField] private WorldHint _worldHint;
        [SerializeField] private ContextMenusWorld _contextMenusWorld;
        [Space]
        [SerializeField] private CanvasHint _canvasHint;
        [SerializeField] private PlayerPanels _playerPanelsUI;

        private GameplayInitObjects _initObjects;

        public string Description => Localization.Instance.GetText(Files.Main, "InitUIStep");
        public float Weight => 0.2f;

        public InitUI Init(GameplayInitObjects objects)
        {
            _initObjects = objects;

            return this;
        }

        public IEnumerator GetEnumerator()
        {
            ProjectColors colors = SceneContainer.Get<ProjectColors>();

            _worldHint.Init(colors.HinBack ,colors.HintDefault);
            _contextMenusWorld.Init(_initObjects.GetContextMenuSettings(_worldHint));
            yield return null;
            _canvasHint.Init(colors.HinBack, colors.HintDefault);
            _playerPanelsUI.Init(_initObjects.players.Player, colors, _initObjects.inputController, _canvasHint);
            yield return null;

            Destroy(gameObject);
            _initObjects = null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _worldHint);
            EUtility.SetObject(ref _contextMenusWorld);

            EUtility.SetObject(ref _canvasHint);
            EUtility.SetObject(ref _playerPanelsUI);
        }
#endif
    }
}
