using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public BrickPiece BrickPiece;
    [HideInInspector]
    public int BrickIndex;

    private static AudioClip[] hitSounds;
    private static AudioClip crashSound;

    private event Action<int> DestroyHandler;
    private bool isCrash;
    private bool isInit = false;
    private int hitTimes;
    private AudioSource audioSource;
    private MeshRenderer meshRender;
    private Material brickMat;

    void Start()
    {
        isCrash = false;
        if ( hitSounds == null )
        {
            hitSounds = new AudioClip[5];
            for ( int i = 1 ; i <= 5 ; i++ )
            {
                hitSounds[i-1] = Resources.Load<AudioClip>( "Audio/Wood" + i );
            }
        }

        if ( crashSound == null )
        {
            crashSound = Resources.Load<AudioClip>( "Audio/WoodCrash" );
        }

        isInit = true;
        hitTimes = 0;
        audioSource = GetComponent<AudioSource>();
        meshRender = GetComponent<MeshRenderer>();
        brickMat = meshRender.material;
    }

    public void AddListener( Action<int> action )
    {
        DestroyHandler += action;
    }

    public void RemoveListener( Action<int> action )
    {
        DestroyHandler -= action;
    }

    public void BeHit()
    {
        if ( !isInit || isCrash ) return;

        if ( hitTimes < 5 )
        {
            audioSource.clip = hitSounds[hitTimes];
            audioSource.Play();
            hitTimes++;

            Vector3 forward = transform.TransformDirection( Vector3.forward );
            iTween.ShakePosition( gameObject , forward * 0.3f , 0.1f );
            brickMat.color += new Color( -0.05f , -0.05f , -0.05f );
        }
        else
        {
            audioSource.clip = crashSound;
            audioSource.Play();

            PlayCrashAnime();
        }
    }

    public bool isHit() { return hitTimes > 0; }

    public void PlayCrashAnime( bool isDestroy = true )
    {
        isCrash = true;
        crashAnime();
        GetComponent<BoxCollider>().enabled = false;
        meshRender.enabled = false;
        if ( isDestroy ) Destroy( gameObject , 1.0f );

        if ( DestroyHandler != null )
            DestroyHandler.Invoke( BrickIndex );
    }

    private void crashAnime()
    {
        int num = 16;
        float size = 1.0f / 4;
        for ( int i = 0 ; i < num ; i++ )
        {
            BrickPiece piece = GameObject.Instantiate<BrickPiece>( BrickPiece );
            piece.Init();
            piece.transform.SetParent( transform.parent );

            Vector3 position = transform.localPosition;
            position.x += ( i % 4 ) * size - 0.375f;
            position.y += ( i / 4 ) * size - 0.375f;
            piece.transform.localPosition = position;
            piece.transform.localRotation = Quaternion.identity;
        }
    }
}
