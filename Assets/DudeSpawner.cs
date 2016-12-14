using UnityEngine;
using System.Collections;

public class DudeSpawner : MonoBehaviour {

    public GameObject objectToSpawn;
    public float spawnTime;

    public float timer;

	// Use this for initialization
	void Start () {
        StartCoroutine("Spawn");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Spawn()
    {
        while (GameManager.numOfPairs > 0)
        {
            if (timer < spawnTime)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                Instantiate(objectToSpawn, transform.position, transform.rotation);
                timer = 0;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
