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
                foreach (var neighbor in hexagon.Neighbors)
                    if (neighbor.IsWarrior)
                        return false;

                return true;
            }

            protected static bool NearWarriors(Hexagon hexagon)
            {
                foreach (var neighbor in hexagon.Neighbors)
                    if (neighbor.IsWarrior)
                        return true;

                return false;
            }
        }
    }
}
