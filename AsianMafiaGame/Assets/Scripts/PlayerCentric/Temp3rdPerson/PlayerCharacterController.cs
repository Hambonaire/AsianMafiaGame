using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour {

    // Use this for initialization
    public Rigidbody rb;
    public float speed;
    public float velocityClamp;
    public float speedH = 2.0f;
    public float jumpForce;
    public float jumpCooldown;
    public GameObject AttackOne;
    public float attackCooldown;
    private Vector3 rbFrontPos; // the position where the attack comes out of character
    public float rbFrontYPos; //adjustable y pos for Vector3 ^

    private float moveHorizontal = 0.0f;
    private float moveVertical = 0.0f;
    private float yaw = 0.0f;
    private bool cdJumpActive;
    private bool cdAttackActive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Movement();
        Rotation();
        CheckSpeed();
        JumpCheck();
        AttackCheck();
    }

    void Movement()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0.0f;
        rb.velocity += movement * speed;
    }
    void CheckSpeed()
    {
        if (rb.velocity.magnitude > velocityClamp)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityClamp);
        }
    }

    void Rotation()
    {
        yaw += speedH * Input.GetAxis("Mouse X");


        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 200), "rigidbody velocity: " + rb.velocity);
    }

    void JumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cdJumpActive == false)
            {
                rb.AddForce(0.0f, jumpForce, 0.0f);
                Invoke("ResetJump", jumpCooldown);
                cdJumpActive = true;
            }

        }
    }
    void ResetJump()
    {
        cdJumpActive = false;
    }

    void AttackCheck()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (cdAttackActive == false)
            {

                rbFrontPos.x = rb.position.x;
                rbFrontPos.y = rb.position.y + rbFrontYPos;
                rbFrontPos.z = rb.position.z;

                Instantiate(AttackOne, rbFrontPos, rb.rotation);
                Invoke("ResetAttack", attackCooldown);
                cdAttackActive = true;
            }
        }
    }
    void ResetAttack()
    {
        cdAttackActive = false;
    }
}
