using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 targetPos;
    private Transform player;
    private Rigidbody2D rb;

    private float speed = 5;

    void Start()
    {
        GameManager.Instance.enemys.Add(this);
        targetPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime));
    }

    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if (offset.magnitude < 1.1f)
        {
            //¹¥»÷
        }
        else
        {
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                //°´ÕÕyÖáÒÆ¶¯
                if (offset.y > 0)
                {
                    y = 1;
                }
                else
                {
                    y = -1;
                }
            }
            else
            {
                //°´ÕÕxÖáÒÆ¶¯
                if (offset.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            targetPos += new Vector2(x, y);
        }
    }
}
