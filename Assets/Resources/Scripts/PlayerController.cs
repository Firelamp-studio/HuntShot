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

    public int LocalIndex => _playerInput.playerIndex;

    private int _health;

    public int Heath
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

            _hudManager.RefreshHealthBar(_health);
        }
    }

    public Color Color => _bodyManager.color;

    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private InputActionManager _inputActionManager;
    private SingleplayerCameraController _cameraController;
    private HUDManager _hudManager;
    private Vector2 _moveDir;
    private float _rotationValue;
    private bool _lockedMovement;
    private bool _isRotating;
    private SpawnManager _spawnManager;
    private PlayerBodyManager _bodyManager;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActionManager = new InputActionManager(_playerInput);

        Camera playerCamera = Instantiate(_playerInput.camera);
        _cameraController = playerCamera.GetComponent<SingleplayerCameraController>();
        _cameraController.player = gameObject;

        _hudManager = _cameraController.HUDManager;

        weaponBehavior
            .Mesh
            .GetComponent<Renderer>().material.SetTexture("_MainTex",
                null);
        weaponBehavior.hudManager = _cameraController.HUDManager;
        
        _spawnManager = GameObject.Find("Spawn:" + LocalIndex).GetComponent<SpawnManager>();
        _bodyManager = GetComponent<PlayerBodyManager>();
        _bodyManager.color = _spawnManager.PlayerColor;
        transform.position = _spawnManager.transform.position;
        transform.rotation = _spawnManager.transform.rotation;

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
            InputActionManager.EventType.STARTED,
            ctx => OnFireStart()
        );

        _inputActionManager.RegisterActionEvent(
            "Fire",
            InputActionManager.EventType.CANCELED,
            ctx => OnFireCancel()
        );

        _inputActionManager.RegisterActionEvent(
            "Reload",
            InputActionManager.EventType.PERFORMED,
            ctx => OnReload()
        );

        Heath = 10;
        _lockedMovement = false;
    }

    private void OnReload()
    {
        weaponBehavior.StartReload();
    }


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

    void OnFireStart()
    {
        weaponBehavior.OnShootStart();
    }

    void OnFireCancel()
    {
        weaponBehavior.OnShootCancel();
    }

    public void OnDamage(Damage damage)
    {
        Heath -= damage.damage;
    }

    public void OnDie()
    {
        _hudManager.SetGameOverScreen();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _inputActionManager.DisposeAllEvents();
    }
}