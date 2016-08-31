using UnityEngine;
using System.Collections;

public class CameraFlash : MonoBehaviour
{
    public Shader flashShader;
    [Range(0,1)]
    public float flashProgress;
    public Color flashColor;
    [Range(0,1)]
    public float blackProgress;

    private GameObject canvas;
    private Material mat;
    private float timer;
    private int playStatus;

    public int StartFlash
    {
        set
        {
            if (value == 1 && playStatus == 1) timer = 0.0f;
            if (value == 2)
            {
                timer = 0.0f;
                flashProgress = 0.0f;
                blackProgress = 0.0f;
            }
            playStatus = value;
        }
    }

    void Start()
    {
        timer = 0.0f;
        flashProgress = 0.0f;
        mat = new Material(flashShader);
        mat.hideFlags = HideFlags.HideAndDontSave;

        mat.SetColor("_Color", flashColor);
        canvas = GameObject.Find("Canvas");
    }

    void Update()
    {
        if (playStatus == 1)
        {
            timer += Time.deltaTime * 5.0f;
            flashProgress = timer;

            if (timer >= 1.0f)
            {
                timer = 0.0f;
                flashProgress = 0.0f;
                StartFlash = 0;
            }
        }
        else if (playStatus == 2)
        {
            timer += Time.deltaTime;
            blackProgress = timer;

            if (timer >= 1.0f)
            {
                timer = 0.0f;
                blackProgress = 1.0f;
                StartFlash = 0;

                canvas.SetActive(true);
            }
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        mat.SetFloat("_Progress", flashProgress);
        mat.SetFloat("_Progress2", 1-blackProgress);
        Graphics.Blit(src, dest, mat);
    }
}
