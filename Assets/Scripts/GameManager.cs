using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private int food = 10;
    private bool sleepStep = true;
    public int level = 4;

    public List<Enemy> enemys = new List<Enemy>();
    private Text foodText;

    private void Awake()
    {
        _instance = this;
        InitGame();
    }

    void InitGame()
    {
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText(0);
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
    }
}
