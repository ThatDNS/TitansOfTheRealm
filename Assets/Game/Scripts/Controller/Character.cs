using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Character : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 7.0f;

    public LayerMask groundLayer;
    public Transform groundCheckPoint;
    public float groundCheckDistance = 0.2f;
    public bool isGrounded;

    private PlayerInputActions playerInputActions;
    private Rigidbody rb;
    private Vector2 moveInput;


    public Canvas MainCanvas;
    public enum CharacterTypes { Warrior, Titan}
    [Tooltip("Is the Warrior or Titan ?")]
    public CharacterTypes CharacterType = CharacterTypes.Warrior;
    private CharacterHandleWeapon handleWeapon;

    #region Monobehaviour
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        handleWeapon = GetComponent<CharacterHandleWeapon>();

        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInputActions.Player.Jump.performed += ctx => Jump();
        playerInputActions.Player.Shoot.performed += ctx => Shoot();


    }
    public PlayerInputActions GetInput()
    {
        return playerInputActions;
    }


    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        Vector3 movement = new Vector3(moveInput.x, 0.0f, moveInput.y) * speed;
        rb.MovePosition(rb.position + transform.TransformDirection(movement) * Time.fixedDeltaTime);
    }
    #endregion
    private bool IsGrounded()
    {
        // Cast a ray downwards from the groundCheckPoint
        RaycastHit hit;
        if (Physics.Raycast(groundCheckPoint.position, -Vector3.up, out hit, groundCheckDistance, groundLayer))
        {
            return true; // Grounded
        }
        return false; // Not grounded
    }

    private void Jump()
    {
        // Check if the warrior is grounded before allowing them to jump
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void Shoot()
    {
        if (handleWeapon.CurrentWeapon == null) return;
        handleWeapon.ShootStart();
    }


}
