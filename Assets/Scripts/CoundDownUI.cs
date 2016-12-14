using UnityEngine;
using System.Collections;

public class CoundDownUI : MonoBehaviour {

    private Transform player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (!player)
            player = GameManager.player.transform;


        transform.LookAt(player);

        /*
        Vector3 targetDir = player.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1f, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        
        transform.rotation = Quaternion.LookRotation(newDir);
        */
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position + offset, 0.2f);
    }
}
