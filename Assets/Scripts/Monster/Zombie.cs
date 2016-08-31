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
    protected FPSController target;
    protected int health;
    protected float stopTime;
    private float attackTimer;

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
        target = GameObject.Find("Player").GetComponent<FPSController>();
    }

    public void whenHitZombie( int damage )
    {
        health -= damage;
        if ( health <= 0 )
        {
            animator.Play( "Death" );
            Destroy( gameObject , 2.5f );
            stopTime = 2.5f;
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

    protected void AttackPlayer( float attackTime , float totalTime , bool isReset = false )
    {
        if (isReset) attackTimer = 0.0f;

        float lastTimer = attackTimer;
        attackTimer += Time.deltaTime;
        if (attackTimer >= totalTime)
        {
            attackTimer = 0.0f;
        }
        else if (attackTimer >= attackTime && lastTimer < attackTime)
        {
            int rand = UnityEngine.Random.Range(-5, 6);
            target.HitPlayer(10 + rand);
        }

        if (target.isDead()) Destroy(gameObject, 2.0f);
    }
}
