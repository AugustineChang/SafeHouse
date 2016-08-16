using UnityEngine;
using System.Collections;

public class ZombieAttacker : MonoBehaviour
{
    private Animator animator;
    private BrickCrash attackBrick;
    private float timer;

	public void Init( BrickCrash brick )
    {
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
        attackBrick = brick;
        attackBrick.AddListener( OnBrickDestroy );
        animator = GetComponent<Animator>();
	}
	
	void Update()
    {
        if ( attackBrick == null ) return;

        timer += Time.deltaTime;
        if ( timer > 0.667f )
        {
            timer = 0.0f;

            attackBrick.BeHit();
        }
	}

    private void OnBrickDestroy( int index )
    {
        attackBrick.RemoveListener( OnBrickDestroy );
        attackBrick = null;
        animator.Play( "Attack01" );
    }
}
