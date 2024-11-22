//Assets\Colonization\Scripts\Diplomacy\Ids\RelationId.cs
namespace Vurbiri.Colonization
{
    public class RelationId : AIdType<RelationId>
    {
        public const int Enemy = 0;
        public const int Friend = 1;

        static RelationId() => RunConstructor();
        private RelationId() { }
    }
}
