//Assets\Vurbiri\Runtime\Coroutine\WaitActivate.cs
using UnityEngine;

namespace Vurbiri
{
    public class WaitActivate : CustomYieldInstruction
    {
        public override bool keepWaiting => _keepWaiting;
        private bool _keepWaiting;

        public WaitActivate() => _keepWaiting = true;

        public void Activate() => _keepWaiting = false;
        public override void Reset() => _keepWaiting = true;
    }
}
