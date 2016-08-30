using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    private Material fadeTarget;
    private float timer;
    private Color startCol;
    private bool isEnd;

    void Start()
    {
        isEnd = false;
        timer = 0.0f; 
        fadeTarget = GetComponent<MeshRenderer>().material;
        startCol = fadeTarget.color;
    }

    void Update()
    {
        if (isEnd) return;

        timer += Time.deltaTime * 2.0f;
        if (timer >= 1.0f)
        {
            timer = 1.0f;
            isEnd = true;
        }
        
        fadeTarget.color = Color.Lerp(startCol, Color.white, timer);
    }
}
