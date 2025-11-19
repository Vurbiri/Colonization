using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Escape : AIState
        {
            public override int Id => WarriorAIStateId.Escape;

            [Impl(256)] public Escape(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                bool isEscape = false;

                if (Status.isMove & (IsInCombat | IsEnemyComing))
                {
                    int enemiesForce, contraForce;
                    if(IsInCombat)
                    {
                        enemiesForce = Status.nearEnemies.Force;
                        contraForce = Status.nearEnemies.GetContraForce();
                    }
                    else
                    {
                        enemiesForce = Status.nighEnemies.Force;
                        contraForce = Status.nighEnemies.GetContraForce() + Actor.CurrentForce;
                    }

                    if (Status.isGuard)
                        enemiesForce /= (Hexagon.GetMaxDefense() + 2);

                    isEscape = Chance.Rolling((enemiesForce * s_settings.ratioForEscape) / contraForce - (s_settings.ratioForEscape + 1));
                }

                return isEscape;
            }

            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return GameContainer.CameraController.ToPositionControlled(Actor.Position);

                if (TryEscape(out Hexagon target))
                    yield return Actor.Move_Cn(target);

                yield return Settings.defense.TryUse_Cn(Actor, true, true, true);

                isContinue.Set(false);
                Exit();
            }

            private bool TryEscape(out Hexagon hexagon)
            {
                Hexagon temp; hexagon = null;
                int enemiesForce = (Status.nearEnemies.Force + Status.nighEnemies.Force)/(Hexagon.GetMaxDefense() + 1);
                var hexagons = Hexagon.Neighbors;

                for (int i = 0; i < HEX.SIDES; ++i)
                {
                    temp = hexagons[i];
                    if (temp.CanWarriorEnter && CheckHexagon(OwnerId, temp, enemiesForce, out int force))
                    {
                        enemiesForce = force;
                        hexagon = temp;
                    }
                }

                return hexagon != null;

                //=============== Local =======================
                static bool CheckHexagon(Id<PlayerId> id, Hexagon target, int maxForce, out int force)
                {
                    force = 0;
                    var hexagons = target.Neighbors;
                    for (int i = 0; i < HEX.SIDES; ++i)
                        if (hexagons[i].TryGetEnemy(id, out Actor actor))
                            force += actor.CurrentForce;
                    
                    force /= (target.GetMaxDefense(id) + 1);
                    return force < maxForce;
                }
            }
        }
    }
}
