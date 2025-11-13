using System.Collections;
using UnityEngine;

public class RendererController : MonoBehaviour
{  
    private Material material;
    private Color initialColor;    
    private void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;
        initialColor = material.color;
    }
    public void ResetMaterialColor()
    {
        material.color = initialColor;
    }
    public void FadeOutMaterialColor(float duration)
    {
        StartCoroutine(FadeOutCo(duration));        
    }

    IEnumerator FadeOutCo(float duration)
    {        
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed+= Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);    
            yield return null;

            //Debug.Log("Fading... Alpha: " + alpha);
        }
        material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }


}
