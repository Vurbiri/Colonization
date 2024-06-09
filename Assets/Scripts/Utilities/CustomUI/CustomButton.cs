using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UICustom/Button", 30)]
public class CustomButton : Button
{
    [SerializeField] private bool _alfaCollider = false;
    [SerializeField, Range(0.01f,1f)] private float _threshold = 0.1f;
    [SerializeField] private Graphic[] _targetGraphics;

    public Graphic[] TargetGraphics => _targetGraphics;

    protected override void Start()
    {
        base.Start();

        if (_targetGraphics.Length > 0)
            targetGraphic = _targetGraphics[0];

        AlfaCollider();
    }

    protected void AlfaCollider()
    {
        Image image = targetGraphic as Image;
        if (image != null && image.sprite != null && image.sprite.texture.isReadable)
            image.alphaHitTestMinimumThreshold = !_alfaCollider ? _threshold : 0f;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if(transition != Transition.ColorTint)
        {
            base.DoStateTransition(state, instant);
            return;
        }
               
                
        if (!gameObject.activeInHierarchy || _targetGraphics == null)
            return;

        Color targetColor = state switch
        {
            SelectionState.Normal => colors.normalColor,
            SelectionState.Highlighted => colors.highlightedColor,
            SelectionState.Selected => colors.selectedColor,
            SelectionState.Pressed => colors.pressedColor,
            _ => colors.disabledColor,
        };

        foreach (Graphic graphic in _targetGraphics)
            StartColorTween(graphic);

        #region Local: StartColorTween(...)
        //=================================
        void StartColorTween(Graphic targetGraphic)
        {
            if (targetGraphic == null)
                return;

            targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }
        #endregion
    }
}
