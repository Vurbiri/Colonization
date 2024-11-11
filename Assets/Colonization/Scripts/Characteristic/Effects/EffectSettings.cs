namespace Vurbiri.Colonization
{
    using UnityEngine;

    [System.Serializable]
    public class EffectSettings
    {
        [SerializeField] private int _targetActor;
        [SerializeField] private int _targetAbility;
        [SerializeField] private int _typeOperation;
        [SerializeField] private int _value;
        [SerializeField] private int _duration = 1;

        public Id<TargetOfEffectId> TargetActor => _targetActor;
        public int TargetAbility => _targetAbility;
        public int Value => _value;
        public Id<TypeOperationId> TypeOperation => _typeOperation;
        public int Duration => _duration;
    }
}
