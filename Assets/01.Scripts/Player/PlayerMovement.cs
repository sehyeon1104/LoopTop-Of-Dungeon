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
    [SerializeField] float speed = 4.25f;

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
       
       if (isMove)
         Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);

        //Move(joystick.Direction);
    }
    public void Move(Vector2 inputVelocity)
    {
        if(!IsMove || GameManager.Instance.Player.playerBase.IsPDead)
        {
            return;
        }
        Vector2 VelocityVec = inputVelocity * speed;
        rb.velocity = VelocityVec;
        PlayerVisual.Instance.VelocityChange(direction.x);
        if (inputVelocity.x != 0 || inputVelocity.y != 0)
        {
            direction = inputVelocity;
        }
    }
}
