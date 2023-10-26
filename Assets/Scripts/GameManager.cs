using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public int food = 10;
    public int level = 4;
    public bool isEnd = false;
    private bool sleepStep = true;

    public List<Enemy> enemys = new List<Enemy>();
    private Text foodText;
    private Text overText;
    private Player player;
    private MapManager map;

    private void Awake()
    {
        _instance = this;
        InitGame();
    }

    void InitGame()
    {
        //初始化地图
        map = GetComponent<MapManager>();
        map.InitMap();

        //初始化UI
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        overText = GameObject.Find("OverText").GetComponent<Text>();
        overText.gameObject.SetActive(false);
        UpdateFoodText(0);

        //初始化参数
        isEnd = false;
        enemys.Clear();  
    }

    void UpdateFoodText(int foodChange)
    {
        if (foodChange == 0)
        {
            foodText.text = "Food:" + food;
        }
        else
        {
            string str = foodChange > 0 ? "+" : "";
            foodText.text = str + foodChange + "  Food:" + food;
        }
    }

    /// <summary>
    /// 添加步数
    /// </summary>
    /// <param name="number">数量</param>
    public void AddFood(int number)
    {
        food += number;
        UpdateFoodText(number);
    }

    /// <summary>
    /// 减去步数
    /// </summary>
    /// <param name="number">数量</param>
    public void RemoveFood(int number)
    {
        food -= number;
        UpdateFoodText(-number);
        if (food <= 0)
        {
            overText.gameObject.SetActive(true);
        }
    }

    public void OnPlayeMove()
    {
        if (sleepStep)
        {
            sleepStep = false;
        }
        else
        {
            foreach (Enemy enemy in enemys)
            {
                enemy.Move();
            }
            sleepStep = true;
        }

        //判断是否到达出口
        if (player.targetPos == map.exitPos)
        {
            isEnd = true;
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    private void OnLevelWasLoaded(int Scenelevel)
    {
        level++;
        InitGame();
    }
}
