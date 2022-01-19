using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    private PlayerController playerController;
    private Rigidbody rb;

    // references
    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
    }
    public void OnJump(InputValue value)
    {
        playerController.isJumping = value.isPressed;
        rb.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping)
        {
            return;
        }
        playerController.isJumping = false;
    }
}
