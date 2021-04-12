using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private InputActionManager _inputActionManager;
    private Vector2 _moveDir;
    private float _rotationValue;
    private bool _lockedMovement;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActionManager = new InputActionManager(_playerInput);

        _inputActionManager.RegisterActionEvent(
            "Move",
            InputActionManager.EventType.PERFORMED,
            ctx => _moveDir = ctx.ReadValue<Vector2>()
        );

        _inputActionManager.RegisterActionEvent(
            "Rotate",
            InputActionManager.EventType.PERFORMED,
            ctx => _rotationValue = ctx.ReadValue<float>()
        );

        _inputActionManager.RegisterActionEvent(
            "Fire",
            InputActionManager.EventType.PERFORMED,
            ctx => OnFire()
        );

        _lockedMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lockedMovement)
            return;

        if (_moveDir != Vector2.zero)
            _characterController.Move((transform.right * _moveDir.x + transform.forward * _moveDir.y) * Time.deltaTime);
        
        if (_rotationValue != 0)
            transform.Rotate(0, _rotationValue * 90 * Time.deltaTime, 0);
    }

    void OnFire()
    {
    }

    private void OnDestroy()
    {
        _inputActionManager.DisposeAllEvents();
    }
}