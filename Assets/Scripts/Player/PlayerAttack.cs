using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector]
    public Vector3 forceDir;

    private new Rigidbody rigidbody;
    private float timer;

    void Start()
    {
        timer = 0.0f;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if ( timer < 0.02f ) rigidbody.AddForce( forceDir * rigidbody.mass * 400 );

        if ( timer > 5 ) Destroy( gameObject );
    }

    void OnCollisionEnter( Collision collision )
    {
        if ( collision.collider.CompareTag( "Zombie" ) )
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            zombie.whenHitZombie( Random.Range( -5 , 6 ) + 20 );

            Destroy( gameObject );
        }
        else if ( collision.collider.CompareTag( "Ground" ) )
        {
            //Destroy( gameObject );
        }
    }
}
