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

    private UIControl canvas;
    private Material mat;
    private float timer;
    private bool isFlash;
    private bool isBlack;

    public int StartFlash
    {
        set
        {
            if ( value == 0 )
            {
                isFlash = false;
                isBlack = false;
            } 
            else if ( value == 1 ) isFlash = true;
            else if ( value == 2 )
            {
                isBlack = true;
                Reset();
            }

            timer = 0.0f;
        }
    }

    public void Reset()
    {
        flashProgress = 0.0f;
        blackProgress = 0.0f;
        if ( canvas ) canvas.SetActive( false );
    }

    void Start()
    {
        flashProgress = 0.0f;
        mat = new Material(flashShader);
        mat.hideFlags = HideFlags.HideAndDontSave;

        mat.SetColor("_Color", flashColor);
        canvas = GameObject.Find( "Canvas" ).GetComponent<UIControl>();
    }

    void Update()
    {
        if ( isFlash )
        {
            timer += Time.deltaTime * 5.0f;
            flashProgress = timer;

            if ( timer >= 1.0f )
            {
                flashProgress = 0.0f;
                StartFlash = 0;
            }
        }

        if ( isBlack )
        {
            timer += Time.deltaTime;
            blackProgress = timer;

            if ( timer >= 1.0f )
            {
                blackProgress = 1.0f;
                StartFlash = 0;

                canvas.SetActive( true );
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
