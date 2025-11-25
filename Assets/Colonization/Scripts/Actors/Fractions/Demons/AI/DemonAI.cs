using System;
using System.Runtime.CompilerServices;
using Vurbiri.Collections;
using static Vurbiri.Colonization.Actor;

namespace Vurbiri.Colonization
{
    public partial class DemonAI : AI<DemonsAISettings, DemonId, DemonAIStateId>
    {
        private static string s_msg;
        private static readonly Func<AI<DemonsAISettings, DemonId, DemonAIStateId>, State>[] s_spec = new Func<AI<DemonsAISettings, DemonId, DemonAIStateId>, State>[DemonId.Count];

        static DemonAI()
        {
            s_spec[DemonId.Imp]   = ImpSpecSkill.Create;
            s_spec[DemonId.Bomb]  = BombSpecSkill.Create;
            s_spec[DemonId.Grunt] = GruntSpecSkill.Create;
            s_spec[DemonId.Fatty] = FattySpecSkill.Create;
            s_spec[DemonId.Boss]  = BossSpecSkill.Create;

            IdArray<DemonAIStateId, System.Func<AI<DemonsAISettings, DemonId, DemonAIStateId>, State>> factories = new();
            factories[DemonAIStateId.ExitFromGate]  = GetExitFromGate;
            factories[DemonAIStateId.UseSpecSkill]  = GetUseSpecSkill;
            factories[DemonAIStateId.Combat]        = GetCombat;
            factories[DemonAIStateId.MoveToHelp]    = GetMoveToHelp;
            factories[DemonAIStateId.MoveToRaid]    = GetMoveToRaid;
            factories[DemonAIStateId.MoveToEnemy]   = GetMoveToEnemy;
            factories[DemonAIStateId.Healing]       = GetHealing;
            factories[DemonAIStateId.Defense]       = GetDefense;
            factories[DemonAIStateId.MoveToAttack]  = GetMoveToAttack;
            factories[DemonAIStateId.FreeFinding]   = GetFreeFinding;
            SetFactories(factories);

            s_msg = "[DemonAI] Initialized";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DemonAI(Actor actor, Goals goals) : base(actor, goals) { }

        public static void Start()
        {
            if (s_msg != null)
            {
                Log.Info(s_msg);
                s_msg = null;
            }
        }

        private static State GetExitFromGate(AI<DemonsAISettings, DemonId, DemonAIStateId> parent) => new ExitFromGate(parent);
        private static State GetUseSpecSkill(AI<DemonsAISettings, DemonId, DemonAIStateId> parent) => s_spec[parent.ActorId](parent);
        private static State GetMoveToEnemy(AI<DemonsAISettings, DemonId, DemonAIStateId> parent)  => new MoveToEnemy(parent);
        private static State GetFreeFinding(AI<DemonsAISettings, DemonId, DemonAIStateId> parent)  => new FreeFinding(parent);
    }
}
