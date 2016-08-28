using UnityEngine;
using System.Collections;

public class ZombieAttacker : Zombie
{
    private Transform target;
    private Brick attackBrick;
    private Brick downBrick;

    private float timer;
    private float walkSpeed;
    private bool isAttack;

    public void Init( Brick brick , Brick brick2 )
    {
        base.Init();

        Vector3 pos = brick.transform.position;
        pos.y = 0;
        Quaternion rot = Quaternion.Euler( 0 , brick.transform.eulerAngles.y , 0 );

        int rand = Random.Range( 0 , 2 );
        if ( rand == 0 )//left
        {
            transform.position = pos + rot * new Vector3( -0.3f , 0 , -0.7f );
            transform.rotation = rot;
        }
        else//right
        {
            transform.position = pos + rot * new Vector3( 0.3f , 0 , -0.7f );
            transform.rotation = rot;
            transform.localScale = new Vector3( -1 , 1 , 1 );
        }

        timer = 0.0f;
        walkSpeed = 0.6f;
        target = GameObject.Find( "Player" ).transform;

        attackBrick = brick;
        attackBrick.AddListener( OnBrickDestroy );
        downBrick = brick2;
        if( downBrick ) downBrick.AddListener( OnBrickDownDestroy );
    }
	
	void Update()
    {
        if ( attackBrick == null )
        {
            if ( downBrick == null )
            {
                timer += Time.deltaTime;
                if ( timer > 2.0f ) walkAndAttack();
                else
                {
                    Vector3 forward = transform.TransformDirection( Vector3.forward );
                    transform.localPosition += forward * walkSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            timer += Time.deltaTime;
            if ( timer > 0.667f )
            {
                timer = 0.0f;

                attackBrick.BeHit();
            }
        }
	}

    private void walkAndAttack()
    {
        if ( stopTime > 0.0f )
        {
            stopTime -= Time.deltaTime;
            return;
        }

        Vector3 targetPos = new Vector3( target.position.x , 0 , target.position.z );
        float distance = Vector3.Distance( transform.position , targetPos );
        if ( distance <= 1.0f )
        {
            if ( !isAttack )
            {
                isAttack = true;
                animator.SetInteger( "status" , 3 );
            }
        }
        else
        {
            if ( isAttack )
            {
                isAttack = false;
                animator.SetInteger( "status" , 2 );
            }

            Vector3 forward = transform.TransformDirection( Vector3.forward );
            transform.localPosition += forward * walkSpeed * Time.deltaTime;
        }

        Vector3 lookForward = targetPos - transform.position;
        transform.rotation = Quaternion.Slerp( transform.rotation , Quaternion.LookRotation( lookForward ) , Time.deltaTime * 3 );
    }

    private void OnBrickDestroy( int index )
    {
        attackBrick.RemoveListener( OnBrickDestroy );
        attackBrick = null;

        if ( downBrick == null ) animator.SetInteger( "status" , 2 );
        else animator.SetInteger( "status" , 1 );
    }

    private void OnBrickDownDestroy( int index )
    {
        downBrick.RemoveListener( OnBrickDestroy );
        downBrick = null;

        if ( attackBrick == null ) animator.SetInteger( "status" , 2 );
    }
}
