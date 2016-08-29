using System;
using UnityEngine;

public class BrokenHole : MonoBehaviour
{
    public Brick Brick;

    private int brickIndex;
    private event Action<int, Brick> whenRepairFinish;

    private int repairTimes;
    private int maxRepairTimes;

    private float timer;
    private Material fadeTarget;
    private Color startCol;

    public void Init( int index )
    {
        brickIndex = index;

        timer = 0.0f;
        repairTimes = 0;
        maxRepairTimes = 5;
        startCol = new Color(1, 1, 1, 0);
    }

    public void AddListener(Action<int, Brick> callback)
    {
        whenRepairFinish += callback;
    }

    public void RemoveListener(Action<int, Brick> callback)
    {
        whenRepairFinish -= callback;
    }

    public void RepairTheHole()
    {
        GameObject batten = GameObject.CreatePrimitive(PrimitiveType.Cube);
        batten.transform.SetParent(transform);
        batten.transform.localPosition = Vector3.up * (0.4f - repairTimes * 0.2f);
        batten.transform.localScale = new Vector3(1, 0.2f, 1);

        fadeTarget = batten.GetComponent<MeshRenderer>().material;
        fadeTarget.color = startCol;

        Destroy(batten.GetComponent<BoxCollider>());
        repairTimes++;

        if (repairTimes >= maxRepairTimes)
        {
            Brick brick = GameObject.Instantiate<Brick>(Brick);
            brick.transform.SetParent(transform.parent);
            brick.transform.position = transform.position;
            brick.transform.localRotation = Quaternion.identity;

            if (whenRepairFinish != null) whenRepairFinish.Invoke(brickIndex, brick);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (fadeTarget == null) return;

        timer += Time.deltaTime;
        fadeTarget.color = Color.Lerp(startCol, Color.white, timer);
        if (timer > 1.0f)
        {
            fadeTarget = null;
            timer = 0.0f;
        }
    }
}
