using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour
{
    private GameObject player;
    private PlayerPhysics playerPhysics;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerPhysics = player.GetComponent<PlayerPhysics>();
    }
    	
	void LateUpdate ()
    {
        transform.position = player.transform.position;
        //transform.rotation = Quaternion.LookRotation(player.transform.forward * playerPhysics.faceDir);
        //transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, , player.transform.eulerAngles.z);
	}
}
