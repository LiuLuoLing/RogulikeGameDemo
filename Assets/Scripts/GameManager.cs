using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public int food = 12;
    public int level = 1;
    public bool isEnd = false;
    private bool sleepStep = true;

    public List<Enemy> enemys = new List<Enemy>();
    private Text foodText;
    private Text overText;
    private Player player;
    private MapManager map;

    private GameObject StartPanel;
    private Image StartBG;
    private Text startText;

    public float duration = 5f; // 颜色过渡的持续时间

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

        StartPanel = GameObject.Find("StartPanel");
        StartBG = StartPanel.GetComponent<Image>();
        startText = StartPanel.GetComponentInChildren<Text>();
        startText.text = level.ToString() + " Day";

        //初始化参数
        isEnd = false;
        enemys.Clear();

        StartCoroutine(StartPanelAnimator());
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

    IEnumerator StartPanelAnimator()
    {
        float elapsedTime = 0f;
        Color initialBGColor = StartBG.color;
        Color initialTextColor = startText.color;
        while (elapsedTime < duration)
        {
            StartBG.color = Color.Lerp(initialBGColor, Color.clear, elapsedTime / duration);
            startText.color = Color.Lerp(initialTextColor, Color.clear, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终颜色为完全透明
        StartBG.color = Color.clear;
        startText.color = Color.clear;
    }
}
