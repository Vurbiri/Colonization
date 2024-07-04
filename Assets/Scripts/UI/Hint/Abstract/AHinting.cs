using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public abstract class AHinting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, FindObject] private HintGlobal _hint;
    [SerializeField] protected TextFiles _file;

    public bool IsShowingHint => _isShowingHint;

    private bool _isShowingHint = false;
    protected Selectable _thisSelectable;
    protected string _text;
    protected Localization _localization;

    protected virtual void Start()
    {
        _thisSelectable = GetComponent<Selectable>();
        _localization = Localization.Instance;

        SetText();
        _localization.EventSwitchLanguage += SetText;
    }

    protected abstract void SetText();

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_isShowingHint || _hint == null || !_thisSelectable.interactable)
            return;

        _isShowingHint = _hint.Show(_text);
    }
    public void OnPointerExit(PointerEventData eventData) => HideHint();

    protected void OnDisable() => HideHint();

    private void HideHint()
    {
        if (_hint != null && _isShowingHint)
        {
            _hint.Hide();
            _isShowingHint = false;
        }
    }

    private void OnDestroy()
    {
        if (_localization != null)
            _localization.EventSwitchLanguage -= SetText;
    }
}