using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject monkeyModel;

    [SerializeField] private KeyCode up = KeyCode.W;
    [SerializeField] private KeyCode down = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;

    [SerializeField] private float movementSpeed;
    private Vector3 velocity;

    

    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        MovementInput();

    }

    private void MovementInput()
    {
        velocity = Vector3.zero;

        if (Input.GetKey(up)) { velocity.z += movementSpeed; }
        if (Input.GetKey(down)) { velocity.z -= movementSpeed; }
        if (Input.GetKey(left)) { velocity.x -= movementSpeed; }
        if (Input.GetKey(right)) { velocity.x += movementSpeed; }

        monkeyModel.transform.rotation = Quaternion.LookRotation(velocity + new Vector3(0, 90, 0)) ;


        animator.SetFloat("MovSpeed", velocity.magnitude);

        rb.velocity = velocity;
    }
}
