using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatUI : MonoBehaviour
{

    
    public Transform target;

    [SerializeField]
    Vector2 offset_viewport;

    [SerializeField]
    MaskableGraphic[] UIElements;

    public float stayTime;

    public Canvas canvas;

    Animator imageAnim;


    // Use this for initialization
    void Start()
    {
        
    }

    public void Activate()
    {
        Debug.Log("Activate");
        SetUIElements(true);
        StartCoroutine("FollowTarget");
    }

    public void Deactivate()
    {
        SetUIElements(false);
        StopCoroutine("FollowTarget");
    }



    IEnumerator FollowTarget()
    {
        if (!target)
        {
            throw new UnityException("Target is null");
        }

        while (true)
        {
            Vector2 pos;
            float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
            float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
            float x = Camera.main.WorldToScreenPoint(target.position).x / Screen.width;
            float y = Camera.main.WorldToScreenPoint(target.position).y / Screen.height;
            pos = new Vector2(width * x - width / 2, y * height - height / 2);
            pos += offset_viewport;
            GetComponent<RectTransform>().anchoredPosition = pos;

            yield return null;
        }
    }

    void SetUIElements(bool status)
    {
        foreach (MaskableGraphic element in UIElements)
        {
            element.enabled = status;
        }
    }

}