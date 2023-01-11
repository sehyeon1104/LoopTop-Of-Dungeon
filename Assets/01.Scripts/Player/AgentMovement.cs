using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    Rigidbody2D playRigid;
    private void Awake()
    {
        playRigid= GetComponent<Rigidbody2D>();
    }
    void MoveAgent(Vector2 inputMove)
    {

    }
}
