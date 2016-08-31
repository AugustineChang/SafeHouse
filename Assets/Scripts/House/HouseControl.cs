using UnityEngine;
using System.Collections.Generic;

public class HouseControl : MonoBehaviour
{
    public Brick Brick;
    public GameObject Pillar;
    public GameObject Roof;
    public GameObject Floor;
    public ZombieAttacker Zombie;
    public ZombieCreeper Zombie2;
    
    private List<Brick> brickList;
    private List<BrokenHole> holeList;
    private float timer;
    private int zombieNum;
    private int zombieMaxNum = 20;

    void Start()
    {
        brickList = new List<Brick>( 120 );
        holeList = new List<BrokenHole>( 120 );
        for (int i = 0; i < 120; i++) holeList.Add(null);

        createHouse();
    }

    private void createHouse()
    {
        createWall("LeftWall", new Vector3(-4.5f, 0.5f, -5), Quaternion.identity);
        createWall("RightWall", new Vector3(4.5f, 0.5f, 5), Quaternion.Euler(0, 180, 0));
        createWall("FrontWall", new Vector3(5, 0.5f, -4.5f), Quaternion.Euler(0, -90, 0));
        createWall("BackWall", new Vector3(-5, 0.5f, 4.5f), Quaternion.Euler(0, 90, 0));

        createPillar(new Vector3(-5, 0, -5));
        createPillar(new Vector3(-5, 0, 5));
        createPillar(new Vector3(5, 0, -5));
        createPillar(new Vector3(5, 0, 5));

        GameObject roof = GameObject.Instantiate<GameObject>(Roof);
        roof.transform.SetParent(transform);
        roof.transform.localPosition = Vector3.up * 3.15f;

        GameObject floor = GameObject.Instantiate<GameObject>(Floor);
        floor.transform.SetParent(transform);
        floor.transform.localPosition = Vector3.down * 0.1f;
    }

    void Update()
    {
        if ( zombieNum >= zombieMaxNum ) return;

        timer += Time.deltaTime;
        float targetTime = ( zombieNum ) * 5.0f + 5.0f;
        if ( timer > targetTime )
        {
            timer = 0.0f;
            randBrickCrash();
            zombieNum++;
        }
    }

    private void createWall( string name , Vector3 position , Quaternion rotation )
    {
        GameObject wall = new GameObject( name );
        wall.transform.SetParent( transform );
        wall.transform.localPosition = position;

        for ( int j = 0 ; j < 3 ; j++ )
        {
            for ( int i = 0 ; i < 10 ; i++ )
            {
                Brick newBrick = GameObject.Instantiate<Brick>( Brick );
                newBrick.transform.SetParent( wall.transform );
                newBrick.transform.localPosition = Vector3.right * i + Vector3.up * j;
                newBrick.AddListener( OnBrickDestroy );
                brickList.Add( newBrick );
                newBrick.BrickIndex = brickList.Count - 1;
            }
        }

        wall.transform.localRotation = rotation;
    }

    private void createPillar( Vector3 position )
    {
        GameObject pillar = GameObject.Instantiate<GameObject>( Pillar );
        pillar.transform.SetParent( transform );

        position.y = 1.5f;
        pillar.transform.localPosition = position;
    }

    private void OnBrickDestroy( int index , BrokenHole hole )
    {
        brickList[index].RemoveListener( OnBrickDestroy );

        hole.AddListener(OnBrickRepair);
        holeList[index] = hole;
        brickList[index] = null;
        
        checkWallBreak( index / 30 );
    }

    private void OnBrickRepair(int index , Brick brick)
    {
        brick.AddListener(OnBrickDestroy);
        brickList[index] = brick;
        holeList[index] = null;
    }

    private void OnZombieDie()
    {
        zombieNum--;
    }

    private void checkWallBreak( int index )
    {
        int breakNum = 0;
        int start = index * 30;
        int end = index * 30 + 30;
        for ( int i = start ; i < end ; i++ )
        {
            if ( brickList[i] == null ) breakNum++;
        }

        if ( breakNum < 7 ) return;

        for ( int i = start ; i < end ; i++ )
        {
            if ( brickList[i] != null )
            {
                brickList[i].RemoveListener( OnBrickDestroy );
                brickList[i].PlayCrashAnime();
                brickList[i] = null;
            }
            if ( holeList[i] != null)
            {
                holeList[i].RemoveListener(OnBrickRepair);
                Destroy(holeList[i].gameObject);
                holeList[i] = null;
            }
        }
    }

    private void randBrickCrash()
    {
        Brick brick = null;
        int rand;
        int wall;
        int cell;
        int row;
        do
        {
            rand = Random.Range( 0 , 80 );
            wall = rand / 20;
            cell = rand - 20 * wall;
            row = cell / 10;

            brick = brickList[wall * 30 + cell];
        }
        while ( brick == null || brick.isHit() );
        
        if ( row == 0 )
        {
            ZombieCreeper newZ = GameObject.Instantiate<ZombieCreeper>( Zombie2 );
            newZ.Init( brick );
            newZ.WhenDie = OnZombieDie;
        }
        else if ( row == 1 )
        {
            ZombieAttacker newZ = GameObject.Instantiate<ZombieAttacker>( Zombie );
            newZ.Init( brick , brickList[wall * 30 + cell - 10] );
            newZ.WhenDie = OnZombieDie;
        }
    }

    public void ResetHouse()
    {
        int child = transform.childCount;
        for (int i = 0; i < child; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        createHouse();
        timer = 0.0f;
        zombieNum = 0;
    }
}
