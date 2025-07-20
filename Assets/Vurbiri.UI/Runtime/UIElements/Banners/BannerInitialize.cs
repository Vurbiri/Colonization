using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.UI
{
	internal class BannerInitialize : MonoBehaviour
	{
		public Banner prefab;
        public Transform container;
        public int startCount;
        [Space]
        public Vector2 maxSize;
        public Vector2 padding;
        public float space;
        public Direction2 direction;
        public float moveSpeed;
        [Space]
        public IdArray<MessageTypeId, Color> colors;

        private void Awake()
        {
            Banner.Init(this);
            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EUtility.SetPrefab(ref prefab);
            this.SetComponent(ref container);
        }
#endif
    }
}
