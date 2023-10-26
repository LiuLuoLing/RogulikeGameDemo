using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private GameObject gameManager;

    private void Awake()
    {
        gameManager = Resources.Load("Prefabs/GameManager") as GameObject;
        if (GameManager.Instance == null)
            Instantiate(gameManager);
    }
}
