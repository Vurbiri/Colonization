//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\LoadScene.cs
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri
{
    [Serializable]
    sealed public class LoadScene : ALoadingStep
    {
        [SerializeField] private int _scene;

        private AsyncOperation _operation;

        public override float Progress => _operation.progress;

        public LoadScene() : base(string.Empty) { }

        public ALoadingStep Load()
        {
            _operation = SceneManager.LoadSceneAsync(_scene);
            return this;
        }

        public void LoadAndWait()
        {
            _operation = SceneManager.LoadSceneAsync(_scene);
            _operation.allowSceneActivation = false;
        }
        public void EndWait() => _operation.allowSceneActivation = true;

        public override bool MoveNext() => !_operation.isDone;
    }
}
