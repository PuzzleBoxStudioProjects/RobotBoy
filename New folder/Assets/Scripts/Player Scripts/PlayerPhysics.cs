using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
    public float moveSpeed         = 10.0f;
    public float jumpForce         = 10.0f;

    public float wallCheckDistance = 5.0f;
    public float rotateSpeed       = 10.0f;
    public float deltaGround       = 0.3f;
    
    public float gravity          = 40.0f;
    public float terminalVelocity = 38.0f;

    public bool isGrounded   = false;
    public bool isRotating   = false;
    public bool isFallingOff = false;

    public int rotationDirection = 1;

    public int faceDir = 1;
    //public int tempRotCount = 0;

    private float characterHeight;  //height of bounding box collider
    private float characterWidth;  //width of bounding box collider
    //public float verticalVelocity;
    //public float distGround;
    
    //private Vector3 moveVector = Vector3.zero;

    //private Vector3 myNormal;
    public Vector3 myForward;
    //private Vector3 myTurnForward;
    public Vector3 surfaceNormal;

    private PlayerMotor playerMotor;

    //getters and setters
    public bool IsGrounded() { return isGrounded; }
    //public int FaceDirection() { return faceDir; }

    public bool IsDead { get; set; }

    void Awake()
    {
        //get the script on this object
        playerMotor = GetComponent<PlayerMotor>();
    }

    void Start()
    {
        surfaceNormal = transform.up;
        CalculateBounds();
    }

    void CalculateBounds()
    {
        //distGround = collider.bounds.extents.y - collider.bounds.center.y;
        characterHeight = collider.bounds.size.y;
        characterWidth = collider.bounds.size.z;
    }

    void Update()
    {
        //if (isRotating)
        //{
        //    return;
        //}
        //GetMotionInput();

        //take control from player once dead
        if (IsDead)
        {
            playerMotor.RemoveControl();
        }

        //ApplyGravity();
        //DetectWall();
        //RotateToSurface();
    }

    void FixedUpdate()
    {
        NewPhysics();
        //ApplyGravity();
    }

    void NewPhysics()
    {
        float dir = Input.GetAxisRaw("Horizontal");

        if (dir != 0)
        {
            faceDir = (int)dir;
        }

        Ray ray;
        RaycastHit hitInfo;

        ray = new Ray(transform.position, transform.forward * faceDir);

        if (Physics.Raycast(transform.position, -transform.up, deltaGround))
        {
            isGrounded = true;
            //isRotating = false;

            if (Input.GetButtonDown("Jump"))
            {
                rigidbody.velocity += jumpForce * surfaceNormal;
            }
            //surfaceNormal = hitInfo.normal;
        }
        else
        {
            isGrounded = false;
        }

        //if (Input.GetButtonDown("Jump"))
        //{
            //ray = new Ray(transform.position, transform.right);

            if (Physics.Raycast(ray, out hitInfo, wallCheckDistance))
            {
                surfaceNormal = hitInfo.normal;

                isRotating = true;
            }
            else
            {
                isRotating = false;
                //rigidbody.velocity += jumpForce * myForward;
            }
        //}

        float halfPlayerWidth = characterWidth * 0.5f;

        Vector3 originLeft = transform.position + transform.forward * 0.5f;
        Vector3 originRight = transform.position - transform.forward * 0.5f;
        
        myForward = Vector3.Cross(transform.right, surfaceNormal);

        if (isGrounded)
        {
            if (!Physics.Raycast(originLeft, -transform.up, deltaGround))
            {
                rotationDirection = 1;
            }
            else if (!Physics.Raycast(originRight, -transform.up, deltaGround))
            {
                rotationDirection = -1;
            }
            else
            {
                rotationDirection = 0;
            }

            Vector3 originalPos = transform.position;
            Vector3 newPos = transform.TransformPoint(Vector3.down);

            if (rotationDirection != 0)
            {
                transform.position = Vector3.Lerp(originalPos, newPos, rotateSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward * rotationDirection), rotateSpeed);
            }
            //Quaternion newTargetRot = Quaternion.LookRotation(myForward, -surfaceNormal);
            //transform.rotation = Quaternion.Lerp(transform.rotation, newTargetRot, rotateSpeed * rotationDirection);
        }
  
        if (isRotating)
        {
            Quaternion targetRot = Quaternion.LookRotation(myForward, surfaceNormal);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed);
        }

        //rigidbody.AddForce(-gravity * surfaceNormal);
        rigidbody.AddRelativeForce(gravity * Vector3.down);

        transform.Translate(0, 0, dir * moveSpeed * Time.deltaTime);

        //print(isRotating);

        Debug.DrawRay(originLeft, -transform.up * deltaGround, Color.green);
        Debug.DrawRay(originRight, -transform.up * deltaGround, Color.black);

        Debug.DrawRay(transform.position, (transform.forward * faceDir) * wallCheckDistance, Color.blue);
    }

    Vector3 GetBottomCenter()
    {
        return collider.bounds.center + collider.bounds.extents.y * -transform.up;
    }

    #region Old Code
    //void LateUpdate()
    //{
    //    //UpdateGroundInfo();
    //}

    //void GetMotionInput()
    //{
    //    if (isRotating)
    //    {
    //        return;
    //    }

    //    //myNormal = Vector3.Lerp(myNormal, surfaceNormal, rotateSpeed * Time.deltaTime);
    //    //myForward = Vector3.Cross(transform.right, surfaceNormal);

    //    //Quaternion targetRot = Quaternion.LookRotation(myForward, surfaceNormal);
    //    //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed);

    //    //transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);

    //    Ray ray;
    //    RaycastHit hitInfo;

    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        Ray rayOrigin = new Ray(transform.position, transform.right);

    //        if (Physics.Raycast(rayOrigin, out hitInfo, wallCheckDistance))
    //        {
    //            isRotating = true;

    //        }
    //        else if (isGrounded)
    //        {
    //            rigidbody.velocity += jumpForce * myNormal;
    //            //verticalVelocity = jumpForce;
    //        }
    //    }

    //    //JumpToWall();
    //    //JumpToWall(hitInfo.point, hitInfo.normal);
    //    transform.Rotate(0, Input.GetAxisRaw("Horizontal") * rotateSpeed * Time.deltaTime, 0);

    //    ray = new Ray(transform.position, -myNormal);

    //    //if (Physics.Raycast(ray, out hitInfo))
    //    if (Physics.Raycast(ray, out hitInfo, deltaGround))
    //    {
    //        //isGrounded = hitInfo.distance <= distGround + deltaGround;
    //        surfaceNormal = hitInfo.normal;

    //        //verticalVelocity = 0;  //stop applying gravity
    //        isGrounded = true;
    //        //isRotating = false;

    //        if (tempRotCount == 2)
    //        {
    //            isRotating = false;
    //            tempRotCount = 0;
    //        }
    //        //rigidbody.isKinematic = false;
    //    }
    //    else
    //    {
    //        isGrounded = false;

    //        surfaceNormal = Vector3.up;
    //    }

    //    float lerpSpeed = 10.0f;

    //    myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
    //    myForward = Vector3.Cross(transform.right, myNormal);

    //    Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
    //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);

    //    moveVector = new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, 0, 0) * Time.deltaTime;
    //    transform.Translate(moveVector);
    //}

    //public void GetMotion(float dir)
    //{
    //    if (isRotating)
    //    {
    //        return;
    //    }
        
    //    //if pressing a directional key, turn
    //    if (dir != 0)
    //    {
    //        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y * dir, transform.eulerAngles.z);
    //        //transform.rotation = Quaternion.LookRotation(Vector3.forward * dir);
    //        faceDir = (int)dir;
    //    }

    //    //myNormal = Vector3.Lerp(myNormal, surfaceNormal, rotateSpeed * Time.deltaTime);
    //    //myForward = Vector3.Cross(transform.right, myNormal);

    //    //Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
    //    //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        
    //    moveVector = new Vector3(dir * moveSpeed, rigidbody.velocity.y, 0) * Time.deltaTime;
    //    transform.Translate(moveVector);

    //    //moveVector = transform.TransformDirection(transform.right);

    //    //move the character
    //    //rigidbody.AddRelativeForce(Vector3.right * dir * moveSpeed, ForceMode.Impulse);
    //    //moveVector = new Vector3(moveSpeed * dir, rigidbody.velocity.y, 0);
    //    //rigidbody.AddRelativeForce(moveVector * Time.deltaTime, ForceMode.Impulse);
    //    //rigidbody.velocity = moveVector;
    //}

    //public void Jump()
    //{
    //    Vector3 vel = rigidbody.velocity;

    //    Ray ray;
    //    RaycastHit hitInfo;

    //    ray = new Ray(transform.position, transform.right);

    //    if (Physics.Raycast(transform.position, transform.right * faceDir, out hitInfo, wallCheckDistance) && !isRotating)
    //    {
    //        //JumpToWall(hitInfo.point, hitInfo.normal);
    //        isRotating = true;
    //    }
    //    else if (isGrounded)
    //    {
    //        //vel.y = jumpForce;
    //        //rigidbody.velocity = vel;
    //        //rigidbody.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    //        //rigidbody.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    //        //rigidbody.velocity += jumpForce * myNormal;
    //        verticalVelocity = jumpForce;
    //    }
    //}

    //void JumpToWall(Vector3 point, Vector3 normal)
    //void JumpToWall()
    //{
    //    isRotating = true;
    //    //rigidbody.isKinematic = true;
    //    tempRotCount = 1;

    //    //Vector3 originalPos = transform.position;
    //    Quaternion originalRot = transform.rotation;

    //    //Vector3 distPos = point + normal * (distGround + 0.5f);

    //    myForward = Vector3.Cross(transform.right, normal);
    //    Quaternion distRot = Quaternion.LookRotation(myForward, normal);

    //    if (isRotating)
    //    {
    //        transform.rotation = Quaternion.RotateTowards(originalRot, distRot, rotateSpeed);
        
    //        if (!isGrounded)
    //        {
    //            tempRotCount = 2;
    //        }
    //    }
    //    //for (float t = 0.0f; t < 1.0f; )
    //    //{
    //    //    t += Time.deltaTime;
    //    //    transform.rotation = Quaternion.RotateTowards(originalRot, distRot, t);
    //    //    //transform.position = Vector3.Lerp(originalPos, distPos, rotateSpeed);

    //    //    //yield return new WaitForEndOfFrame();
    //    //}

    //    myNormal = normal;

       
    //    //rigidbody.isKinematic = false;
    //    //isRotating = false;
    //}
    #endregion

    void ApplyGravity()
    {
        //apply gravity as long as not grounded
        if (!isGrounded && rigidbody.velocity.y > -terminalVelocity)
        {
            rigidbody.AddRelativeForce(Vector3.down * gravity);
            //rigidbody.AddForce(-gravity * rigidbody.mass * myNormal);
            //verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //check if the character hit something above
        //if so stop the upwards force on the jump
        ContactPoint contact = col.contacts[0];
        Vector3 vel = rigidbody.velocity;

        float checkAbove = Vector3.Angle(contact.normal, -transform.up);

        if (checkAbove < 60)
        {
            vel.y = 0;
            rigidbody.velocity = vel;
            //verticalVelocity = 0;
        }
    }

    //void UpdateGroundInfo()
    //{
    //    //detect ground
    //    //float dist = 0.5f;
    //    //Vector3 dir = transform.up;

    //    Ray ray;

    //    RaycastHit hitInfo;

    //    ray = new Ray(transform.position, -transform.up);

    //    //if (Physics.Raycast(ray, out hitInfo, distGround + deltaGround))
    //    if (Physics.Raycast(ray, out hitInfo, deltaGround))
    //    {
    //        //isGrounded = hitInfo.distance <= distGround + deltaGround;
    //        surfaceNormal = hitInfo.normal;

    //        verticalVelocity = 0;  //stop applying gravity
    //        isGrounded = true;

    //        if (tempRotCount == 2)
    //        {
    //            isRotating = false;
    //            tempRotCount = 0;
                
    //        }
    //    }
    //    else
    //    {
    //        isGrounded = false;

    //        //surfaceNormal = Vector3.up;
    //    }
    //}

    //void DetectWall()
    //{
    //    RaycastHit hitInfo;

    //    float dist = 0.5f;
    //    //float radiusCheck = collider.bounds.size.y;

    //    if (Physics.SphereCast(transform.position + transform.up * 1.5f, dist, transform.right * faceDir, out hitInfo, dist) && !isRotating)
    //    //if (Physics.Raycast(transform.position, transform.right * faceDir, out hitInfo, wallCheckDistance) && !isRotating)
    //    {
    //        rotationDirection += 90;  //////////FIND A DIFFERENT WAY TO DO THE INCREMENTING METHOD
    //        tempRotCount = 1;

    //        //moveVector = hitInfo.normal
    //        isRotating = true;

    //        //moveVector.x = 0;
    //    }
        
    //    Debug.DrawRay(transform.position, transform.right * wallCheckDistance * faceDir, Color.blue);

    //    //rotationDirection = Mathf.Clamp(rotationDirection, 0, 360);
        
    //    if (isRotating)
    //    {
    //        //Quaternion localRot = Quaternion.FromToRotation(-transform.up, transform.right) * transform.rotation;

    //        //transform.rotation = Quaternion.RotateTowards(transform.rotation, localRot, rotateSpeed * faceDir);
    //        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward), rotateSpeed);

    //        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(rotationDirection * faceDir, transform.forward), rotateSpeed);

    //        transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, rotationDirection * faceDir, rotateSpeed));
    //        //transform.rotation = Quaternion.AngleAxis(rotationDirection * faceDir, transform.forward);

    //        if (!isGrounded)
    //        {
    //            tempRotCount = 2;
    //        }
    //        //isRotating = false;
    //    }
        
    //}

    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.transform.tag == "FallOff")
    //    {
    //        Vector3 originalPos = transform.position;
    //        Vector3 newPos = transform.TransformPoint(Vector3.down * 2);

    //        transform.position = Vector3.Lerp(originalPos, newPos, rotateSpeed);

    //        Quaternion targetRot = Quaternion.LookRotation(myForward, -surfaceNormal);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed);
    //    }
    //}
}