using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class AttackSkillUI
    {
        [SerializeField] private string _keyName;
        [SerializeField] private Sprite _sprite;

        public string KeyName => _keyName;
        public Sprite Sprite => _sprite;
        
    }
}
