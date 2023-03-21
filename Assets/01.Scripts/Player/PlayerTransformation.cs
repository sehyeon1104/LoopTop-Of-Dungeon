using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoSingleton<PlayerTransformation> 
{
    [Space]
    [Header("변신")]
    PlayerBase playerBase;
    public List<PlayerSkillData> playerTransformDataSOArr; // 모든 변신 데이터
    [field: SerializeField]
    public PlayerSkillData playerTransformDataSO;     // 현재 변신 데이터
    private void Awake()
    {
        playerBase = GameManager.Instance.Player.playerBase;
        playerTransformDataSO = Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset");
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));

    }
    private void Start()
    {
        playerTransformDataSO = playerTransformDataSOArr[(int)playerBase.PlayerTransformTypeFlag];
    }

    public void TransformAilen(int indexAilen)
    {
        playerTransformDataSO = playerTransformDataSOArr[indexAilen];
    }

}
