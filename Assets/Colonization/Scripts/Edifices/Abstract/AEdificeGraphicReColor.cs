using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class AEdificeGraphicReColor : AEdificeGraphic
    {
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField, Range(0, 5)] protected int _idMaterial;

        private Transform _transform;

        private void Awake()
        {
            //_meshRenderer.enabled = false;
            _transform = transform;
            _transform.localPosition = new(0f, 2f, 0f);
        }

        public override void Init(Id<PlayerId> playerId, IReadOnlyList<CrossroadLink> links)
        {
            _meshRenderer.SetSharedMaterial(SceneContainer.Get<HumansMaterials>()[playerId].materialLit, _idMaterial);

            StartCoroutine(Init_Cn());
        }

        private IEnumerator Init_Cn()
        {
            float progress = 2f;

            //_meshRenderer.enabled = true;
            while (progress > 0f) 
            {
                progress -= Time.deltaTime * 10f;
                _transform.localPosition = new(0f, progress, 0f);
                yield return null;
            }
            _transform.localPosition = new(0f, 0f, 0f);
            yield return new WaitRealtime(0.25f);

            //_meshRenderer.enabled = true;
            Destroy(this);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
        }
#endif
    }
}
