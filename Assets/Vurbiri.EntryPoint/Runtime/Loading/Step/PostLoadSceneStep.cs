//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\PostLoadSceneStep.cs
namespace Vurbiri.EntryPoint
{
    sealed internal class PostLoadSceneStep : ALoadingStep
    {
        private bool _run;
        public PostLoadSceneStep() : base(string.Empty) { }
        
        public override bool MoveNext() => _run;

        public void Stop() => _run = false;
        public ALoadingStep Restart(string desc) 
        {
            _run = true;
            _desc = desc;
            return this;
        }
       
    }
}
