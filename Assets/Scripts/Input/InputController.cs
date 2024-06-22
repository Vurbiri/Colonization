using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[DefaultExecutionOrder(-1)]
public class InputController : ASingleton<InputController>
{
    [Space]
    [SerializeField] private LayerMask _layerLeft;
    [SerializeField] private LayerMask _layerRight;
    [Space]
    [SerializeField] private float _distance = 900f;

    public InputControlAction.CameraActions CameraActions => _inputActions.Camera;

    private InputControlAction _inputActions;
    private Camera _camera;
    private Ray _ray;
    private RaycastHit _hit;
    private ISelectable _obj;

    protected override void Awake()
    {
        _isNotDestroying = false;
        base.Awake();

        _inputActions = new();
        _camera = Camera.main;
    }

    private void Start()
    {
        _inputActions.Enable();

        _inputActions.Gameplay.LeftClick.performed += OnLeftClick;
        _inputActions.Gameplay.RightClick.performed += OnRightClick;
    }

    public void EnableGameplayMap()
    {
        _inputActions.Gameplay.Enable();
        _inputActions.Camera.Enable();
    }

    private void OnLeftClick(CallbackContext ctx) => Click(ctx.ReadValue<Vector2>(), _layerLeft);
    private void OnRightClick(CallbackContext ctx) => Click(ctx.ReadValue<Vector2>(), _layerRight);

    private void Click(Vector2 mousePosition, LayerMask layer)
    {
        _ray = _camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(_ray, out _hit, _distance, layer.value) && _hit.collider.TryGetComponent(out _obj))
            _obj.Select();
    }

    private void OnDisable() => _inputActions?.Disable();
}

