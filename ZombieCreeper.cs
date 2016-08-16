using UnityEngine;
using System.Collections;

public class ZombieCreeper : MonoBehaviour
{
    private Animator animator;
    private BrickCrash attackBrick;
    private float timer;

    //creep
    private Transform target;
    private bool isAttack;
    private int state;
    private float speed;

	public void Init( BrickCrash brick )
    {
        Vector3 pos = brick.transform.position;
        pos.y = 0;
        Quaternion rot = Quaternion.Euler( 0 , brick.transform.eulerAngles.y , 0 );

        transform.position = pos + rot * new Vector3( 0 , 0 , -1.2f );
        transform.rotation = rot;

        timer = 0.367f;
        attackBrick = brick;
        attackBrick.AddListener( OnBrickDestroy );
        animator = GetComponent<Animator>();

        state = 0;
        speed = 0.6f;
        target = GameObject.Find( "Player" ).transform;
	}
	
	void Update()
    {
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
        Vector3 targetPos = new Vector3( target.position.x , 0 , target.position.z );
        float distance = Vector3.Distance( transform.position , targetPos );
        if ( distance <= 1.5f )
        {
            if ( !isAttack )
            {
                isAttack = true;
                animator.Play( "GroundAttack" );
            }
        }
        else
        {
            if ( isAttack )
            {
                isAttack = false;
                animator.Play( "GroundWalk" );
            }
            
            Vector3 lookForward = targetPos - transform.position;
            transform.rotation = Quaternion.Slerp( transform.rotation , Quaternion.LookRotation( lookForward ) , Time.deltaTime * 3 );

            Vector3 forward = transform.TransformDirection( Vector3.forward );
            transform.localPosition += forward * speed * Time.deltaTime;
        }
    }

    private void OnBrickDestroy( int index )
    {
        attackBrick.RemoveListener( OnBrickDestroy );
        attackBrick = null;
        animator.Play( "GroundWalk" );
        timer = 0.0f;
    }
}
