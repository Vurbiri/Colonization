using System.Runtime.CompilerServices;
using static Vurbiri.Colonization.Actor;

namespace Vurbiri.Colonization
{
    public partial class DemonAI : AI<DemonsAISettings, DemonId, DemonAIStateId>
    {
        private static string s_msg;

        static DemonAI()
        {


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
    }
}
