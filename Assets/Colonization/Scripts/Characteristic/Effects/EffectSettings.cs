namespace Vurbiri.Colonization
{
    using UnityEngine;

    [System.Serializable]
    public class EffectSettings
    {
        [SerializeField] private Target _target;
        [SerializeField] private int _typeOperation;
        [SerializeField] private int _value;
        [SerializeField] private int _isAttack;
        [SerializeField] private int _duration;



        [SerializeField] private int _keyDescId;

        public Id<TargetOfEffectId> TargetActor => _target.actor;
        public int TargetAbility => _target.ability;
        public int Value => _value;
        public Id<TypeOperationId> TypeOperation => _typeOperation;
        public int Duration => _duration;
        public int KeyDescId => _keyDescId;

        public static implicit operator Effect(EffectSettings value) => new(value._target.ability, value._typeOperation, value._value, value._duration);

        [System.Serializable]
        public struct Target
        {
            public int actor;
            public int ability;
        }
    }
}
