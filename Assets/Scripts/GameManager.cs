using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private GameObject startPanel;
    private Image startBG;
    private Text startText;

    private float duration = 4f; // 颜色过渡的持续时间

    private GameObject showInputPanel;
    private bool isAndroid;

    private Button restartBtn;
    private Button exitBtn;

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
        if (overText != null)
            overText.gameObject.SetActive(false);
        UpdateFoodText(0);

        startPanel = GameObject.Find("StartPanel");
        startBG = startPanel.GetComponent<Image>();
        startText = startPanel.GetComponentInChildren<Text>();
        startText.text = level.ToString() + " Day";

        //初始化参数
        isEnd = false;
        enemys.Clear();

        showInputPanel = GameObject.Find("Input");
        if (showInputPanel != null)
            showInputPanel.SetActive(false);

        isAndroid = player.Run();

        StartCoroutine(StartPanelAnimator());

        restartBtn = overText.transform.Find("Restart").GetComponent<Button>();
        exitBtn = overText.transform.Find("Exit").GetComponent<Button>();

        Init();
    }

    void Init()
    {
        //重新开始游戏
        restartBtn.onClick.AddListener(Restart);

        //退出游戏
        exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
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
            startPanel.gameObject.SetActive(false);
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
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
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
        Color initialBGColor = startBG.color;
        Color initialTextColor = startText.color;
        while (elapsedTime < duration)
        {
            startBG.color = Color.Lerp(initialBGColor, Color.clear, elapsedTime / duration);
            startText.color = Color.Lerp(initialTextColor, Color.clear, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终颜色为完全透明
        startBG.color = Color.clear;
        startText.color = Color.clear;

        if (isAndroid)
            showInputPanel.SetActive(true);
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    void Restart()
    {
        food = 12;
        level = 0;
        isEnd = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
