using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RAIN.Core;

public class DatingDude : Dude {

    [SerializeField]
    private GameObject countDown_Object;
    [SerializeField]
    private KissObject kiss;
    private TextMesh countDownText;
    private bool interrupted;

    [SerializeField]
    private float minKissTime;
    [SerializeField]
    private float maxKissTime;
    [SerializeField]
    private float minKissTime_wait;
    [SerializeField]
    private float maxKissTime_wait;
    [SerializeField]
    private float kissWarnTime;

    private Animator TextAnim;

    public GameObject partner;
    public Animator partner_anim;

    [SerializeField]
    private float kissDamage;
    [SerializeField]
    private float punchDamage;
    [SerializeField]
    private float punchCD;

    bool kissed;
    bool playerInfront;

    public AudioClip[] interruptedClips;
    public AudioClip[] beforeKissClips;
    public AudioClip kissClip;
    public AudioClip[] punchKissClips;



    // Use this for initialization
    public new void Start() {
        base.Start();
        aus = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        partner_anim = partner.GetComponent<Animator>();
        StartCoroutine("WaitForKiss");
        InitializeCountDownUI();
    }

    void Update()
    {
        if (!mainC)
            mainC = GameManager.player;
    }

    void InitializeCountDownUI()
    {
        TextAnim = countDown_Object.GetComponent<Animator>();
        countDownText = countDown_Object.GetComponent<TextMesh>();
    }

    IEnumerator WaitForKiss()
    {
        while (!interrupted)
        {
            float kissTime = Random.Range(minKissTime_wait, maxKissTime_wait);
            yield return new WaitForSeconds(kissTime);
            StartCoroutine("PreKiss");
            while (!kissed)
            {
                yield return null;
            }
            kissed = false;
            Debug.Log("Next kiss");
        }
    }

    IEnumerator PreKiss()
    {
        string originalStr = countDownText.text;
        float originalSize = countDownText.characterSize;
        countDown_Object.SetActive(true);
        if (kiss.isSeen)
        {
            GameManager.audioPlay.PlayRandomSound(aus, beforeKissClips);
        }
        
        float currentTime = kissWarnTime;
        while (currentTime >= 0f)
        {
            TextAnim.SetTrigger("Shake");
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            countDownText.text += "!";
            countDownText.characterSize += 0.02f;
        }
        countDownText.text = originalStr;
        countDownText.characterSize = originalSize;
        countDown_Object.SetActive(false);
        StartCoroutine("Kiss");
    }

    IEnumerator Kiss()
    {

        partner_anim.SetTrigger("Kiss");
        anim.SetTrigger("Kiss");

        //Kiss Sound
        
        float kissTime = Random.Range(minKissTime, maxKissTime);

        GameManager.audioPlay.PlayLoopedSoundForTime(aus, kissClip, kissTime);

        while (kissTime > 0)
        {
            kissTime -= Time.fixedDeltaTime;
            if (kiss.isSeen)
            {
                mainC.TakeDamage(kissDamage);
                Debug.Log(gameObject.name + " is kissing and dealing damage");
            }
            
            yield return new WaitForFixedUpdate();
        }

        partner_anim.SetTrigger("Afterkiss");
        anim.SetTrigger("Afterkiss");
        kissed = true;
        
    }

    public new void ReceiveDamage(float damage)
    {
        Debug.Log("Receive damage: " + damage);
        StopAllCoroutines();
        if (partner)
        {
            partner.transform.parent = null;
            partner.GetComponentInChildren<AIRig>().AI.WorkingMemory.SetItem<bool>("dating", false);
        }

        if (!interrupted)
        {
            interrupted = true;
            anim.SetBool("Interrupted", true);
            aiRig.AI.WorkingMemory.SetItem<bool>("dating", false);
            countDown_Object.SetActive(false);
            GameManager.audioPlay.PlayRandomSound(aus, interruptedClips);
        }

        if (!isDead)
        {
            StartCoroutine("GetHit");
        }

        health -= damage;
        if (health <= 0)
        {
            Death();
        }

        //base.ReceiveDamage(damage);
    }

    public new void Death()
    {
        base.Death();
        anim.SetBool("Die", true);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("On Trigger Enter");
        

        if (other.CompareTag("Player"))
        {
            playerInfront = true;
        }

        if (mainC.isAttack && other.CompareTag("Punch"))
        {
            GameManager.audioPlay.PlayRandomSound(aus, punchKissClips);
            ReceiveDamage(mainC.punchDmg);
            GameObject bloodP = Instantiate(bloodParticle, transform.position, Quaternion.identity) as GameObject;
            Destroy(bloodP, 10f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInfront = true;
        }
    }

    public void Punch(GameObject gameObject)
    {
        Debug.Log("Punch");
        anim.SetTrigger("Punch");
        gameObject.GetComponent<mainCharacter>().TakeDamage(punchDamage);
    }
}
