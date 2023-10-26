using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    private int hp = 2;
    public Sprite attackSprite;

    private void TakeAttack()
    {
        hp--;
        GetComponent<SpriteRenderer>().sprite = attackSprite;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
