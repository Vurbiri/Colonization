using UnityEngine;
using UnityEngine.EventSystems;

public class Hinting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, FindObject] private Hint _hint;
    [SerializeField] private string _key;
    [SerializeField] private TextFiles _file;

    public Hint Hint { get => _hint; set => _hint = value; }
    public string Key { get => _key; set => _key = value; }
    public bool IsShowingHint => _isShowingHint;

    private bool _isShowingHint = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_hint == null || _isShowingHint)
            return;

        _isShowingHint = _hint.Show(_file, _key);
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
}
