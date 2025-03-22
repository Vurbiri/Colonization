//Assets\Vurbiri\Runtime\Coroutine\WaitFrames.cs
namespace Vurbiri
{
    public class WaitFrames : UnityEngine.CustomYieldInstruction
    {
        private ushort _waitUntilFrames;
        private ushort _waitFrames;

        public ushort Frames
        {
            get => _waitFrames;
            set { _waitFrames = value; _waitUntilFrames = _waitFrames; }
        }

        public override bool keepWaiting
        {
            get
            {
                if (--_waitUntilFrames > 0)
                    return true;

                _waitUntilFrames = _waitFrames;
                return false;
            }
        }

        public WaitFrames(ushort frames)  => Frames = frames;

        public WaitFrames SetFrames(ushort value)
        {
            Frames = value;
            return this;
        }

        public override void Reset() => _waitUntilFrames = _waitFrames;
    }
}
