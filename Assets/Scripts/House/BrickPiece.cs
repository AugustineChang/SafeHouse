using UnityEngine;
using System.Collections;

public class BrickPiece : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private float timer;
    private Vector3 forceDir;

	public void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        timer = 0.0f;
        forceDir = Random.onUnitSphere;
	}

	void Update()
    {
        timer += Time.deltaTime;
        if ( timer <= 0.05f )
        {
            rigidbody.AddRelativeForce( forceDir * 100 );
        }
        else if ( timer > 2.0f )
        {
            Destroy( gameObject );
        }
	}
}
