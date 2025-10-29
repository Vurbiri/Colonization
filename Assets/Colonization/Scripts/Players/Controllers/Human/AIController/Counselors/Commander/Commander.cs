using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Commander : Counselor
        {
            private readonly Dictionary<int, Warrior.AI> _ai = new(CONST.DEFAULT_MAX_WARRIOR);
            
            public Commander(AIController parent) : base(parent)
            {
                Human.Actors.Subscribe(OnActor);
            }

            public override IEnumerator Execution_Cn()
            {
                yield break;
            }

            private void OnActor(Actor actor, TypeEvent type)
            {
                if (type == TypeEvent.Subscribe | type == TypeEvent.Add)
                    _ai.Add(actor.Index, Create(actor));
                else if (type == TypeEvent.Remove)
                    _ai.Remove(actor.Index);

                // ====== Local ======
                [Impl(256)] static Warrior.AI Create(Actor actor) => actor.Id switch
                {
                    WarriorId.Militia => new MilitiaAI(actor),
                    WarriorId.Solder  => new SolderAI(actor),
                    WarriorId.Wizard  => new WizardAI(actor),
                    WarriorId.Warlock => new WarlockAI(actor),
                    WarriorId.Knight  => new KnightAI(actor),
                    _ => null
                };
            }
        }
    }
}
