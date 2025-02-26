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

        const int OWNER = PlayerId.Demons;

        public Demon Init(int id, Hexagon startHex)
        {
            _demon.Init(_demonSettings[id], _collider, OWNER, startHex);
            _collider.enabled = false;

            Destroy(this);

            return _demon;
        }

        public Demon Load(ActorLoadData data, Hexagon startHex)
        {
            _demon.Load(_demonSettings[data.id], _collider, OWNER, startHex, data);
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
