using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float gravity = -9.81f;
    private float turnSmoothVelocity;
    [SerializeField] private Vector3 playerVelocity;
    private bool isGrounded;
    private Vector3 moveDir;
    
    [Header("Player Movement")]
    [SerializeField] public float speed = 6f;
    [SerializeField] public float jumpHeight = 1.0f;

    [Serializable]
    public struct KeyInputs
    {
        public float horizontal; //horizontal movement value
        public float vertical; //vertical movement value
    }
    public KeyInputs keyInputs;

    private bool setup;

    public void Enable() 
    {
        setup = true;
        controller.enabled = true;
    }

    private void Start() {
        setup = false;
    }

    private void FixedUpdate()
    {
        if (!setup)
            return;
        
        isGrounded = IsGrounded();
    }

    // Update is called once per frame
    void Update() 
    {
        if (!setup)
            return;
                
        Direction();
        Jump();
        Movement();
        
        float oldGravity = moveDir.y;

        Direction();

        moveDir = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * moveDir;

        float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, turnSmoothTime);
        transform.eulerAngles = new Vector3(0f, smoothAngle, 0f);

        moveDir.x *= speed;
        moveDir.y = oldGravity;
        moveDir.z *= speed;
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }

    private void Movement()
    {
        Vector3 direction = new Vector3(keyInputs.horizontal, 0f, keyInputs.vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                                                                        + cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                                            ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float movementSpeed = speed;

            controller.Move(moveDir * movementSpeed * Time.deltaTime);
        }
    }

    private void Direction()
    {
        float horizontal = 0; //reset movement values
        float vertical = 0;

        if (Input.GetKey(KeyCode.W)) //Forward
        {
            vertical++;
        }
        if (Input.GetKey(KeyCode.S)) //Backwards
        {
            vertical--;
        }
        if (Input.GetKey(KeyCode.D)) //Right
        {
            horizontal++;
        }
        if (Input.GetKey(KeyCode.A)) //Left
        {
            horizontal--;
        }

        moveDir = new Vector3(horizontal, 0f, vertical).normalized; 
    }

    void Jump()
    {
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, controller.radius, -Vector3.up, out hit, controller.bounds.extents.y + 0.1f - controller.bounds.extents.x, layerMask))
        {
            return true;
        }
        return false;
    }
}
