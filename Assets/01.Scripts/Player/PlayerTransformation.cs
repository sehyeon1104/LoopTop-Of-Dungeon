using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoSingleton<PlayerTransformation> 
{
    [Space]
    [Header("����")]
    PlayerBase playerBase;
    public List<PlayerSkillData> playerTransformDataSOArr; // ��� ���� ������
    [field: SerializeField]
    public PlayerSkillData playerTransformDataSO;     // ���� ���� ������
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
