//Assets\Colonization\Scripts\Diplomacy\Diplomacy.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Diplomacy : IReactive<int, int>
	{
		private readonly int[] _values = new int[PlayerId.PlayersCount];
        private readonly DiplomacySettings _stt;

        private Action<int, int> actionValueChange;

        #region Constructors
        public Diplomacy(DiplomacySettings settings) 
        {
            _stt = settings;
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _values[i] = _stt.defaultValue;
        }
        public Diplomacy(IReadOnlyList<int> values, DiplomacySettings settings)
        {
            _stt = settings;
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _values[i] = values[i];
        }
        #endregion

        public Relation GetRelation(Id<PlayerId> idA, Id<PlayerId> idB)
		{
            if (idA == PlayerId.None | idB == PlayerId.None)
                return Relation.None;

            if (idA == idB)
				return Relation.Friend;
			
			if(idA == PlayerId.Demons | idB == PlayerId.Demons)
				return Relation.Enemy;

			return _values[idA + idB - 1] > 0 ? Relation.Friend : Relation.Enemy;
        }

        public void ActorsInteraction(Id<PlayerId> idA, Id<PlayerId> idB, Relation relation)
        {
            if (idA == idB | idA == PlayerId.None | idB == PlayerId.None | idA == PlayerId.Demons | idB == PlayerId.Demons)
                return;


        }

        #region IReactive
        public IUnsubscriber Subscribe(Action<int, int> action, bool calling = true)
        {
            actionValueChange += action;

            if (calling)
            {
                for (int i = 0; i < PlayerId.PlayersCount; i++)
                    action(i, _values[i]);
            }

            return new Unsubscriber<Action<int, int>>(this, action);
        }

        public void Unsubscribe(Action<int, int> action) => actionValueChange -= action;
        #endregion
    }
}
