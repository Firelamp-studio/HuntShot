using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private WeaponBehavior weaponBehavior;

    public Weapon Weapon
    {
        get => weaponBehavior.Weapon;
        set => weaponBehavior.Weapon = value;
    }

    public bool IsRotating => _isRotating;
    public bool IsMoving => _characterController.velocity != Vector3.zero;
    public Vector3 Velocity => _characterController.velocity;

    private float _health;

    public float Heath
    {
        get => _health;
        set
        {
            if (value <= 0)
            {
                _health = 0;
                OnDie();
            }

            _health = value;
        }
    }

    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private InputActionManager _inputActionManager;
    private Vector2 _moveDir;
    private float _rotationValue;
    private bool _lockedMovement;
    private bool _isRotating;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActionManager = new InputActionManager(_playerInput);

        Camera playerCamera = Instantiate(_playerInput.camera);
        playerCamera.GetComponent<SingleplayerCameraController>().player = gameObject;

        weaponBehavior
            .Mesh
            .GetComponent<Renderer>().material.SetTexture("_MainTex",
                null);
        
        Heath = 10;

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
            _characterController.SimpleMove(transform.right * _moveDir.x + transform.forward * _moveDir.y);

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
        weaponBehavior.OnShoot();
    }

    public void OnDamage(Damage damage)
    {
        Heath -= damage.damage;
    }

    public void OnDie()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _inputActionManager.DisposeAllEvents();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(150 + (Screen.width / 2 * _playerInput.playerIndex), 100, 150, 100), "H: " + _health);
    }
}