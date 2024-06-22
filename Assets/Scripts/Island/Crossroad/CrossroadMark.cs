using System.Collections.Generic;
using UnityEngine;

public class CrossroadMark : MonoBehaviour
{
    [SerializeField] private GameObject _graphicObject;
    [SerializeField] private UnityDictionary<LinkType, GameObject> _billboards;

    public bool IsShow { set { _isShow = value; SetActive(); } }
    public bool IsActive { set { _isActive = value; SetActive(); } }

    private bool _isShow = true, _isActive;

    private void Awake()
    {
#if !UNITY_EDITOR
        _billboards.DeleteList();
#endif

        IsActive = false;
        foreach (var billboard in _billboards.Values)
            billboard.SetActive(false);
    }

    public void Setup(CrossroadType type, ICollection<LinkType> linkTypes)
    {
        foreach (var linkType in linkTypes)
            _billboards[linkType].SetActive(true);

        if (type == CrossroadType.Up)
            transform.rotation *= Quaternion.Euler(0, 180, 0);
        else
            transform.rotation *= Quaternion.identity;

        IsActive = true;
    }

    private void SetActive() => _graphicObject.SetActive(_isShow && _isActive);
}
