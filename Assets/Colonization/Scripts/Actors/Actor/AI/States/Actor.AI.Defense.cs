using System.Collections;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            protected abstract class Defense<T> : State<T> where T : AI
            {
                private bool _isBuff, _isBlock;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                protected Defense(T parent) : base(parent) { }

                sealed public override bool TryEnter()
                {
                    _isBuff = _isBlock = false;
                    if (IsEnemyComing)
                        (_isBuff, _isBlock) = Settings.defense.CanUsed(Actor);

                    return _isBuff | _isBlock;
                }

                sealed public override void Dispose() { }

                sealed public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    yield return GameContainer.CameraController.ToPositionControlled(Actor.Position);
                    yield return Settings.defense.Use_Cn(Actor, _isBuff, _isBlock);

                    isContinue.Set(false);
                    Exit();
                }
            }
        }
    }
}
