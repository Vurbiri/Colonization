using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            protected abstract class Defense<T> : State<T> where T : AI<TSettings, TActorId, TStateId>
            {
                private int _oldForce, _chance;
                private bool _isBuff, _isBlock;

                [Impl(256)] protected Defense(T parent) : base(parent) { }

                sealed public override bool TryEnter()
                {
                    int enemiesForce = Status.nighEnemies.Force;
                    if (enemiesForce <= _oldForce)
                        _chance >>= 1;
                    else
                        _chance = enemiesForce * s_settings.ratioForDefence / Actor.CurrentForce;

                   _isBuff = _isBlock = false;

                    if (Chance.Rolling(_chance))
                        (_isBuff, _isBlock) = Settings.defense.CanUsed(Actor);

                    _oldForce = enemiesForce;
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
