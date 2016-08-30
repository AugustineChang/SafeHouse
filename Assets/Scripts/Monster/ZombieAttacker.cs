using UnityEngine;
using System.Collections;

public class ZombieAttacker : Zombie
{
    private Brick attackBrick;
    private Brick downBrick;
    private BrokenHole attackHole;
    private BrokenHole downHole;

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

        attackBrick = brick;
        attackBrick.AddListener( OnBrickDestroy );
        downBrick = brick2;
        if( downBrick ) downBrick.AddListener( OnBrickDownDestroy );
    }
	
	void Update()
    {
        if ( attackBrick == null )
        {
            if (downBrick == null)
            {
                timer += Time.deltaTime;
                if (timer > 2.0f) walkAndAttack();
                else
                {
                    Vector3 forward = transform.TransformDirection(Vector3.forward);
                    transform.localPosition += forward * walkSpeed * Time.deltaTime;
                }
            }
            else attackBehindWall();
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

        Vector3 targetPos = new Vector3( target.transform.position.x , 0 , target.transform.position.z );
        float distance = Vector3.Distance( transform.position , targetPos );
        if ( distance <= 1.0f )
        {
            AttackPlayer(0.5f, 1.0f, !isAttack);
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

    private void attackBehindWall()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance <= 1.35f)
        {
            AttackPlayer(0.167f, 0.667f);
        }
    }

    private void OnBrickDestroy( int index , BrokenHole hole )
    {
        if (animator == null) return;

        attackBrick.RemoveListener( OnBrickDestroy );
        attackBrick = null;
        
        if (downBrick == null) animator.SetInteger("status", 2);
        else
        {
            animator.SetInteger("status", 1);

            attackHole = hole;
            attackHole.AddListener(OnBrickRepair);
        }
    }

    private void OnBrickRepair(int index, Brick brick)
    {
        if (animator == null) return;

        //下面砖破了 说明怪已经进屋了 就无需相应事件
        if (downBrick == null) return;
        if (health <= 0) return;

        attackHole.RemoveListener(OnBrickRepair);
        attackHole = null;

        attackBrick = brick;
        attackBrick.AddListener(OnBrickDestroy);

        //下砖没破 改为继续拍当前砖
        animator.SetInteger("status", 0);
        animator.Play("Idle");
    }

    private void OnBrickDownDestroy( int index, BrokenHole hole )
    {
        if (animator == null) return;

        downBrick.RemoveListener( OnBrickDestroy );
        downBrick = null;

        if (attackBrick == null) animator.SetInteger("status", 2);
        else//当前砖破了 说明怪已经进屋了 就无需相应事件
        {
            downHole = hole;
            downHole.AddListener(OnBrickDownRepair);
        }
    }

    private void OnBrickDownRepair(int index, Brick brick)
    {
        if (animator == null) return;

        //当前砖破了 说明怪已经进屋了 就无需相应事件
        if (attackBrick == null) return;

        downHole.RemoveListener(OnBrickDownRepair);
        downHole = null;

        downBrick = brick;
        downBrick.AddListener(OnBrickDownDestroy);
    }
}
