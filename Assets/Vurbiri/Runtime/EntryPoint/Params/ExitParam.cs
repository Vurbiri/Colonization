//Assets\Vurbiri\Runtime\EntryPoint\Params\ExitParam.cs
namespace Vurbiri.EntryPoint
{
    public class ExitParam
    {
        private readonly int _nextScene;
        private readonly AEnterParam _enterParam;

        public int NextScene => _nextScene;
        public AEnterParam EnterParam => _enterParam;

        public ExitParam(int nextScene, AEnterParam enterParam = null)
        {
            _nextScene = nextScene;
            _enterParam = enterParam;
        }
    }
}
