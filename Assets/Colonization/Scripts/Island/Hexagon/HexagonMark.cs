//Assets\Colonization\Scripts\Island\Hexagon\HexagonMark.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent (typeof (MeshRenderer))]
    public class HexagonMark : APooledObject<HexagonMark>
    {
        [SerializeField] private Material _greenMaterial;
        [SerializeField] private Material _redMaterial;

        private MeshRenderer _thisRenderer;
        private bool _isGreenMaterial;

        public override void Init()
        {
            base.Init();
            _thisRenderer = GetComponent<MeshRenderer>();
            _isGreenMaterial = _greenMaterial == _thisRenderer.sharedMaterial;
        }

        public HexagonMark View(bool isGreen)
        {
            if (_isGreenMaterial != isGreen)
                _thisRenderer.sharedMaterial = isGreen ? _greenMaterial : _redMaterial;

            _isGreenMaterial = isGreen;
            _thisGObj.SetActive(true);

            return this;
        }
    }
}
