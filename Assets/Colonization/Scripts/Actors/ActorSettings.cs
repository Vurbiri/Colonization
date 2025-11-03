using System;
using UnityEngine;
using Vurbiri.Collections;
using static Vurbiri.Colonization.ActorAbilityId;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public abstract class ActorSettings : IDisposable
    {
        [SerializeField] private int _id;
        [SerializeField] private IdArray<ActorAbilityId, int> _abilities;
        [SerializeField] private Skills _skills;
        [SerializeField] private ActorSkin _prefabActorSkin;
        [SerializeField] private int _force;

        public abstract int TypeId { get; }
        public int Id => _id;
        public AbilitiesSet<ActorAbilityId> Abilities => new(_abilities, SHIFT_ABILITY, MAX_ID_SHIFT_ABILITY);
        public Skills Skills => _skills;
        public int Force => _force;

        public T InstantiateSkin<T>(Id<PlayerId> owner, Transform parent) where T : ActorSkin
        {
            T skin = (T)Object.Instantiate((Object)_prefabActorSkin, parent, false);
            skin.Init(owner, _skills);
            return skin;
        }

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
                _skills.OnValidate(TypeId, _id);
            }
            else
            {
                string prefabName = $"P_{ActorTypeId.GetName(TypeId, _id)}Skin";
                _prefabActorSkin = EUtility.FindAnyPrefab<ActorSkin>(prefabName);
            }

            _force = Formulas.ActorForce(_abilities);
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

        public void PrintForce_Ed() => Debug.Log($"{ActorTypeId.GetName(TypeId, _id)}: {_force}");
        public void PrintProfit_Ed(int main, int adv)
        {
            int ap = _abilities[MaxAP] + 1;
            Debug.Log($"[{ActorTypeId.GetName(TypeId, _id)}] Main: {_abilities[ProfitMain] * ap * main / 10000}. Adv: {_abilities[ProfitAdv] * ap * adv / 10000}");
        }
#endif
    }
}
