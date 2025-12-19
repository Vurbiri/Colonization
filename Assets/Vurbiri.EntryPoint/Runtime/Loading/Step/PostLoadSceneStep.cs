namespace Vurbiri.EntryPoint
{
    sealed internal class PostLoadSceneStep : AEnumeratorStep
    {
        private int _counter;

        public PostLoadSceneStep() : base("...") { }

        public ALoadingStep Restart() 
        {
            _counter = 5;
            return this;
        }

        public override bool MoveNext() => --_counter > 0;
    }
}
