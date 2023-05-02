using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    bool isMove = true;
    public bool IsMove
    {
        get => isMove;
        set
        {
            if (value == false)
                rb.velocity = Vector2.zero;
            isMove = value;
        }
    }
    public Joystick joystick { private set; get; }
    public bool IsControl { get; set; } = true;
    Rigidbody2D rb;
    private float x;
    private float y;
    Vector2 direction = Vector2.zero;
    public Vector2 Direction => direction;
    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
       if (isMove && IsControl)
         Move((new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))+ joystick.Direction).normalized);
    }
    public void Move(Vector2 inputVelocity)
    {
        if(!IsMove || GameManager.Instance.Player.playerBase.IsPDead)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        rb.velocity = inputVelocity * GameManager.Instance.Player.playerBase.MoveSpeed;
        if (inputVelocity.x != 0 || inputVelocity.y != 0)
        {
            PlayerVisual.Instance.VelocityChange(direction.x);
            direction = inputVelocity;
        }
    }
}
