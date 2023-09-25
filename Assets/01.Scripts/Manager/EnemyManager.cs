using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public UnityEvent EnemyDeadRelatedItemEffects { get; private set; } = new UnityEvent();
    public UnityEvent<Vector3> EnemyDamagedRelatedItemEffects { get; private set; } = new UnityEvent<Vector3>();
}
