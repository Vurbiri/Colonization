using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class MoveToRaid : MoveToColony
            {
                [Impl(256)] public MoveToRaid(AI<TSettings, TActorId, TStateId> parent) : base(parent, 3) { }

                public override bool TryEnter()
                {
                    _targetHexagon = null;

                    if ((Settings.raider & Status.isMove & !Status.isSiege) && Status.percentHP > s_settings.minHPRaid)
                    {
                        _owners.Clear();
                        for (int i = 0; i < PlayerId.HumansCount; ++i)
                            if (GameContainer.Diplomacy.IsGreatEnemy(OwnerId, i))
                                _owners.Add(i);

                        TrySetColony(s_settings.maxDistanceRaid, 3);
                    }
                    return false;
                }

               
            }
        }
    }
}
