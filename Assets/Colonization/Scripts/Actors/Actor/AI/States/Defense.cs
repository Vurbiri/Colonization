using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class Defense : State
            {
                private int _oldForce, _chance;
                private bool _isBuff, _isBlock;

                [Impl(256)] public Defense(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                public override bool TryEnter()
                {
                    if(!Settings.defense.IsValid) return false;

                    int enemiesForce = Status.nighEnemies.Force;
                    if (enemiesForce > 0 && enemiesForce <= _oldForce)
                        _chance >>= 1;
                    else
                        _chance = enemiesForce * s_settings.ratioForDefence / Actor.CurrentForce;

                    _isBuff = _isBlock = false;

                    if (Chance.Rolling(_chance))
                        (_isBuff, _isBlock) = Settings.defense.CanUsed(Actor);

                    _oldForce = enemiesForce;
                    return _isBuff | _isBlock;
                }

                public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    yield return GameContainer.CameraController.ToPositionControlled(Actor);
                    yield return Settings.defense.Use_Cn(Actor, _isBuff, _isBlock);

                    isContinue.Set(false);
                    Exit();
                }

                sealed public override void Dispose() { }
            }
        }
    }
}
