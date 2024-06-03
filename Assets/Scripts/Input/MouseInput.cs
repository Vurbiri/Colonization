using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private LayerMask _layerLeft;
    [SerializeField] private LayerMask _layerRight;
    [Space]
    [SerializeField] private float _distance = 900f;

    private Camera _camera;
    private Ray _ray;
    private RaycastHit _hit;
    private ISelectable _obj;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerLeft.value) && _hit.collider.TryGetComponent(out _obj))
                _obj.Select();
        }
    }
}
