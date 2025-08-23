using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    [System.Serializable]
    public abstract class ActorSettings : IDisposable
    {
        [SerializeField] private int _id;
        [SerializeField] private IdArray<ActorAbilityId, int> _abilities;
        [SerializeField] private Skills _skills;
        [SerializeField] private ActorSkin _prefabActorSkin;

        public abstract int TypeId { get; }
        public int Id => _id;
        public AbilitiesSet<ActorAbilityId> Abilities => new(_abilities, ActorAbilityId.SHIFT_ABILITY, ActorAbilityId.MAX_ID_SHIFT_ABILITY);
        public Skills Skills => _skills;

        public ActorSkin InstantiateActorSkin(Id<PlayerId> owner, Transform parent) => UnityEngine.Object.Instantiate(_prefabActorSkin, parent).Init(owner, _skills);
        public void CreateStates(Actor actor) => _skills.CreateStates(actor);

        public void Init()
        {
#if UNITY_EDITOR
            if (_prefabActorSkin != null)
#endif
            _skills.Init(TypeId, _id);
        }

        public void Dispose()
        {
            _skills.Dispose();
        }

#if UNITY_EDITOR
        public void OnValidate() => _skills.OnValidate(TypeId);

        public bool UpdateName_Ed(string oldName, string newName) => _skills.UpdateName_Ed(oldName, newName);
        public void UpdateAnimation_Ed() => _skills.UpdateAnimation_Ed((AnimatorOverrideController)_prefabActorSkin.GetComponent<Animator>().runtimeAnimatorController);
#endif
    }
}
