//Assets\Colonization\Scripts\SFX\Spell\SpellSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.SFX
{
    public class SpellSFX : MonoBehaviour
	{
        private Transform _thisTransform;
        private ParticleSystem _psSpell;
        private ParticleSystem.MainModule _mainPS;

        private void Awake()
		{
           _thisTransform = transform;
            
            _psSpell = GetComponent<ParticleSystem>();
			_mainPS = _psSpell.main;

            _psSpell.Stop();
        }

        public void Setup(Vector2 position, float time)
        {
            _thisTransform.localPosition = position;
            _mainPS.duration = time;
        }
		
#if UNITY_EDITOR
        private void OnValidate()
        {
			
        }
#endif
	}
}
