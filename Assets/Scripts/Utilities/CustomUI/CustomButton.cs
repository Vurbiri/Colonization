using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UICustom/Button", 30)]
public class CustomButton : Button
{
    [SerializeField] private GameObject _interactableIcon;
    [SerializeField] private bool _alfaCollider = false;
    [SerializeField, Range(0.01f,1f)] private float _threshold = 0.1f;
    [SerializeField] private Graphic[] _targetGraphics;

    public Graphic[] TargetGraphics => _targetGraphics;

    public new bool interactable 
    { 
        get => base.interactable; 
        set 
        { 
            base.interactable = value; 
            if(_interactableIcon != null)
                _interactableIcon.SetActive(!value);
        } 
    }

    protected override void Start()
    {
        base.Start();

        if (_targetGraphics.Length > 0)
            targetGraphic = _targetGraphics[0];

        Image img = image;
        if (img != null && img.sprite != null && img.sprite.texture.isReadable)
            img.alphaHitTestMinimumThreshold = !_alfaCollider ? _threshold : 0f;
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
