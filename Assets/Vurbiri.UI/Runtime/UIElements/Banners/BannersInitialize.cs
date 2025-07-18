using UnityEngine;

namespace Vurbiri.UI
{
	internal class BannersInitialize : MonoBehaviour
	{
		[SerializeField] private Banner _prefab;
        [SerializeField] private RectTransform _container;
        [Space]
        [SerializeField] private Vector2 _bannerMaxSize;
        [SerializeField] private Vector2 _bannerPadding;
        [SerializeField] private float _space;
        [SerializeField] private Direction2 _bannerDirect;
        [SerializeField] private float _moveSpeed;

        private void Awake()
        {
            Banner.Init(_bannerMaxSize, _bannerPadding, _bannerDirect);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetPrefab(ref _prefab);
            this.SetComponent(ref _container);

        }
#endif
    }
}
