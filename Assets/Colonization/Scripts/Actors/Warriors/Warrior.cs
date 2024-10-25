using UnityEngine;

namespace Vurbiri.Colonization
{
    public class Warrior : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Animator _animator;
        
        private int _id;


        

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();
            if(_animator == null)
                _animator = GetComponentInChildren<Animator>();
        }
#endif
    }
}
