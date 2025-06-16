using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class PerksWindow : VToggleGroup
    {
#if UNITY_EDITOR

        public RectTransform UpdateVisuals_Editor(float side)
        {
            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = new(side, side);

            return thisRectTransform;
        }

        [Header("┌──────────── Editor ─────────────────────")]
        [SerializeField] private PerksScriptable _perks;
        [SerializeField] private PerkToggle _prefab;
        [SerializeField, ReadOnly] private PerkToggle[] _economic = new PerkToggle[EconomicPerksId.Count];

        protected override void OnValidate()
        {
            base.OnValidate();

            _allowSwitchOff = true;

            EUtility.SetScriptable(ref _perks);
            EUtility.SetPrefab(ref _prefab);
            EUtility.SetArray(ref _economic, EconomicPerksId.Count);
        }

        public void Create_Editor()
        {
            for (int i = 0; i < EconomicPerksId.Count; i++)
            {
                if (_economic[i] == null)
                {
                    _economic[i] = (PerkToggle)UnityEditor.PrefabUtility.InstantiatePrefab(_prefab);
                    _economic[i].transform.SetParent(transform, false);
                }
            }
        }
        public void Delete_Editor()
        {
            var thisTransform = transform;
            while (thisTransform.childCount > 0)
            {
                DestroyImmediate(thisTransform.GetChild(0).gameObject);
            }
        }

#endif
    }
}
