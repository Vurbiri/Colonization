using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextLocalization : MonoBehaviour
{
    public TMP_Text Text {get; protected set;}
    protected string _key;    

    public void Setup(string key = null)
    {
        Text = GetComponent<TMP_Text>();
        _key = string.IsNullOrEmpty(key) ? Text.text : key;
        SetText();
        Localization.Instance.EventSwitchLanguage += SetText;
    }

    protected virtual void SetText() => Text.text = Localization.Instance.GetText(_key);

    private void OnDestroy()
    {
        if(Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= SetText;
    }
}
