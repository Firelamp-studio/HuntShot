using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = default;

    private InputActionManager _inputActionManager;
    private Vector2 _moveDir;
    private float _rotationValue;

    // Start is called before the first frame update
    void Start()
    {
        _inputActionManager = new InputActionManager(playerInput);

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnFire()
    {
        
    }
    
    private void OnDestroy()
    {
        _inputActionManager.DisposeAllEvents();
    }
}