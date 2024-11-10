namespace Vurbiri.Colonization
{
    using UnityEngine;

    [System.Serializable]
    public class EffectSettings : IAbilityModifierSettings
    {
        [SerializeField] private int _type;
        [SerializeField] private int _targetActor;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeOperation;
        [SerializeField] private int _value;
        [SerializeField] private int _duration = 1;
        [SerializeField] private string _keyDescription;

        public Id<EffectTypeId> Type => _type;
        public Id<RelationId> TargetActor => _targetActor;
        public Id<ActorAbilityId> TargetAbility => _targetAbility;
        public int Value => _value;
        public Id<TypeOperationId> TypeOperation => _typeOperation;
        public Chance Chance => new(100);
        public int Duration => _duration;
        public string KeyDescription => _keyDescription;
    }
}
