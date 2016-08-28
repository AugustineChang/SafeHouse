using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
    public new Camera camera;
    public PlayerAttack bullet;

    private CharacterController character;
    private MouseLookControl mouseLook;
    private float moveSpeed;
    private float timer;
    private float fireCD;

    void Start()
    {
        character = GetComponent<CharacterController>();
        mouseLook = new MouseLookControl();
        mouseLook.Init( transform , camera.transform );

        moveSpeed = 0.05f;
        fireCD = 1.0f;
        timer = fireCD;
    }

    void Update()
    {
        mouseLook.LookRotation( transform , camera.transform );
        
        Vector3 input = new Vector3();
        input.x = Input.GetAxis( "Horizontal" );
        input.y = -10;
        input.z = Input.GetAxis( "Vertical" );
        Vector3 moveDir = transform.TransformDirection( input );
        character.Move( moveDir * moveSpeed );
        
        checkFire();
    }

    private void checkFire()
    {
        timer += Time.deltaTime;

        if ( Input.GetButton( "Jump" ) )
        {
            if ( timer < fireCD ) return;
            else timer = 0.0f;

            PlayerAttack attack = GameObject.Instantiate<PlayerAttack>( bullet );

            Vector3 moveDir = camera.transform.TransformDirection( Vector3.forward );
            attack.transform.position = camera.transform.position + moveDir * 0.5f;
            attack.forceDir = moveDir;
        }
    }
}
