using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float test = 0;

    private PlayerController playerController;
    private Rigidbody rb;
    private Animator playerAnimator;
    public GameObject followTarget;

    // references
    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector2 lookInput = Vector2.zero;

    float lookUpMax = 180;
    float lookUpMin = -180;
    float lookUpClamp;

    public float aimSensitivity = 1;

    // Animator hashes
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");
    public readonly int AimVerticalHash = Animator.StringToHash("AimVertical");


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        if (!GameManager.Instance.cursorActive)
        {
            AppEvents.InvokeOnMouseCursorEnable(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       lookUpClamp = Mathf.Clamp(0.0f, lookUpMin, lookUpMax);
    }

    // Update is called once per frame
    void Update()
    {
        // Aiming/looking

        // horizontal rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);
        // vertical rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.left);

        var angles = followTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.transform.localEulerAngles.x;
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }

        lookUpClamp += lookInput.y;
        float lookParameter = Mathf.InverseLerp(lookUpMin, lookUpMax, lookUpClamp);

        playerAnimator.SetFloat(AimVerticalHash, lookParameter);

        // if we aim up, adjust animations to have a mask that will let us properly animate the aim

        followTarget.transform.localEulerAngles = angles;

        // rotate the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);

        followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0 );

        // movement
        if (playerController.isJumping) { return; }

        if (!(inputVector.magnitude > 0)) moveDirection = Vector3.zero;

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeedd = playerController.isRunning ? runSpeed : walkSpeed;

        Vector3 movementDirection = moveDirection * (currentSpeedd * Time.deltaTime);
        transform.position += movementDirection;
    }

    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        playerAnimator.SetBool(isRunningHash, playerController.isRunning);
    }
    public void OnJump(InputValue value)
    {
        if (playerController.isJumping)
        {
            return;
        }

        playerController.isJumping = value.isPressed;
        rb.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        playerAnimator.SetBool(isJumpingHash, playerController.isJumping);

    }

    public void OnAim(InputValue value)
    {
        playerController.isAiming = value.isPressed;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

      
        
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping)
        {
            return;
        }
        playerController.isJumping = false;
        playerAnimator.SetBool(isJumpingHash, playerController.isJumping);

    }
}
