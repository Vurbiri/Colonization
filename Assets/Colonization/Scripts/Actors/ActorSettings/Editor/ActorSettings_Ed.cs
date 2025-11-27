#if UNITY_EDITOR

using UnityEngine;
using static Vurbiri.Colonization.ActorAbilityId;

namespace Vurbiri.Colonization
{
    public abstract partial class ActorSettings
    {
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
        }

        public void SetDefaultForce_Ed() => _force = Formulas.ActorForce(_abilities);

        public bool UpdateSFXName_Ed(string oldName, string newName)
        {
            return _prefabActorSkin ? _skills.UpdateSFXName_Ed(oldName, newName) : false;
        }

        public void UpdateAnimation_Ed()
        {
            if (_prefabActorSkin)
                _skills.UpdateAnimation_Ed(_prefabActorSkin.GetComponent<Animator>().runtimeAnimatorController as AnimatorOverrideController);
        }

        public void PrintForce_Ed() => Debug.Log($"{ActorTypeId.GetName(TypeId, _id)}: {_force}");
        public void PrintProfit_Ed(int main, int adv)
        {
            int ap = _abilities[MaxAP] + 1;
            Debug.Log($"[{ActorTypeId.GetName(TypeId, _id)}] Main: {_abilities[ProfitMain] * ap * main / 10000}. Adv: {_abilities[ProfitAdv] * ap * adv / 10000}");
        }

        public (int, int, int) GetAttackPierceDefence_Ed() => (_abilities[Attack], _abilities[Pierce], _abilities[Defense]);
    }
}
#endif