using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fireRate;

    [SerializeField] private GameObject weapon;

    
    public bool IsRotating => _isRotating;
    public bool IsMoving => _characterController.velocity != Vector3.zero;

    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private InputActionManager _inputActionManager;
    private Vector2 _moveDir;
    private float _rotationValue;
    private bool _lockedMovement;
    private bool _isRotating;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActionManager = new InputActionManager(_playerInput);


        Camera playerCamera = Instantiate(_playerInput.camera);
        playerCamera.GetComponent<SingleplayerCameraController>().player = gameObject;


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
            _characterController.SimpleMove((transform.right * _moveDir.x + transform.forward * _moveDir.y));

        if (_rotationValue != 0)
        {
            _isRotating = true;
            transform.Rotate(0, _rotationValue * 90 * Time.deltaTime, 0);
        }
        else
        {
            _isRotating = false;
        }
    }

    void OnFire()
    {
        var bulletInstance = Instantiate(weapon, transform.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody>().velocity =
            transform.forward * fireRate + _characterController.velocity;
        bulletInstance.transform.rotation = transform.rotation;
        bulletInstance.GetComponent<Bullet>().owner = this;
        
        Physics.IgnoreCollision(GetComponent<Collider>(), bulletInstance.GetComponent<Collider>(), true);
    }

    public void OnDamage(Damage damage)
    {
        Debug.Log($"Damage: {damage.damage} | By: {damage.owner}");
    }

    private void OnDestroy()
    {
        _inputActionManager.DisposeAllEvents();
    }
}