using UnityEngine;
using Vurbiri.EntryPoint;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed public class GameplayEntryPoint : ASceneEntryPoint
    {
        [SerializeField] private SceneId _nextScene;
        [Space]
        [SerializeField] private IslandCreator _islandCreator;
        [SerializeField] private InitUI _initUI;
        [Space]
        [SerializeField] private FileIds _localizationFiles = new(true);
        [Space]
        [SerializeField] private GameplayInitObjects _initObjects;

        [Space]
        [Header("══════ TEST ══════")]
        [SerializeField] private bool _isLoad;

        public override ISubscription<ExitParam> Enter(Loading loading, AEnterParam param)
        {
            GameplayContent content = new();
            GameplayContainer.GameSettings.IsLoad = _isLoad;

            Localization.Instance.SetFiles(_localizationFiles);

            _initObjects.CreateObjectsAndFillingContainer(content);
                        
            loading.Add(_islandCreator.Init(content));
            loading.Add(new CreatePlayers(_initObjects));
            loading.Add(_initUI.Init(_initObjects));
            loading.Add(new ClearResources());
            loading.Add(new GameplayStart());

            Destroy(this);

            return new SceneExitPoint(_nextScene, new GameplayContainer(content)).EventExit;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetObject(ref _islandCreator);
            EUtility.SetObject(ref _initUI);

            _initObjects.OnValidate();
        }
#endif
    }
}
