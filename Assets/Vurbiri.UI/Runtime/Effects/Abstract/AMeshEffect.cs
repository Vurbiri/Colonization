using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(Graphic))]
    public abstract class AMeshEffect : MonoBehaviour, IMeshModifier
    {
        [SerializeField, HideInInspector] protected Graphic _graphic;
                
        public void ModifyMesh(Mesh mesh)
        {
            using var vh = new VertexHelper(mesh);
            ModifyMesh(vh);
            vh.FillMesh(mesh);
        }

        public abstract void ModifyMesh(VertexHelper verts);

        protected virtual void OnEnable()
        {
            _graphic.SetVerticesDirty();
        }

        protected virtual void OnDisable()
        {
            _graphic.SetVerticesDirty();
        }

        protected virtual void OnDidApplyAnimationProperties()
        {
            _graphic.SetVerticesDirty();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
           this.SetComponent(ref _graphic);
            _graphic.SetVerticesDirty();
        }
#endif
    }
}
