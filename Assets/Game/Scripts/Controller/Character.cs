using Fusion.Addons.ConnectionManagerAddon;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Character : MonoBehaviour,IPlayerVisitor
{
    public float speed = 5.0f;
    public float jumpForce = 7.0f;

    public bool isGrounded;
    private CharacterController controller;
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;
    private Vector2 moveInput;
    [SerializeField] private Health HP;

    public Canvas MainCanvas;
    public enum CharacterTypes { Warrior, Titan}
    [Tooltip("Is the Warrior or Titan ?")]
    public CharacterTypes CharacterType = CharacterTypes.Warrior;
    private CharacterHandleWeapon handleWeapon;

    public ConnectionManager connectionManager;
    private Animator animator;
    #region Monobehaviour
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        HP = GetComponent<Health>();
        animator=GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
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
        if (connectionManager != null && !connectionManager.isConnected)
            return;

        isGrounded = IsGrounded();
        Vector3 movement = new Vector3(moveInput.x, 0.0f, moveInput.y);
        Vector3 direction = transform.TransformDirection(movement);

        controller.Move(direction * Time.deltaTime * speed);
        animator.SetFloat("Speed", moveInput.magnitude);

        if (direction != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 3.0f);
        }


    }
    #endregion
    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void Jump()
    {
        // Check if the warrior is grounded before allowing them to jump
        if (isGrounded && (connectionManager == null || connectionManager.isConnected))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
        {
            collectible.Accept(this);
        }
    }
    private void Shoot()
    {
        if (handleWeapon.CurrentWeapon == null) return;
        handleWeapon.ShootStart();
    }

    public void Visit(CureCollectible cure)
    {
        if (HP == null) return;
        HP.ReceiveHealth(cure.cureAmount);
        
    }
    public IPlayerVisitor GetVisitor()
    {
        return this;
    }
}
