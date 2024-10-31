using UnityEngine;

namespace Vurbiri
{
    public class Banners : ASingleton<Banners>
    {
        [SerializeField] private Banner _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _repository;
        [Space]
        [SerializeField] private int _sizePool = 3;
        [Space]
        [SerializeField] private Color[] _colors;
        [Header("Desktop")]
        [SerializeField] private float _fontSize = 14;

        private Pool<Banner> _banners;

        public Color[] Colors => _colors;
        public float FontSize => _fontSize;

        public void Init()
        {
            _banners = new(_prefab, _repository, _sizePool);
        }

        public void Message(string message, MessageType messageType, float time, bool isThrough)
        {
            _banners.Get(_container).Setup(message, messageType, time, isThrough);
        }

        public void Clear()
        {
            Transform child;
            while (_container.childCount > 0)
            {
                child = _container.GetChild(0);
                child.GetComponent<Banner>().ToPool();
                child.SetParent(_repository);
            }
        }
    }
}
