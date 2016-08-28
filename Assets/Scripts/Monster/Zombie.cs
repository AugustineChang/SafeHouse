using System;
using UnityEngine;

public abstract class Zombie : MonoBehaviour
{
    [HideInInspector]
    public Action WhenDie;

    protected new CapsuleCollider collider;
    protected Animator animator;
    private AudioSource audioSource;
    private AudioClip[] soundList;
    private int health;
    protected float stopTime;

    protected void Init()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();

        soundList = new AudioClip[5];
        soundList[0] = Resources.Load<AudioClip>( "Audio/HitZombie" );
        for ( int i = 1 ; i < 5 ; i++ )
        {
            soundList[i] = Resources.Load<AudioClip>( "Audio/Zombie" + i );
        }

        health = 100;
    }

    public void whenHitZombie( int damage )
    {
        health -= damage;
        if ( health <= 0 )
        {
            animator.Play( "Death" );
            Destroy( gameObject , 3.0f );
            stopTime = 3.0f;
            WhenDie.Invoke();
            zombieSound( 100 );
        }
        else
        {
            animator.SetBool( "isHit" , true );
            stopTime = 0.667f;
            zombieSound( 20 );
        }
    }

    private void zombieSound( int prob )
    {
        //prob%概率吼叫 (100-prob)%概率一般打中声音
        int rand = UnityEngine.Random.Range( 0 , 400 / prob );
        if ( rand < 4 ) audioSource.clip = soundList[rand + 1];
        else audioSource.clip = soundList[0];
        audioSource.Play();
    }
}
