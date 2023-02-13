using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAgent
{
    // public int Hp { get;  set; } 
    public UnityEvent GetHit { get; set; }
    public UnityEvent OnDie { get; set; }
}
