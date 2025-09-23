namespace Vurbiri.EntryPoint
{
    public class ExitParam
    {
        private readonly int _nextScene;
        private readonly IEnterParam _enterParam;

        public int NextScene => _nextScene;
        public IEnterParam EnterParam => _enterParam;

        public ExitParam(int nextScene, IEnterParam enterParam = null)
        {
            _nextScene = nextScene;
            _enterParam = enterParam;
        }
    }
}
