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

        public ActorSkin InstantiateActorSkin(Id<PlayerId> owner, Transform parent) => UnityEngine.Object.Instantiate(_prefabActorSkin, parent, false).Init(owner, _skills);
        public T InstantiateActorSkin<T>(Id<PlayerId> owner, Transform parent) where T : ActorSkin
        {
            return (T)UnityEngine.Object.Instantiate(_prefabActorSkin, parent, false).Init(owner, _skills);
        }
        public void CreateStates(Actor actor) => _skills.CreateStates(actor);

        public void Init()
        {
            _skills.Init(TypeId, _id);
        }

        public void Dispose()
        {
            _skills.Dispose();
        }

#if UNITY_EDITOR

        public void OnValidate()
        {
            if (_prefabActorSkin)
            {
                _skills.OnValidate(TypeId);
            }
            else
            {
                string prefabName = $"P_{(TypeId == ActorTypeId.Warrior ? WarriorId.GetName_Ed(_id) : DemonId.GetName_Ed(_id))}Skin";
                _prefabActorSkin = EUtility.FindAnyPrefab<ActorSkin>(prefabName);
            }
        }
        public bool UpdateName_Ed(string oldName, string newName)
        {
            return _prefabActorSkin ? _skills.UpdateName_Ed(oldName, newName) : false;
        }
        public void UpdateAnimation_Ed()
        {
            if(_prefabActorSkin)
                _skills.UpdateAnimation_Ed(_prefabActorSkin.GetComponent<Animator>().runtimeAnimatorController as AnimatorOverrideController);
        }
#endif
    }
}
