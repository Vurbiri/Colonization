using UnityEngine;
using Vurbiri.Colonization.Actors.UI;

namespace Vurbiri.Colonization.EntryPoint
{
    [System.Serializable]
    public class PoolEffectsBarFactory
	{
        [SerializeField] private EffectsBarFactory _prefabEffectsBar;
        [SerializeField] private Transform _repositoryUI;

        public Pool<EffectsBar> Create()
        {
            return new Pool<EffectsBar>(_prefabEffectsBar.Create, _repositoryUI, 3);
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
