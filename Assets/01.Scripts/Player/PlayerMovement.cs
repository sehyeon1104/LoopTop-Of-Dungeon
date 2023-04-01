using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    public Joystick joystick { private set; get; }
    [Range(1,5)] [SerializeField] float speed = 3;
    Rigidbody2D rb;

    private float x;
    private float y;

    public Vector2 GetVector => new Vector2(x, y);
    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        Move(new Vector2(x, y));
        Move(joystick.Direction);
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
