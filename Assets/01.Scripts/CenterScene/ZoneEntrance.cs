using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneEntrance : MonoBehaviour
{
    private Define.MapTypeFlag mapTypeFlag;

    private bool isLoadScene = false;
    Define.Scene sceneType;

    private bool isInteraction = false;

    [SerializeField]
    private TextMeshProUGUI mapNameTMP = null;

    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isLoadScene = false;
        Init();
    }

    private void Init()
    {
        spriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Portal/{gameObject.name}PortalSprite.png");
        ToggleMapNameTMP();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ToggleMapNameTMP();

            GameManager.Instance.SetMapTypeFlag(mapTypeFlag);

            if (!isInteraction)
            {
                InteractionPlayer();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (UIManager.Instance.IsActiveAttackBtn())
            {
                return;
            }
            else
            {
                UIManager.Instance.RotateAttackButton();
            }
            isInteraction = false;
            ToggleMapNameTMP();
        }
    }

    public void MoveNextStage()
    {
        if (!isLoadScene)
        {
            isLoadScene = true;

            GameManager.Instance.StageMoveCount++;

            if (GameManager.Instance.StageMoveCount == 0 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.BossScene;
            }
            else if (GameManager.Instance.StageMoveCount < 3 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.StageScene;
            }
            else if (GameManager.Instance.sceneType == Define.Scene.BossScene)
            {
                sceneType = Define.Scene.CenterScene;
            }
            GameManager.Instance.SetSceneType(sceneType);
            GameManager.Instance.SaveData();

            Fade.Instance.FadeInAndLoadScene(sceneType);
            //Managers.Scene.LoadScene(sceneType);

        }
    }

    public void InteractionPlayer()
    {
        isInteraction = true;

        mapTypeFlag = gameObject.name switch
        {
            "Ghost" => Define.MapTypeFlag.Ghost,
            "LavaSlime" => Define.MapTypeFlag.LavaSlime,
            "Electricity" => Define.MapTypeFlag.Electricity,
            "Werewolf" => Define.MapTypeFlag.Werewolf,
            "Lizard" => Define.MapTypeFlag.Lizard,
            "Power" => Define.MapTypeFlag.Power,
            _ => Define.MapTypeFlag.Default,
        };

        UIManager.Instance.RotateInteractionButton();

        ToggleMapNameTMP();

        UIManager.Instance.GetInteractionButton().onClick.RemoveListener(MoveNextStage);
        UIManager.Instance.GetInteractionButton().onClick.AddListener(MoveNextStage);
    }

    public void ToggleMapNameTMP()
    {
        mapNameTMP.gameObject.SetActive(isInteraction);
        mapNameTMP.SetText(gameObject.name);
    }
}
