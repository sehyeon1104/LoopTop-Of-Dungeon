using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Player MoveMent Class
public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    [SerializeField]
    Joystick _joystick;
    SpriteRenderer _spriteRenderer;
    
    [Range(1,5)] [SerializeField] float speed = 3;
    Rigidbody2D rb;

    public UnityEvent<float> VelocityChange;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Move(Vector2 inputVelocity)
    {
        if(inputVelocity.x ==0 && inputVelocity.y == 0)
        {
            return;
        }
        else
        {
            VelocityChange.Invoke(inputVelocity.x);
        }
        Vector2 VelocityVec = inputVelocity * speed * Time.deltaTime;
        rb.MovePosition(rb.position + VelocityVec);
    }

}
