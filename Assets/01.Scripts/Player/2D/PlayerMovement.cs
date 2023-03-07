using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player MoveMent Class
public partial class Player
{
    [SerializeField] private Joystick _joystick;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float speed;
    Rigidbody2D rb;
    Vector2 moveVec2;

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        float x = _joystick.Horizontal;
        float y = _joystick.Vertical; 

        moveVec2 = new Vector2 (x, y) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVec2);

        if(moveVec2.sqrMagnitude == 0)
        {
            return;
        }

        if (x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // юс╫ц
        //if (collision.CompareTag("EntranceDoor") && Boss.Instance.isBDead)
        //{
        //    MapManager.Instance.MoveMap(collision.name);
        //}
    }
}
