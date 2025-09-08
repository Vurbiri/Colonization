using UnityEngine;

namespace Vurbiri.UI
{
	internal class BannerInitialize : MonoBehaviour
	{
        [SerializeField] private int _startCount;
        [Space]
        [SerializeField] private int _space;
        [SerializeField] private Direction2 _direction;
        [Space]
        [SerializeField] private Banner.Settings _settings;

        private void Awake()
        {
            Banner.Init(_settings.Init(_direction, _space, _startCount));
            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _settings.OnValidate(this);
        }
#endif
    }
}
