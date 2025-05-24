using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri.EntryPoint
{
    [Serializable]
    sealed public class LoadScene : AEnumeratorStep
    {
        [SerializeField] private int _scene;

        private AsyncOperation _operation;

        public LoadScene() : base(string.Empty) { }

        public ALoadingStep Load()
        {
            _operation = SceneManager.LoadSceneAsync(_scene);
            return this;
        }

        public override bool MoveNext() => !_operation.isDone;
    }
}
