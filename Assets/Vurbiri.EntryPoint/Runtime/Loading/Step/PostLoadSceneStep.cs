//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\PostLoadSceneStep.cs
namespace Vurbiri.EntryPoint
{
    sealed internal class PostLoadSceneStep : AEnumeratorStep
    {
        private bool _run;

        public PostLoadSceneStep() : base("...") { }

        public void Stop() => _run = false;
        public ALoadingStep Restart() 
        {
            _run = true;
            return this;
        }

        public override bool MoveNext() => _run;
    }
}
