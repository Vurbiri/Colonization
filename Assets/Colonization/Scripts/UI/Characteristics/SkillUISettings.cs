using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class SkillUISettings
	{
        [SerializeField] protected string _keyName;
        [SerializeField] protected Sprite _sprite;
        [SerializeField] protected int _cost;

        private SkillUISettings() { }

        protected SkillUISettings(SkillUISettings other)
        {
            _keyName = other._keyName;
            _sprite = other._sprite;
            _cost = other._cost;
        }
    }
}
