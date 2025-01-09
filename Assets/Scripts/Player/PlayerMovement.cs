using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  [RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject monkeyModel;

    [SerializeField] private KeyCode up = KeyCode.W;
    [SerializeField] private KeyCode down = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;

    [SerializeField] private float movementSpeed;
    private float defaultSpeed;
    private Vector3 velocity;
    public Vector3 Velocity { get { return velocity; } }

    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private Transform cameraPlayer;

    [SerializeField] private float downwardForce = 10f; // Force applied to keep the player grounded
    private GroundCheck groundCheck;

    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        defaultSpeed = movementSpeed;
        velocity = Vector3.zero;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!groundCheck.Grounded)
            ApplyDownwardForce();

    }

    private void MovementInput()
    {
        velocity = Vector3.zero;

        if (Input.GetKey(up)) { velocity.z += 1; }
        if (Input.GetKey(down)) { velocity.z -= 1; }
        if (Input.GetKey(left)) { velocity.x -= 1; }
        if (Input.GetKey(right)) { velocity.x += 1; }

        Vector3 camFoward = cameraPlayer.forward;
        Vector3 camRight = cameraPlayer.right;

        camFoward.y = 0;
        camRight.y = 0;

        Vector3 fowardRelative = velocity.z * camFoward;
        Vector3 rightRelative = velocity.x * camRight;

        velocity = fowardRelative + rightRelative;

        if ((velocity.magnitude != 0))
            monkeyModel.transform.rotation = Quaternion.LookRotation(velocity);// + new Vector3(0, 90, 0));


        animator.SetFloat("MovSpeed", velocity.magnitude);

        float rbVelocityY = rb.linearVelocity.y;

        if (groundCheck.Grounded)
            rbVelocityY = 0f;

        rb.linearVelocity = velocity.normalized * movementSpeed;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rbVelocityY, rb.linearVelocity.z);
    }

    private void ApplyDownwardForce()
    {
        // Apply a constant downward force to the Rigidbody
        rb.AddForce(Vector3.down * downwardForce, ForceMode.Acceleration);
    }

    public void MultiplySpeed(float multiply) => movementSpeed = defaultSpeed * multiply;

    public void AddSpeed(float add) => movementSpeed = defaultSpeed + add;

    public void ResetSpeed() => movementSpeed = defaultSpeed;
}
