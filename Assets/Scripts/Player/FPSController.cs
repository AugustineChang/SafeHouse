using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
    public new Camera camera;
    public PlayerAttack bullet;

    private CharacterController character;
    private MouseLookControl mouseLook;
    private float moveSpeed;
    private float fireTimer;
    private float repairTimer;
    private float calmDown;
    private int rayLayer;

    private float health;
    private CameraFlash cameraFlash;

    void Start()
    {
        character = GetComponent<CharacterController>();
        mouseLook = new MouseLookControl();
        mouseLook.Init(transform, camera.transform);

        moveSpeed = 0.05f;
        calmDown = 0.5f;
        fireTimer = calmDown;
        repairTimer = calmDown;
        rayLayer = LayerMask.NameToLayer("BrokenHole");

        health = 200;
        cameraFlash = camera.GetComponent<CameraFlash>();
    }

    void Update()
    {
        if (isDead()) return;

        playerMove();
        checkFire();
        checkRepair();
    }

    private void playerMove()
    {
        mouseLook.LookRotation(transform, camera.transform);

        Vector3 input = new Vector3();
        input.x = Input.GetAxis("Horizontal");
        input.y = -10;
        input.z = Input.GetAxis("Vertical");
        Vector3 moveDir = transform.TransformDirection(input);
        character.Move(moveDir * moveSpeed);
    }

    private void checkFire()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            if (fireTimer < calmDown) return;
            else fireTimer = 0.0f;

            PlayerAttack attack = GameObject.Instantiate<PlayerAttack>(bullet);

            Vector3 moveDir = camera.transform.TransformDirection(Vector3.forward);
            attack.transform.position = camera.transform.position + moveDir * 0.5f;
            attack.forceDir = moveDir;
        }
    }

    private void checkRepair()
    {
        repairTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (repairTimer < calmDown) return;
            else repairTimer = 0.0f;

            RaycastHit hitData;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitData, 2.0f, 1 << rayLayer))
            {
                BrokenHole hole = hitData.collider.GetComponent<BrokenHole>();
                hole.RepairTheHole();
            }
        }
    }

    public void HitPlayer(int damage)
    {
        if (isDead()) return;

        health -= damage;

        if (health > 0) cameraFlash.StartFlash = 1;
        else cameraFlash.StartFlash = 2;
    }

    public bool isDead() { return health <= 0; }

    public void ResetPlayer()
    {
        health = 200;
    }
}
