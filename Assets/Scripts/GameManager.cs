using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private int food = 10;
    private bool sleepStep = true;

    public static GameManager Instance { get { return _instance; } }
    public int level = 4;
    public List<Enemy> enemys = new List<Enemy>();

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// 添加步数
    /// </summary>
    /// <param name="number">数量</param>
    public void AddFood(int number)
    {
        food += number;
    }

    /// <summary>
    /// 减去步数
    /// </summary>
    /// <param name="number">数量</param>
    public void RemoveFood(int number)
    {
        food -= number;
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
