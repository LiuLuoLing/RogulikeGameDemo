using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUI : MonoBehaviour
{
    private static InputUI _instance;
    public static InputUI Instance { get { return _instance; } }

    private Button left;
    private Button right;
    private Button up;
    private Button down;

    private float h;
    private float v;
    void Awake()
    {
        _instance = this;
        left = transform.Find("Left").GetComponent<Button>();
        right = transform.Find("Right").GetComponent<Button>();
        up = transform.Find("Up").GetComponent<Button>();
        down = transform.Find("Down").GetComponent<Button>();
        h = 0;
        v = 0;
    }

    void Start()
    {
        left.onClick.AddListener(() =>
        {
            h = -1;
            v = 0;
            StartCoroutine(ResetDirection());
        });
        right.onClick.AddListener(() =>
        {
            h = 1;
            v = 0;
            StartCoroutine(ResetDirection());
        });
        down.onClick.AddListener(() =>
        {
            v = -1;
            h = 0;
            StartCoroutine(ResetDirection());
        });
        up.onClick.AddListener(() =>
        {
            v = 1;
            h = 0;
            StartCoroutine(ResetDirection());
        });
    }

    IEnumerator ResetDirection()
    {
        yield return new WaitForSeconds(0.5f);
        h = 0;
        v = 0;
    }

    public float SetH()
    {
        return h;
    }

    public float SetV()
    {
        return v;
    }
}
