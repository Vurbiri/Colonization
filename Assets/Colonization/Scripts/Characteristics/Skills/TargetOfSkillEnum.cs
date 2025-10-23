namespace Vurbiri.Colonization
{
    public enum TargetOfSkill
    {
        Enemy,
        Friend,
        Self,
    }

    public static class ExtensionsTargetOfSkill
    {
        public static Relation ToRelation(this TargetOfSkill skill) => skill switch
        {
            TargetOfSkill.Enemy => Relation.Enemy,
            TargetOfSkill.Friend => Relation.Friend,
            _ => Relation.None
        };
    }
}
