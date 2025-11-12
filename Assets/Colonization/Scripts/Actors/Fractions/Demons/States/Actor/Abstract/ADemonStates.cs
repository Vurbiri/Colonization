using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
    sealed public partial class Demon
    {
        public abstract partial class ADemonStates<TSkin> : AStates<Demon, TSkin> where TSkin : ActorSkin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected ADemonStates(Demon actor, ActorSettings settings) : base(actor, settings) { }
            
            sealed public override bool IsAvailable => _stateMachine.IsDefaultState;

            protected static bool NearNoWarriors(Hexagon hexagon)
            {
                var neighbors = hexagon.Neighbors;
                for (int i = 0; i < neighbors.Count; ++i)
                    if (neighbors[i].IsWarrior)
                        return false;
                return true;
            }

            protected static bool NearWarriors(Hexagon hexagon)
            {
                var neighbors = hexagon.Neighbors;
                for (int i = 0; i < neighbors.Count; ++i)
                    if (neighbors[i].IsWarrior)
                        return true;
                return false;
            }
        }
    }
}
