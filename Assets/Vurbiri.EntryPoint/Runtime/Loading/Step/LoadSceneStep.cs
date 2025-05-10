//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\LoadSceneStep.cs
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri
{
    sealed internal class LoadSceneStep : ALoadingStep
    {
        private readonly AsyncOperation _operation;

        public override float Progress => _operation.progress;
        public bool AllowSceneActivation { get => _operation.allowSceneActivation; set => _operation.allowSceneActivation = value; }

        public LoadSceneStep(int sceneBuildIndex, string desc) : base(desc)
        {
            _operation = SceneManager.LoadSceneAsync(sceneBuildIndex);
        }

        public override bool MoveNext() => !_operation.isDone;
    }
}
