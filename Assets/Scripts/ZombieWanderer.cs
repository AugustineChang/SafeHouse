using UnityEngine;
using System.Collections;

public class ZombieWanderer : MonoBehaviour 
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;

    private Quaternion startRot;
    private Quaternion endRot;

    private int state;
    private float timer;
    private float timeFactor;

    void Start()
    {
        state = 0;
        timer = 0.0f;
        timeFactor = Vector3.Distance( StartPoint , EndPoint ) / 0.9f;
        
        startRot = Quaternion.LookRotation( EndPoint - StartPoint );
        endRot = Quaternion.LookRotation( StartPoint - EndPoint );
        transform.rotation = startRot;
    }

	void Update()
    {
        if ( state == 0 ) walk();
        else if ( state == 1 ) turnBack();
	}

    private void walk()
    {
        timer += Time.deltaTime / timeFactor;

        if ( timer > 1.0f )
        {
            transform.position = Vector3.Lerp( StartPoint , EndPoint , 1.0f );

            state = 1;
            timer = 0.0f;
            Vector3 temp = StartPoint;
            StartPoint = EndPoint;
            EndPoint = temp;
        }
        else
        {
            transform.position = Vector3.Lerp( StartPoint , EndPoint , timer );
        }
    }

    private void turnBack()
    {
        timer += Time.deltaTime;
        if ( timer > 1.0f )
        {
            transform.rotation = Quaternion.Slerp( startRot , endRot , 1.0f );

            state = 0;
            timer = 0.0f;
            Quaternion temp = startRot;
            startRot = endRot;
            endRot = temp;
        }
        else
        {
            transform.rotation = Quaternion.Slerp( startRot , endRot , timer );
        }
    }
}
