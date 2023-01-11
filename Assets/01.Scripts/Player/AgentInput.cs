using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour,IAgentInput
{

    [field:SerializeField] public UnityEvent Attack { get; set; }
    [field:SerializeField] public UnityEvent SkillFirst { get; set; }
    [field:SerializeField] public UnityEvent<Vector2> MovementInput { get; set; }
  

    // Update is called once per frame
    void Update()
    {
        GetMovementInputMove();
    }
    void GetMovementInputMove()
    {
        MovementInput.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }
}
