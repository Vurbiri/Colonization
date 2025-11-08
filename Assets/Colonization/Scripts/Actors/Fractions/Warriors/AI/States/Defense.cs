using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Defense : AIState
        {
            private bool _isEscape, _isBuff, _isBlock;
            private Hexagon _target;

            public Defense(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                _isEscape = _isBuff = _isBlock = false;
                if(!Status.isInCombat && IsEnemyComing)
                {
                    _isEscape = (!Status.isGuard || Actor.CurrentHP < s_settings.minHPUnsiege) && EscapeChance(Status.forceNearTwoEnemies) && TryEscape(3, out _target);
                    _isBuff  = !_isEscape && s_settings.defenseBuff[Actor.Id].CanUsed(Action, Actor);
                    _isBlock = !_isEscape && Action.CanUsedSpecSkill() && _parent._blockChance.Roll;
                }

                return _isEscape | _isBuff | _isBlock;
            }

            public override void Dispose() => _target = null;

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return GameContainer.CameraController.ToPositionControlled(Actor.Position);

                if (_isEscape)
                {
                    yield return Move_Cn(_target);
                }
                else
                {
                    if (_isBuff)
                        yield return s_settings.defenseBuff[Actor.Id].Use(Action);
                    if (_isBlock && Action.CanUsedSpecSkill())
                        yield return Action.UseSpecSkill();
                }

                isContinue.Set(false);
                Exit();
            }
        }
    }
}
