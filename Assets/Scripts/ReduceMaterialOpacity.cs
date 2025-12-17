using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class ReduceMaterialOpacity : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecuteurOpacite(float fadeInDuration)
    {
        StartCoroutine(ReduceTimeline(fadeInDuration));
    }

    public IEnumerator ReduceTimeline(float fadeInDuration)
    {
        float elapsed = 0f;
        var trans = GetComponent<MeshRenderer>().material.color.a;
        var rouge = GetComponent<MeshRenderer>().material.color.r;
        var vert = GetComponent<MeshRenderer>().material.color.g;
        var bleu = GetComponent<MeshRenderer>().material.color.b;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(trans, 0.2f, elapsed / fadeInDuration);
            GetComponent<MeshRenderer>().material.color = new Color(rouge, vert, bleu, alpha);
            yield return null;
        }
    }
}
