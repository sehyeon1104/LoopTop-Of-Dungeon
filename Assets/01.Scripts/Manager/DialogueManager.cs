using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Text;
using UnityEngine.UI;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    [SerializeField]
    private GameObject DialoguePanel = null;
    [SerializeField]
    private TextMeshProUGUI contentTmp = null;
    [SerializeField]
    private float waitTime = 3f;

    private Vector3 dialoguePos;

    private WaitForSeconds waitForSeconds;

    private string[] contentArr;

    private string[] asdsa = new string[3];

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(waitTime);
    }

    private void Start()
    {
        asdsa[0] = "안녕하세요!";
        asdsa[1] = "반갑습니다!";
        asdsa[2] = "당신은 이제..\n싸우시면 됩니다!!";
        DialoguePanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleDialoguePanel();
            SetContentNPos(asdsa, GameManager.Instance.Player.transform.position);
        }
    }

    public void ToggleDialoguePanel()
    {
        DialoguePanel.gameObject.SetActive(!DialoguePanel.gameObject.activeSelf);
    }

    public void SetContentNPos(string[] contents, Vector3 pos)
    {
        contentArr = new string[contents.Length];
        for(int i = 0; i < contents.Length; ++i)
        {
            contentArr[i] += contents[i];
        }

        dialoguePos = Camera.main.WorldToScreenPoint(pos) + Vector3.up * 100;

        DialoguePanel.transform.position = dialoguePos;

        StartCoroutine(IEStartDialogue());
    }

    public IEnumerator IEStartDialogue()
    {
        for(int i = 0; i < contentArr.Length; ++i)
        {
            contentTmp.DOText($"{contentArr[i]}", 2f);
            yield return waitForSeconds;
            contentTmp.SetText("");
        }

        ToggleDialoguePanel();

        yield break;
    }
}
