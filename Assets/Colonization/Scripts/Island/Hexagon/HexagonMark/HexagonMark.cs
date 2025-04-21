//Assets\Colonization\Scripts\Island\Hexagon\HexagonMark\HexagonMark.cs
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    sealed public class HexagonMark : APooledObject<HexagonMark>
    {
        private readonly Material _greenMaterial;
        private readonly Material _redMaterial;
        private readonly MeshRenderer _thisRenderer;
        private bool _isGreenMaterial;

        public HexagonMark(HexagonMarkFactory initObj, Action<HexagonMark, bool> callback) : base(initObj.gameObject, callback)
        {
            _greenMaterial = initObj.greenMaterial;
            _redMaterial = initObj.redMaterial;
            _thisRenderer = initObj.GetComponent<MeshRenderer>();
            _isGreenMaterial = _greenMaterial == _thisRenderer.sharedMaterial;

            Object.Destroy(initObj);
        }

        public HexagonMark View(bool isGreen)
        {
            if (_isGreenMaterial != isGreen)
                _thisRenderer.sharedMaterial = isGreen ? _greenMaterial : _redMaterial;

            _isGreenMaterial = isGreen;
            _gameObject.SetActive(true);

            return this;
        }
    }
}
