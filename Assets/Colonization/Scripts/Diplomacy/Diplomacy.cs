//Assets\Colonization\Scripts\Diplomacy\Diplomacy.cs
namespace Vurbiri.Colonization
{
    public class Diplomacy
	{
		private readonly int[] _values = new int[PlayerId.PlayersCount];

		public Id<RelationId> GetRelation(Id<PlayerId> idA, Id<PlayerId> idB)
		{
			if(idA == idB)
				return RelationId.Friend;
			
			if(idA == PlayerId.Demons || idB == PlayerId.Demons)
				return RelationId.Enemy;

			return _values[idA + idB - 1] > 0 ? RelationId.Friend : RelationId.Enemy;
        }

    }
}
