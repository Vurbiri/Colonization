using System.Collections.Generic;
using UnityEngine;

public class CrossroadMark : MonoBehaviour
{
    [SerializeField] private GameObject _graphicObject;
    [SerializeField] private UnityDictionary<LinkType, GameObject> _billboards;

    private void Awake()
    {
        _billboards.DeleteList();

        _graphicObject.SetActive(false);
        foreach (var billboard in _billboards.Values)
            billboard.SetActive(false);
    }

    public void Setup(CrossroadType type, ICollection<LinkType> linkTypes)
    {
        _graphicObject.SetActive(true);
        foreach (var linkType in linkTypes)
            _billboards[linkType].SetActive(true);

        if (type == CrossroadType.Up)
            transform.rotation *= Quaternion.Euler(0, 180, 0);
        else
            transform.rotation *= Quaternion.identity;
    }

    public void SetActive(bool active) => _graphicObject.SetActive(active);
}
