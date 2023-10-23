using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public UnityEvent PrePlayerTransformChangeEffects = new UnityEvent();
    public UnityEvent PostPlayerTransformChangeEffects = new UnityEvent();
}
