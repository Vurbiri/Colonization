namespace Vurbiri
{
    public class WaitFrames : System.Collections.IEnumerator
    {
        private ushort _waitFrames;
        private ushort _waitUntilFrames;

        public ushort Frames
        {
            get => _waitFrames;
            set { _waitFrames = value; _waitUntilFrames = _waitFrames; }
        }

        public object Current => null;

        public bool MoveNext()
        {
            if (--_waitUntilFrames > 0)
                return true;

            _waitUntilFrames = _waitFrames;
            return false;
        }

        public WaitFrames(ushort frames)  => Frames = frames;

        public WaitFrames Restart(ushort value)
        {
            Frames = value;
            return this;
        }

        public void Reset() => _waitUntilFrames = _waitFrames;
    }
}
