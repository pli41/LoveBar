using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class mainCharacter : MonoBehaviour {

    public static MainCharacterGameState state;

    public AudioClip[] punchMissClips;
   // public AudioClip[] punchClips;
    public AudioSource aus;

    public float startingHealth = 50f;
    public float punchDmg = 5f;
    public float flashSpeed = 5f;
    public float coolDown = 2f;
    public float attackStart = 0f;

    public Animator rightHandAnime;
    public Animator leftHandAnime;

    public float caughtDmg = 0.1f;

    public float caughtHealthReductionDelay = 10f;

    public float currentHealth;

    public Slider healthBar;
    public Image damageImage;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    public bool isAttack = false;

    public Transform soapInstant;

    private GameObject throwSoap;

    private FirstPersonController controller;

    public bool soapPicked = false;
    public bool caughtByDude = false;
    public bool escapeDude = false;

    public Collider rightarmCollider;
    public Collider leftarmCollider;

    private Transform arm;
    private Transform rightArm;
    public Transform grabber;
    private Transform pickUp;
    private Rigidbody soapRg;

    bool damaged;
    bool isDead;
    MainCharacterGameState lastState = MainCharacterGameState.GameOver;

	void Start () {
        healthBar.value = startingHealth;
        state = MainCharacterGameState.NormalState;
        currentHealth = startingHealth;
        rightArm = transform.Find("polySurface4");
        controller = GameObject.FindObjectOfType<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.value = currentHealth;


        isAttack = false;
        rightarmCollider.enabled = false;
        leftarmCollider.enabled = false;
        //if (!soapPicked)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Debug.Log("left clicked");
        //        isAttack = true;
        //        armCollider.enabled = true;
        //    }

        //}


        ////Debug.Log(isAttack);

        //if(soapPicked && Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log("throw soap");

        //    pickUp.transform.parent = null;
        //    soapRg.isKinematic = false;
        //    soapRg.velocity = Camera.main.transform.forward * 5f;

        //    soapPicked = false;
        //}

        if (damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
           damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;

        if(state != lastState)
        {
            Debug.Log("MainCharacterGameState : " + state);
            lastState = state;
        }


        switch (state)
        {
//////////////////////////////////////////////////////////
            case MainCharacterGameState.NormalState:
                if (currentHealth <= 0f && !isDead)
                {
                    isDead = true;
                    state = MainCharacterGameState.GameOver;
                }
                else
                {
                    if (!soapPicked)
                    {
                        if (Input.GetMouseButtonDown(0) && Time.time >= attackStart + coolDown)
                        {
                            GameManager.audioPlay.PlayRandomSound(aus,punchMissClips);

                            //Debug.Log("Left clicked");
                            //Debug.Log(Time.time);
                            attackStart = Time.time;
                            isAttack = true;

                            if (Random.Range(0f, 1f) >= 0.5f)
                            {
                                rightarmCollider.enabled = true;
                                rightHandAnime.SetTrigger("handPunch");
                            }
                            else 
                            {
                                leftarmCollider.enabled = true;
                                leftHandAnime.SetTrigger("handPunch");
                            }

                            
                        }
                    }
                    if (soapPicked)
                    {
                        state = MainCharacterGameState.PickedSoap;
                    }

                    if (caughtByDude)
                    {
                        state = MainCharacterGameState.CaughtByDude;
                    }
                }

                break;
//////////////////////////////////////////////////////////////
            case MainCharacterGameState.PickedSoap:

                if (currentHealth <= 0f && !isDead)
                {
                    isDead = true;
                    state = MainCharacterGameState.GameOver;
                }
                else
                {
                    if (soapPicked)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            pickUp.transform.parent = null;
                            soapRg = pickUp.GetComponent<Rigidbody>();
                            soapRg.isKinematic = false;
                            soapRg.useGravity = true;
                            soapRg.velocity = Camera.main.transform.forward * 5f;
                            soapPicked = false;
                        }
                    }
                    if (!soapPicked)
                    {
                        state = MainCharacterGameState.NormalState;
                    }

                    if (caughtByDude)
                    {
                        state = MainCharacterGameState.CaughtByDude;
                    }

                }
                break;
//////////////////////////////////////////////////////////////////
            case MainCharacterGameState.CaughtByDude:

                currentHealth -= Time.deltaTime * caughtDmg;

                healthBar.value = currentHealth;

                if (currentHealth <= 0 && !isDead)
                {
                    isDead = true;
                    state = MainCharacterGameState.GameOver;
                }
                else
                {
                    if (!escapeDude)
                    {
                        controller.m_WalkSpeed = 1.0f;

                        if (soapPicked && Input.GetMouseButtonDown(0))
                        {
                            pickUp.transform.parent = null;
                            soapRg = pickUp.GetComponent<Rigidbody>();
                            soapRg.isKinematic = false;
                            soapRg.useGravity = true;
                            soapRg.velocity = Camera.main.transform.forward * 5f;
                            soapPicked = false;
                        }

                        if(!soapPicked && Input.GetMouseButtonDown(0) && Time.time >= attackStart + coolDown)
                            if (Input.GetMouseButtonDown(0) && Time.time >= attackStart + coolDown)
                            {
                                Debug.Log("Left clicked");
                                //Debug.Log(Time.time);
                                attackStart = Time.time;
                                isAttack = true;

                                if (Random.Range(0f, 1f) >= 0.5f)
                                {
                                    rightarmCollider.enabled = true;
                                    rightHandAnime.SetTrigger("handPunch");
                                }
                                else
                                {
                                    leftarmCollider.enabled = true;
                                    leftHandAnime.SetTrigger("handPunch");
                                }


                            }
                    }

                    if (escapeDude)
                    {
                        caughtByDude = false;
                        if (soapPicked)
                        {
                            state = MainCharacterGameState.PickedSoap;
                            controller.m_WalkSpeed = 5f;
                            escapeDude = false;
                        }

                        if (!soapPicked)
                        {
                            state = MainCharacterGameState.NormalState;
                            controller.m_WalkSpeed = 5f;
                            escapeDude = false;
                        }
                    }
                }
                break;

            case MainCharacterGameState.GameOver:

                Death();
                break;
        }

    }

    public void PickUp(Transform ob)
    {
        pickUp = ob;
        pickUp.position = grabber.position;
        pickUp.SetParent(grabber);
        //pickUp.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        soapRg = pickUp.GetComponent<Rigidbody>();
        soapRg.isKinematic = true;
        soapRg.useGravity = false;
        soapPicked = true;
    }

    //public void CaughtDecreaseHealth()
    //{
    //    currentHealth -= caughtDmg;
    //}

    public void TakeDamage(float amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthBar.value = currentHealth;

    }

    public void AddMood(float amount)
    {
        currentHealth += amount;
        healthBar.value = currentHealth;
    }

    public void Death()
    {
        isDead = true;

        controller.enabled = false;
        GameManager.EndGame(false);
    }



    public void enterNormalState()
    {
        state = MainCharacterGameState.NormalState;
    }

    public void enterPickedSoap()
    {
        state = MainCharacterGameState.PickedSoap;
    }

    public void ExitCaughtByDude()
    {
        escapeDude = true;
    }

    public void enterCaughByDude()
    {
        state = MainCharacterGameState.CaughtByDude;
    }

    public void enterGameOver()
    {
        state = MainCharacterGameState.GameOver;
    }

    public enum MainCharacterGameState
    {
        NormalState,

        PickedSoap,

        CaughtByDude,

        GameOver,
    }
}
