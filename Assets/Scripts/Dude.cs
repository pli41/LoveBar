using UnityEngine;
using System.Collections;
using System;
using RAIN.Core;

public class Dude : MonoBehaviour, Damagable{

    public float health;
    public mainCharacter mainC;
    public GameObject bloodParticle;
    public float thrust = 5f;
    private Rigidbody rb;
    [SerializeField]
    float deadBodyRemainTime;
    public AIRig aiRig;
    public bool isDead;

    public Animator anim;
    public bool isHugging;

    public float shoutMin;
    public float shoutMax;

    public AudioClip[] shoutClips;
    public AudioClip[] punchClips;
    public AudioSource aus;


    public void ReceiveDamage(float damage)
    {
        if (!isDead)
        {
            anim.SetTrigger("Hit");
            StartCoroutine("GetHit");

        }



        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        
    }

    public void Death()
    {
        if (!isDead)
        {
            if (gameObject.name.Contains("DatingDude"))
            {
                GameManager.DestroyAPair();
            }
            isDead = true;
        }
            
        if (isHugging)
        {
            isHugging = false;
            mainC.ExitCaughtByDude();
            transform.parent = null;
        }
        Destroy(GetComponent<Collider>());
        rb.AddForce(mainC.transform.forward * thrust, ForceMode.Impulse);
        anim.SetBool("Death", true);
        transform.Find("AI").gameObject.SetActive(false);
        Destroy(gameObject, deadBodyRemainTime);
    }

    // Use this for initialization
    public void Start () {
        if(gameObject.name == "Dude")
            StartCoroutine("Chasing");
        aus = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        aiRig = GetComponentInChildren<AIRig>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!mainC)
            mainC = GameManager.player;

        

        if (mainC.isAttack && other.CompareTag("Punch"))
        {
            ReceiveDamage(mainC.punchDmg);

            GameManager.audioPlay.PlayRandomSound(aus, punchClips);

            GameObject bloodP = Instantiate(bloodParticle, transform.position, Quaternion.identity) as GameObject;
            Destroy(bloodP, 10f);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void Hug(GameObject gameObject)
    {
        Debug.Log("Hug " + gameObject);
        mainCharacter mc = gameObject.GetComponent<mainCharacter>();
        if (!isHugging && mainCharacter.state != mainCharacter.MainCharacterGameState.CaughtByDude)
        {
            isHugging = true;
            anim.SetBool("Hug", true);
            //transform.SetParent(gameObject.transform);
            StartCoroutine("Hugging");
            gameObject.GetComponent<mainCharacter>().enterCaughByDude();
        }
    }

    public IEnumerator GetHit()
    {
        //Debug.Log("123123");
        anim.SetTrigger("Hit");
        aiRig.AI.WorkingMemory.SetItem<bool>("dating", true);
        yield return new WaitForSeconds(2f);
        aiRig.AI.WorkingMemory.SetItem<bool>("dating", false);

    }

    public IEnumerator Hugging()
    {
        if (!mainC)
            mainC = GameManager.player;
        Vector3 relativeToPlayer = mainC.transform.position - transform.position;
        while (isHugging)
        {
            transform.position = mainC.transform.position - relativeToPlayer;
            yield return null;
        }
    }

    public IEnumerator Chasing()
    {
        while (!isHugging && !isDead)
        {
            float randomTime = UnityEngine.Random.Range(shoutMin, shoutMax);
            GameManager.audioPlay.PlayRandomSound(aus, shoutClips);


            yield return new WaitForSeconds(randomTime);
        }
    }
}
