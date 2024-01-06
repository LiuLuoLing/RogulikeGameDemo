using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 targetPos;
    private Transform player;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private float speed = 5;
    public int damage = 10;

    void Start()
    {
        GameManager.Instance.enemys.Add(this);
        targetPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (gameObject.name == "Enemy1(Clone)")
        {
            damage = 10;
        }
        else
        {
            damage = 20;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime));
    }

    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if (offset.magnitude < 1.1f)
        {
            //攻击
            animator.SetTrigger("Attack");
            player.SendMessage("TakeAttack", damage);
        }
        else
        {
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                //按照y轴移动
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
                //按照x轴移动
                if (offset.x > 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }

            //检测前方物体
            boxCollider.enabled = false;
            RaycastHit2D hit2D = Physics2D.Linecast(targetPos, targetPos + new Vector2(x, y));
            boxCollider.enabled = true;

            if (hit2D.transform == null || hit2D.collider.tag == "Suda" || hit2D.collider.tag == "Food")
            {
                targetPos += new Vector2(x, y);
            }
            else
            {

            }
        }
    }
}
