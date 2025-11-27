using System.Collections.Generic;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

namespace Vurbiri.Colonization
{
	public class SkillHits_Ed
	{
		private readonly string _name;
		private readonly List<SkillHit_Ed> _hits = new();

        public bool IsValid => _hits.Count > 0;
        public int Count => _hits.Count;

        public SkillHits_Ed(string name) { _name = name; }

		public void Add(SkillHit_Ed hit) => _hits.Add(hit);

        public void Update(int attack, int pierce, int defense, bool isHoly)
        {
            for (int i = 0; i < _hits.Count; ++i)
                _hits[i].Update(attack, pierce, defense, isHoly);
        }

        public void Draw()
        {
            LabelField(_name, EditorStyles.whiteBoldLabel);
            ++EditorGUI.indentLevel;

            for (int i = 0; i < _hits.Count; ++i)
                LabelField(_hits[i].damage); 

            --EditorGUI.indentLevel;
        }
    }
    public class SkillHit_Ed
    {
        private readonly int _percent;
        private readonly int _holy;
        private readonly int _pierce;
        public string damage;

        public SkillHit_Ed(int percent, int holy, int pierce)
        {
            _percent = percent;
            _holy = holy;
            _pierce = pierce;
        }

        public void Update(int attack, int pierceOther, int defense, bool isHoly)
        {
            if (isHoly)
                damage = Formulas.Damage(attack * (_percent + _holy) / 100, defense * (100 - _pierce - pierceOther) / 100).ToString();
            else
                damage = Formulas.Damage(attack * _percent / 100, defense * (100 - _pierce - pierceOther) / 100).ToString();
        }
    }

}
