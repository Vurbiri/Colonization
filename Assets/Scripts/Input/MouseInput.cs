using NaughtyAttributes;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private LayerMask _layerLeft;
    [SerializeField] private LayerMask _layerRight;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerLeft.value) && hit.collider.TryGetComponent(out ISelectable obj))
                obj.Select();
        }
    }
}
