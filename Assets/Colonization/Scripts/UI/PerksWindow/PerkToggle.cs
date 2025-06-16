using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class PerkToggle : AHintToggle
    {
		[SerializeField, ReadOnly] private int _perkId;
        [SerializeField, ReadOnly] private string _key;
        [SerializeField, ReadOnly] private bool _enabled;


#if UNITY_EDITOR
        public void Init_Editor(Perk perk)
        {
            UnityEditor.SerializedObject self = new(this);
            self.FindProperty("_perkId").intValue = perk.Id;
            self.FindProperty("_key").stringValue = perk.keyDescription;
            self.FindProperty("_enabled").boolValue = perk.perkModifier == PerkModifierId.Enable;
            self.ApplyModifiedProperties();
        }
#endif
    }
}
