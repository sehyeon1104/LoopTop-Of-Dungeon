using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public UnityEvent EnemyDeadRelatedItemEffects { get; private set; }
}
