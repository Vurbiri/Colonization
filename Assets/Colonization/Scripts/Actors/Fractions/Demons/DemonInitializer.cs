//Assets\Colonization\Scripts\Actors\Fractions\Demons\DemonInitializer.cs
using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    public class DemonInitializer : MonoBehaviour
	{
        [SerializeField] private Demon _demon;
        [SerializeField] private BoxCollider _collider;
        [Space]
        [SerializeField] private DemonsSettingsScriptable _demonSettings;

        public Demon Init(int id, ActorInitData initData, Hexagon startHex)
        {
            _demon.Init(_demonSettings[id], initData, _collider, startHex);
            _collider.enabled = false;

            Destroy(this);

            return _demon;
        }

        public Demon Load(ActorLoadData data, ActorInitData initData, Hexagon startHex)
        {
            _demon.Load(_demonSettings[data.id], initData, _collider, startHex, data);
            _collider.enabled = false;

            Destroy(this);

            return _demon;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_demon == null)
                _demon = GetComponent<Demon>();
            if (_collider == null)
                _collider = GetComponent<BoxCollider>();
            if (_demonSettings == null)
                _demonSettings = EUtility.FindAnyScriptable<DemonsSettingsScriptable>();
        }
#endif
    }
}
