using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    [SerializeField] private Joystick _joystick;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float speed;
    Rigidbody2D rb;
    Vector2 moveVec2;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

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

        if(x < 0)
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
        if (collision.CompareTag("EntranceDoor"))
        {
            MapManager.Instance.MoveMap(collision.name);
        }
    }
}
