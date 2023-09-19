using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteRoom : RoomBase
{
    private GameObject portalMapIcon = null;

    private bool isEliteMonsterSpawn = false;

    [SerializeField]
    private GameObject MoveNextMapPortal;

    protected override void Awake()
    {
        base.Awake();
        InstantiateMoveMapIcon();
        MoveNextMapPortal = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Maps/Magic_Circle_Move.prefab");
    }

    public void InstantiateMoveMapIcon()
    {
        //Debug.Log("아이콘 생성");
        portalMapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/PortalMapIcon.prefab");
        portalMapIcon.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 1.5f);
        portalMapIcon.SetActive(false);
    }

    protected override void ShowIcon()
    {
        base.ShowIcon();
        portalMapIcon.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player") && !isEliteMonsterSpawn)
        {
            isEliteMonsterSpawn = true;
            // TODO : 엘리트 몬스터 등장 전 위험표시 및 엘리트 몬스터전 추가

            // Door.Instance.CloseDoors();
            EnemySpawnManager.Instance.SpawnEliteMonster(transform);
            StartCoroutine(CheckClear());
        }
    }

    private IEnumerator CheckClear()
    {
        yield return new WaitUntil(() => EnemySpawnManager.Instance.curEnemies.Count == 0 && isEliteMonsterSpawn);
        IsClear();
    }

    protected override void IsClear()
    {
        if (isEliteMonsterSpawn && EnemySpawnManager.Instance.curEnemies.Count == 0)
        {
            isClear = true;
            AssignMoveNextMapPortal();
            // Door.Instance.OpenDoors();
        }
    }

    public void AssignMoveNextMapPortal()
    {
        Instantiate(MoveNextMapPortal, transform.position, Quaternion.identity);
        StageManager.Instance.InstantiateChest(transform.position + Vector3.down * 3.5f, Define.ChestRating.Epic);
    }
}
