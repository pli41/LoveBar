using UnityEngine;
using System.Collections;

public class Soap : MonoBehaviour {

    public float minDist = 10f;
    private float dist;
    private bool isPickable;

    private Animator anime;

    public Transform player;
	// Use this for initialization
	void Start () {

        anime = GetComponent<Animator>();

        isPickable = false;

	}
	
	// Update is called once per frame
	void Update () {

        dist = Vector3.Distance(transform.position, player.position);

        if(dist <= minDist)
        {

            isPickable = true;
            anime.SetTrigger("highlight");
        }
	}

    void OnTriggerStay(Collider other)
    {
        if(isPickable && other.CompareTag("Grabber"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.GetComponent<mainCharacter>().PickUp(transform);
                isPickable = false;
            }
        }
    }
}
