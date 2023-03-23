using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Player MoveMent Class
public class PlayerMovement : MonoBehaviour
{
    Joystick joystick;
    [Range(1,5)] [SerializeField] float speed = 3;
    Rigidbody2D rb;
    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        Move(new Vector2(joystick.Horizontal, joystick.Vertical));
    }
    public void Move(Vector2 inputVelocity)
    {
        if(inputVelocity.x ==0 && inputVelocity.y == 0)
        {
            return;
        }
        else
        {
            PlayerVisual.Instance.VelocityChange(inputVelocity.x);
        }
        Vector2 VelocityVec = inputVelocity * speed * Time.deltaTime;
        rb.MovePosition(rb.position + VelocityVec);
        
    }

}
