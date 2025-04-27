//Assets\Vurbiri.UI\Runtime\UIElements\Abstract\AVSelectable.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    public abstract partial class AVSelectable : Selectable
    {
        [SerializeField] protected Graphic _interactableIcon;
        [SerializeField] private bool _alphaCollider = false;
        [SerializeField, Range(0.01f, 1f)] private float _threshold = 0.1f;
        [SerializeField] protected List<TargetGraphic> _targetGraphics = new();

        public new bool interactable
        {
            get => base.interactable;
            set
            {
                if (base.interactable == value) return;

                base.interactable = value;
                if (_interactableIcon != null)
                    _interactableIcon.CrossFadeAlpha(value ? 0f : 1f, colors.fadeDuration, true);
            }
        }

        public Graphic InteractableIcon => _interactableIcon;
        public int TargetGraphicCount => _targetGraphics.Count;

        public Graphic GetTargetGraphic(int index) => _targetGraphics[index].Graphic;

        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                if (!_targetGraphics[i].IsValid) _targetGraphics.RemoveAt(i);

            _targetGraphics.TrimExcess();

            if (_targetGraphics.Count > 0)
                targetGraphic = _targetGraphics[0];
        }

        protected override void Start()
        {
            base.Start();

            if (_interactableIcon != null)
                _interactableIcon.canvasRenderer.SetAlpha(base.interactable ? 0f : 1f);

            Image image = targetGraphic as Image;
            if (_alphaCollider && image != null && image.sprite != null && image.sprite.texture.isReadable)
                image.alphaHitTestMinimumThreshold = _threshold;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            _targetGraphics = new()
            {
                GetComponent<Graphic>()
            };
        }
#endif
    }
}
