using UnityEngine;
using System.Collections;

public class ZombieCreeper : Zombie
{
    private Brick attackBrick;
    private float timer;

    //creep
    private bool isAttack;
    private int state;
    private float speed;

	public void Init( Brick brick )
    {
        base.Init();

        Vector3 pos = brick.transform.position;
        pos.y = 0;
        Quaternion rot = Quaternion.Euler( 0 , brick.transform.eulerAngles.y , 0 );

        transform.position = pos + rot * new Vector3( 0 , 0 , -1.2f );
        transform.rotation = rot;

        timer = 0.367f;
        attackBrick = brick;
        attackBrick.AddListener( OnBrickDestroy );

        state = 0;
        speed = 0.6f;
	}
	
	void Update()
    {
        if ( target.isDead() )
        {
            Destroy( gameObject , 2.0f );
            return;
        }

        if ( attackBrick == null )
        {
            if ( state == 0 )
            {
                timer += Time.deltaTime;
                if ( timer > 2.0f )
                {
                    state = 1;
                    return;
                }
                else
                {
                    Vector3 forward = transform.TransformDirection( Vector3.forward );
                    transform.localPosition += forward * speed * Time.deltaTime;
                }
                    
            }
            else if ( state == 1 ) creep();
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

    private void creep()
    {
        if ( stopTime > 0.0f )
        {
            stopTime -= Time.deltaTime;
            return;
        }

        Vector3 targetPos = new Vector3( target.transform.position.x , 0 , target.transform.position.z );
        float distance = Vector3.Distance( transform.position , targetPos );
        if ( distance <= 1.5f )
        {
            AttackPlayer(0.333f, 0.667f, !isAttack);
            if ( !isAttack )
            {
                isAttack = true;
                animator.SetInteger( "status" , 0 );
            }
        }
        else
        {
            if ( isAttack )
            {
                isAttack = false;
                animator.SetInteger( "status" , 1 );
            }

            Vector3 forward = transform.TransformDirection( Vector3.forward );
            transform.localPosition += forward * speed * Time.deltaTime;
        }

        Vector3 lookForward = targetPos - transform.position;
        transform.rotation = Quaternion.Slerp( transform.rotation , Quaternion.LookRotation( lookForward ) , Time.deltaTime * 3 );
    }

    private void OnBrickDestroy( int index , BrokenHole hole )
    {
        attackBrick.RemoveListener( OnBrickDestroy );
        attackBrick = null;

        animator.SetInteger( "status" , 1 );
        timer = 0.0f;
    }
}
