using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player MoveMent Class
public class PlayerMovement : MonoSingleton<PlayerMovement>
{
     private Joystick _joystick;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float speed;
    Rigidbody2D rb;
    Vector2 moveVec2;

    public void Move(Vector2 inputVelocity)
    {
        moveVec2 = new Vector2 (inputVelocity.x,inputVelocity.y) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVec2);

        if(moveVec2.sqrMagnitude == 0)
        {
            return;
        }

        if (inputVelocity.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }
}
