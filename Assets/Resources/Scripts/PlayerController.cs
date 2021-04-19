using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private WeaponBehavior weaponBehavior;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private float footstepDelay;

    [SerializeField, Header("SFX"), EventRef]
    private string damageSFX;

    [SerializeField, EventRef] private string defeatSFX, stepSFX;

    private float _footstepTimer;

    public Weapon Weapon
    {
        get => weaponBehavior.Weapon;
        set => weaponBehavior.Weapon = value;
    }

    public bool IsRotating => _isRotating;
    public bool IsMoving => _characterController.velocity != Vector3.zero;
    public Vector3 Velocity => _characterController.velocity;
    public float RotationSpeed => Mathf.Abs(_rotationValue);

    public int LocalIndex => _playerInput.playerIndex;

    private int _health;

    private GameObject _soundtrack;

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

            hudManager.RefreshHealthBar(_health);
        }
    }

    public Color Color => _bodyManager.color;

    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private InputActionManager _inputActionManager;
    private SingleplayerCameraController _cameraController;
    private Vector2 _moveDir;
    private float _rotationValue;
    private bool _lockedMovement;
    private bool _isRotating;
    private SpawnManager _spawnManager;
    private PlayerBodyManager _bodyManager;

    void Start()
    {
        _footstepTimer = 0;

        _soundtrack = GameObject.FindWithTag("Soundtrack");

        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActionManager = new InputActionManager(_playerInput);

        _cameraController = _playerInput.camera.GetComponent<SingleplayerCameraController>();

        weaponBehavior
            .Mesh
            .GetComponent<Renderer>().material.SetTexture("_MainTex",
                null);
        weaponBehavior.hudManager = hudManager;

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

        _inputActionManager.RegisterActionEvent(
            "SwitchMusic",
            InputActionManager.EventType.PERFORMED,
            ctx => OnSwitchMusic()
        );

        _inputActionManager.RegisterActionEvent(
            "SwitchPlayerJoinActive",
            InputActionManager.EventType.PERFORMED,
            ctx => OnSwitchPlayerJoinActive()
        );

        Heath = 10;
        _lockedMovement = false;
    }

    private void OnSwitchPlayerJoinActive()
    {
        var im = GameObject.FindWithTag("InputManager");
        if (im == null)
            return;

        var inputManager = im.GetComponent<PlayerInputManager>();
        if (inputManager == null)
            return;

        if (inputManager.joiningEnabled)
        {
            inputManager.DisableJoining();
            RuntimeManager.PlayOneShot("event:/SFX/UI/DisablePlayerJoin");
        }
        else
        {
            inputManager.EnableJoining();
            RuntimeManager.PlayOneShot("event:/SFX/UI/EnablePlayerJoin");
        }
    }

    private void OnSwitchMusic()
    {
        if (_soundtrack != null)
            _soundtrack.SetActive(!_soundtrack.activeSelf);
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

        if (IsMoving || IsRotating)
        {
            if (_footstepTimer > 0)
            {
                _footstepTimer -= Time.deltaTime;
            }
            else if (_footstepTimer <= 0)
            {
                var playerVelocity = Velocity.magnitude;
                if (playerVelocity < RotationSpeed)
                    playerVelocity = RotationSpeed;

                _footstepTimer = 1.2f - playerVelocity;

                RuntimeManager.PlayOneShot(stepSFX, transform.position);
            }
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
        RuntimeManager.PlayOneShot(damageSFX, transform.position);
    }

    public void OnDie()
    {
        RuntimeManager.PlayOneShot(defeatSFX, transform.position);

        var players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length <= 2)
        {
            foreach (var o in players)
            {
                var player = o.GetComponent<PlayerController>();
                if (player == null || player.LocalIndex == LocalIndex)
                    break;

                player.OnWin();
            }
        }

        Destroy(transform.parent.gameObject);
    }

    public void OnWin()
    {
        hudManager.SetWinScreen();
    }

    private void OnDestroy()
    {
        _inputActionManager.DisposeAllEvents();
    }
}