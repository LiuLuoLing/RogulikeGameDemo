using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    public float restTime = 1;

    private float restTimer = 0;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public Vector2 targetPos = new Vector2(1, 1);
    private Animator animator;
    private SpriteRenderer sprite;
    private bool showButtonUI;

    private bool isAndroid;
    private float h;
    private float v;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        isAndroid = Run();
    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime));

        if (GameManager.Instance.food <= 0 || GameManager.Instance.isEnd)
            return;

        restTimer += Time.fixedDeltaTime;
        if (restTimer < restTime)
            return;

        h = 0;
        v = 0;

        if (isAndroid)
        {
            //安卓平台
            h = InputUI.Instance.SetH();
            v = InputUI.Instance.SetV();
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        if (h > 0)
            v = 0;

        if (h != 0 || v != 0)
        {
            GameManager.Instance.RemoveFood(1);
            //切换方向
            if (h < 0)
                sprite.flipX = true;
            if (h > 0)
                sprite.flipX = false;

            //检测前方物体
            boxCollider.enabled = false;
            RaycastHit2D hit2D = Physics2D.Linecast(targetPos, targetPos + new Vector2(h, v));
            boxCollider.enabled = true;

            if (hit2D.transform == null)
            {
                targetPos += new Vector2(h, v);
            }
            else
            {
                switch (hit2D.collider.tag)
                {
                    case "OutWalk":
                        break;
                    case "Walk":
                        animator.SetTrigger("Attack");
                        hit2D.collider.SendMessage("TakeAttack");
                        break;
                    case "Food":
                        GameManager.Instance.AddFood(10);
                        targetPos += new Vector2(h, v);
                        Destroy(hit2D.transform.gameObject);
                        break;
                    case "Suda":
                        GameManager.Instance.AddFood(15);
                        targetPos += new Vector2(h, v);
                        Destroy(hit2D.transform.gameObject);
                        break;
                    case "Enemy":
                        break;
                }
            }
            GameManager.Instance.OnPlayeMove();
            restTimer = 0;
        }

    }

    private void TakeAttack(int damage)
    {
        GameManager.Instance.RemoveFood(damage);
        animator.SetTrigger("Damage");
    }

    //运行平台
    public bool Run()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                showButtonUI = false;
                break;
            case RuntimePlatform.WindowsEditor:
                showButtonUI = false;
                break;
            case RuntimePlatform.Android:
                showButtonUI = true;
                break;
        }
        return showButtonUI;
    }


}
