using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public UnityEvent<Vector3> EnemyDeadRelatedItemEffects { get; private set; } = new UnityEvent<Vector3>();
    public UnityEvent<Vector3> EnemyDamagedRelatedItemEffects { get; private set; } = new UnityEvent<Vector3>();
}
