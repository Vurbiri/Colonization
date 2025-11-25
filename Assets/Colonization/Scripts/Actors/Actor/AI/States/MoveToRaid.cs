namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class MoveToRaid : MoveToColony
            {
                private readonly bool _isHuman;
                
                public MoveToRaid(AI<TSettings, TActorId, TStateId> parent) : base(parent, 2, s_settings.maxDistanceRaid) 
                {
                    _isHuman = OwnerId != PlayerId.Satan;
                    if (!_isHuman)
                    {
                        _owners.Capacity = PlayerId.HumansCount;
                        for (int i = 0; i < PlayerId.HumansCount; ++i)
                            _owners.Add(i);
                    }
                }

                public override bool TryEnter()
                {
                    _targetHexagon = null;
                    if ((Settings.raider & Status.isMove & !Status.isSiege) && Status.percentHP > s_settings.minHPRaid)
                    {
                        if (_isHuman)
                        {
                            _owners.Clear();
                            for (int i = 0; i < PlayerId.HumansCount; ++i)
                                if (GameContainer.Diplomacy.IsGreatEnemy(OwnerId, i))
                                    _owners.Add(i);
                        }
                        return TrySetColony(s_settings.maxDistanceRaid);
                    }
                    return false;
                }
            }
        }
    }
}
