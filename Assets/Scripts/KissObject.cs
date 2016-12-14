using UnityEngine;
using System.Collections;

public class KissObject : MonoBehaviour {

    public bool isSeen;


	// Use this for initialization
	void Start () {
        StartCoroutine("CheckVisible");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnBecameVisible()
    {
        Debug.Log("seeing");
        //isSeen = true;
    }

    void OnBecameInvisible()
    {
        Debug.Log("not seeing");
        //isSeen = false;
    }

    IEnumerator CheckVisible()
    {
        while (isActiveAndEnabled)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            Vector3 dir = Camera.main.transform.position - transform.position; 
            if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && !Physics.Raycast(transform.position, dir, dir.magnitude))
            {
                isSeen = true;
            }
            else
            {
                isSeen = false;
            }
            yield return null;
        }
    }
}
