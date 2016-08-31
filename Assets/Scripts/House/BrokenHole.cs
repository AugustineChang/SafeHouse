using System;
using UnityEngine;

public class BrokenHole : MonoBehaviour
{
    public Brick Brick;
    public GameObject Batten;

    private int brickIndex;
    private event Action<int, Brick> whenRepairFinish;

    private int repairTimes;
    private int maxRepairTimes;

    public void Init( int index )
    {
        brickIndex = index;

        repairTimes = 0;
        maxRepairTimes = 5;
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
        GameObject batten = GameObject.Instantiate<GameObject>(Batten);
        batten.transform.SetParent(transform);
        batten.transform.localPosition = Vector3.up * (0.4f - repairTimes * 0.2f);
        batten.transform.localRotation = Quaternion.identity;
        batten.transform.localScale = new Vector3(1, 0.2f, 1);
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
}
