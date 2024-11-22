//Assets\Vurbiri\Runtime\Coroutine\WaitActivate.cs
using UnityEngine;

namespace Vurbiri
{
    public class WaitActivate : CustomYieldInstruction
    {
        public override bool keepWaiting => _keepWaiting;
        private bool _keepWaiting = true;

        public WaitActivate()
        {
            _keepWaiting = true;
        }

        public void Activate() => _keepWaiting = false;
        public WaitActivate Deactivate()
        {
            _keepWaiting = true;
            return this;
        }
    }
}
