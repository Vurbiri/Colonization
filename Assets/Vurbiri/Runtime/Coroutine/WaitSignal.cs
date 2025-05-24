using UnityEngine;

namespace Vurbiri
{
    sealed public class WaitSignal : CustomYieldInstruction
    {
        public override bool keepWaiting => _keepWaiting;
        private bool _keepWaiting;

        public WaitSignal() => _keepWaiting = true;

        public void Send() => _keepWaiting = false;
        public override void Reset() => _keepWaiting = true;
    }
}
