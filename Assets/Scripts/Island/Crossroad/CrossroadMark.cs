using UnityEngine;

public class CrossroadMark : MonoBehaviour
{
    [SerializeField] private GameObject _graphicObject;

    public void Setup(CrossroadType type)
    {
        gameObject.SetActive(true);

        if(type == CrossroadType.Up)
            transform.rotation *= Quaternion.Euler(0, 180, 0);
        else
            transform.rotation *= Quaternion.identity;
    }

    public void SetActive(bool active) => gameObject.SetActive(active);
}
