using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private List<GameObject> outWalkArray;
    private List<GameObject> floorArray;
    private List<GameObject> wallArray;
    private List<GameObject> foodArray;
    private List<GameObject> enemyArray;
    private List<Vector2> positionList;
    private GameObject exitPrefab;
    private Transform outWalk;
    private Transform floor;
    private Transform walk;
    private Transform food;
    private Transform enemy;
    private GameManager gameManager;
    private int rows;
    private int clos;
    private int minCountWalk;
    private int maxCountWalk;

    private void Awake()
    {
        outWalkArray = new List<GameObject>();
        floorArray = new List<GameObject>();
        wallArray = new List<GameObject>();
        foodArray = new List<GameObject>();
        enemyArray = new List<GameObject>();
        rows = 10;
        clos = 10;
        minCountWalk = 2;
        maxCountWalk = 8;
        gameManager = this.GetComponent<GameManager>();
        positionList = new List<Vector2>();
        for (int i = 0; i < 3; i++)
        {
            outWalkArray.Add(Resources.Load("Prefabs/OutWalk" + (i + 1)) as GameObject);
        }
        for (int i = 0; i < 8; i++)
        {
            floorArray.Add(Resources.Load("Prefabs/Floor" + (i + 1)) as GameObject);
            wallArray.Add(Resources.Load("Prefabs/Walk" + (i + 1)) as GameObject);
        }
        foodArray.Add(Resources.Load("Prefabs/Food") as GameObject);
        foodArray.Add(Resources.Load("Prefabs/Suda") as GameObject);
        enemyArray.Add(Resources.Load("Prefabs/Enemy1") as GameObject);
        enemyArray.Add(Resources.Load("Prefabs/Enemy2") as GameObject);
        exitPrefab = Resources.Load("Prefabs/Exit") as GameObject;
    }

    void Start()
    {
        InitMap();
    }

    void Update()
    {

    }

    private void InitMap()
    {
        outWalk = new GameObject("OutWalks").transform;
        floor = new GameObject("Floors").transform;
        walk = new GameObject("Walls").transform;
        food = new GameObject("Foods").transform;
        enemy = new GameObject("Enemys").transform;
        //围墙和地板
        for (int x = 0; x < clos; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x == 0 || y == 0 || x == clos - 1 || y == rows - 1)
                {
                    int index = Random.Range(0, outWalkArray.Count);
                    GameObject.Instantiate(outWalkArray[index], new Vector3(x, y, 0), Quaternion.identity, outWalk);
                }
                else
                {
                    int index = Random.Range(0, floorArray.Count);
                    GameObject.Instantiate(floorArray[index], new Vector3(x, y, 0), Quaternion.identity, floor);
                }
            }
        }

        positionList.Clear();
        for (int x = 2; x < clos - 2; x++)
        {
            for (int y = 2; y < rows - 2; y++)
            {
                positionList.Add(new Vector2(x, y));
            }
        }

        //创建障碍物食物和敌人

        //创建障碍物
        int walkCount = Random.Range(minCountWalk, maxCountWalk + 1);
        InstantiteItems(walkCount, wallArray, walk);

        //创建食物
        int foodCount = Random.Range(1, gameManager.level * 2 + 1);
        InstantiteItems(foodCount, foodArray, food);

        //创建敌人
        int enemyCount = gameManager.level / 2;
        InstantiteItems(enemyCount, enemyArray, enemy);

        //创建出口
        GameObject exit = GameObject.Instantiate(exitPrefab, new Vector2(clos - 2, rows - 2), Quaternion.identity);
    }

    //生成位置
    private Vector2 RandomPosition()
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        return pos;
    }

    //生成预制体
    private GameObject RandomPrefab(List<GameObject> prefabs)
    {
        int index = Random.Range(0, prefabs.Count);
        return prefabs[index];
    }

    //地图上生成
    private void InstantiteItems(int count, List<GameObject> prefabs, Transform trans)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = RandomPosition();
            GameObject prefab = RandomPrefab(prefabs);
            GameObject game = GameObject.Instantiate(prefab, pos, Quaternion.identity, trans);
        }
    }
}
