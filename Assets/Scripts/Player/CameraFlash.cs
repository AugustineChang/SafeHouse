using UnityEngine;
using System.Collections;

public class CameraFlash : MonoBehaviour
{
    public Shader flashShader;
    [Range(0,1)]
    public float flashProgress;
    public Color flashColor;
    //[HideInInspector]
    public bool StartFlash;

    private Material mat;
    private float timer;

    void Start()
    {
        timer = 0.0f;
        flashProgress = 0.0f;
        mat = new Material(flashShader);
        mat.hideFlags = HideFlags.HideAndDontSave;

        mat.SetColor("_Color", flashColor);
    }

    void Update()
    {
        if (!StartFlash) return;

        timer += Time.deltaTime * 5.0f;
        flashProgress = timer;

        if (timer >= 1.0f)
        {
            timer = 0.0f;
            flashProgress = 0.0f;
            StartFlash = false;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        mat.SetFloat("_Progress", flashProgress);
        Graphics.Blit(src, dest, mat);
    }
}
