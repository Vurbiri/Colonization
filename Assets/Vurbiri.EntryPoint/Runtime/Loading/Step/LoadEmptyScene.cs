using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri.EntryPoint
{
    [Serializable]
    sealed public class LoadEmptyScene : ALoadingStep, IEnumerator
    {
        [SerializeField] private int _scene;

        private AsyncOperation _operation;

        public LoadEmptyScene() : base(string.Empty) { }

        public object Current => null;

        public override IEnumerator GetEnumerator()
        {
            _operation = SceneManager.LoadSceneAsync(_scene);
            return this;
        }

        public bool MoveNext() => !_operation.isDone;

        public void Reset() { }
    }
}
