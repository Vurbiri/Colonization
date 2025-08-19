using UnityEngine;
using Vurbiri.Colonization.Actors.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    [System.Serializable]
    public class PoolEffectsBarFactory
	{
        [SerializeField] private EffectsBarFactory _prefabEffectsBar;
        [SerializeField] private Transform _repositoryUI;
        [SerializeField] private int _startCount;
        [Space]
        [SerializeField] private Vector2 _startPosition = new(3.7f, -0.9f);
        [SerializeField] private Vector2 _offsetPosition = new(-0.7f, -0.9f);
        [Space]
        [SerializeField] private int _maxIndex = 7;

        public Pool<EffectsBar> Create()
        {
            _prefabEffectsBar.CreateSettings(_startPosition, _offsetPosition, _maxIndex);
            return new Pool<EffectsBar>(_prefabEffectsBar.Create, _repositoryUI, _startCount);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetPrefab(ref _prefabEffectsBar);
                EUtility.SetObject(ref _repositoryUI, "WorldUIRepository");
            }
        }
#endif
    }
}
