using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpForce = 10.0f;

    public float gravity = 40.0f;
    public float terminalVelocity = 38.0f;

    private bool isGrounded      = false;
    private bool isOnWall        = false;

    //private int faceDir = 1;

    //private float characterHeight;  //height of bounding box collider
    //private float characterWidth;  //width of bounding box collider
    public float verticalVelocity;

    private Vector3 moveVector = Vector3.zero;

    private PlayerMotor playerMotor;

    //getters and setters
    public bool IsGrounded() { return isGrounded; }

    public bool IsDead { get; set; }

    void Awake()
    {
        //get the script on this object
        playerMotor = GetComponent<PlayerMotor>();
    }

    void Start()
    {
        //CalculateBounds();
    }

    //void CalculateBounds()
    //{
    //    characterHeight = collider.bounds.size.y;
    //    characterWidth = collider.bounds.size.x;
    //}

    void Update()
    {
        //take control from player once dead
        if (IsDead)
        {
            playerMotor.RemoveControl();
        }

        ApplyGravity();
        DetectWall();
    }

    void LateUpdate()
    {
        UpdateGroundInfo();
    }

    public void GetMotion(float dir)
    {
        //if pressing a directional key, turn
        if (dir != 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward * dir);
        }

        //move the character
        moveVector = new Vector3(moveSpeed * dir, verticalVelocity, 0);
        rigidbody.velocity = moveVector;
    }

    public void Jump()
    {
        verticalVelocity = jumpForce;
    }

    void ApplyGravity()
    {
        //apply gravity as long as not grounded
        if (!isGrounded && verticalVelocity > -terminalVelocity)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //check if the character hit something above
        //if so stop the upwards force on the jump
        ContactPoint contact = col.contacts[0];

        float checkAbove = Vector3.Angle(contact.normal, -transform.up);

        if (checkAbove < 60)
        {
            verticalVelocity = 0;
        }
    }

    void UpdateGroundInfo()
    {
        //detect ground
        float dist = 0.5f;
        Vector3 dir = Vector3.down;

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, dir, out hitInfo, dist))
        {
            
            verticalVelocity = 0;  //stop applying gravity
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void DetectWall()
    {
        RaycastHit hitInfo;

        float dist = 0.3f;
        //float radiusCheck = collider.bounds.size.y;

        if (Physics.SphereCast(transform.position, dist, transform.right, out hitInfo, dist))
        {
            isOnWall = true;
            moveVector.x = 0;
        }
        else
        {
            isOnWall = false;
        }
    }
}
