using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool ShootPressed;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => JumpPressed = true;
        inputActions.Player.Shoot.performed += ctx => ShootPressed = true;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void LateUpdate()
    {
        // Reset jump flag after it's been read
        JumpPressed = false;
    }
}
