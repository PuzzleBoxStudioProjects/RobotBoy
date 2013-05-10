using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour
{
    private PlayerPhysics playerPhysics;
    //private PlayerAnimation playerAnimation;

    private bool hasControl;

    public void RemoveControl() { hasControl = false; }

    void Awake()
    {
        //find this script on this object
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    // Use this for initialization
    void Start()
    {
        hasControl = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasControl)
        {
            if (Input.GetButton("Jump"))
            {
                //playerPhysics.Jump();
            }
            //playerPhysics.GetMotion(Input.GetAxisRaw("Horizontal"));
        }
    }
}
