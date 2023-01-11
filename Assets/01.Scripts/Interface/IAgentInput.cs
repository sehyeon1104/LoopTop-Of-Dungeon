using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAgentInput
{
    UnityEvent Attack { get; set; }
    UnityEvent SkillFirst { get; set; }
    UnityEvent<Vector2> MovementInput { get; set; }
}
