using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteRoom : RoomBase
{
    private GameObject portalMapIcon = null;

    private bool isEliteMonsterSpawn = false;

    [SerializeField]
    private GameObject MoveNextMapPortal;

    private void Start()
    {
        InstantiateMoveMapIcon();
        MoveNextMapPortal = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Maps/Magic_Circle_Move.prefab");
    }

    protected override void IsClear()
    {
        // TODO : ����Ʈ ���� ��� �� Ŭ���� ó��
        if(isEliteMonsterSpawn && EnemySpawnManager.Instance.curEnemies.Count == 0)
        {
            isClear = true;
            AssignMoveNextMapPortal();
            Door.Instance.OpenDoors();
        }
    }

    public void InstantiateMoveMapIcon()
    {
        Debug.Log("������ ����");
        portalMapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/PortalMapIcon.prefab");
        portalMapIcon.transform.position = transform.position;
        portalMapIcon.SetActive(false);
    }

    protected override void ShowIcon()
    {
        portalMapIcon.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player") && !isEliteMonsterSpawn)
        {
            isEliteMonsterSpawn = true;
            // TODO : ����Ʈ ���� ���� �� ����ǥ�� �� ����Ʈ ������ �߰�

            Door.Instance.CloseDoors();
            EnemySpawnManager.Instance.SpawnEliteMonster(transform);
        }
    }

    public void AssignMoveNextMapPortal()
    {
        Instantiate(MoveNextMapPortal, transform.position, Quaternion.identity);
    }
}
