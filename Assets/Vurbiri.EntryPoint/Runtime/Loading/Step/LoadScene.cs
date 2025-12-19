using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri.EntryPoint
{
    sealed internal class LoadScene : ILoadingStep, System.Collections.IEnumerator
    {
        private readonly Func<string> _getDesc;
        private AsyncOperation _operation;
        private int _scene;

        public float Weight => 0.99f;
        public string Description => _getDesc();

        public object Current => null;

        public LoadScene(Func<string> getDesc) => _getDesc = getDesc;

        public LoadScene Load(int sceneBuildIndex)
        {
            _scene = sceneBuildIndex;
            return this;
        }

        public IEnumerator GetEnumerator()
        {
            _operation = SceneManager.LoadSceneAsync(_scene);
            return this;
        }

        public bool MoveNext() => !_operation.isDone;

        public void Reset() { }
    }
}
