using UnityEngine;
using System.Collections;

public class Girl : MonoBehaviour {

    public float exitTime;
    public SkinnedMeshRenderer mesh;

    public Color finalColor;

    Material material;
    public bool disappearing;

	// Use this for initialization
	void Start () {
        material = mesh.material;

    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Disappear()
    {
        if (!disappearing)
        {
            disappearing = true;
            Debug.Log("Disappear");
            StartCoroutine("lerpToTransparent");
        }
        
    }

    IEnumerator lerpToTransparent()
    {
        float currentTime = exitTime;
        mesh.material.SetFloat("_Mode", 3);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        while (currentTime > 0)
        {
            currentTime -= Time.fixedDeltaTime;
            material.color = ColorShift(Color.white, finalColor, (1 - currentTime/exitTime));
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    Color ColorShift(Color start, Color dest, float lerp)
    {
        float finalColor_r = Mathf.Lerp(start.r, dest.r, lerp);
        float finalColor_g = Mathf.Lerp(start.g, dest.g, lerp);
        float finalColor_b = Mathf.Lerp(start.b, dest.b, lerp);
        float finalColor_a = Mathf.Lerp(start.a, dest.a, lerp);
        return new Color(finalColor_r, finalColor_g, finalColor_b, finalColor_a);
    }
}
