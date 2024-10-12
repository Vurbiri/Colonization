using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vurbiri.UI;

namespace Vurbiri
{
    public class AProjectEntryPoint : MonoBehaviour
    {
        [SerializeField] protected string _keySave = "CLN";
        [Space]
        [SerializeField] protected LoadScene _startScene;
        [SerializeField] private SceneId _emptyScene;
        [Space]
        [SerializeField] protected LoadingScreen _loadingScreen;

        protected DIContainer _container = new(null);

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _loadingScreen.Init(true);
            _container.AddInstance(_loadingScreen);
        }

        protected IEnumerator LoadScene(int sceneId)
        {
            _loadingScreen.TurnOnOf(true);

            yield return SceneManager.LoadSceneAsync(_emptyScene);
            yield return SceneManager.LoadSceneAsync(sceneId);

            yield return null;

            FindAndRunScene();
        }

        protected void FindAndRunScene()
        {
            ASceneEntryPoint scene = FindAnyObjectByType<ASceneEntryPoint>();
            if (scene == null)
                throw new System.Exception("Точка входа на сцену не найдена");

            scene.Run(_container);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_loadingScreen == null)
                _loadingScreen = FindAnyObjectByType<LoadingScreen>();
        }
#endif
    }
}
