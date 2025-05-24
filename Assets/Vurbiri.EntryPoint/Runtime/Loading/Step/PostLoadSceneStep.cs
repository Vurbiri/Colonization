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

        public override bool MoveNext()
        {
            UnityEngine.Debug.Log("PostLoadSceneStep");
            return _run;
        }
    }
}
