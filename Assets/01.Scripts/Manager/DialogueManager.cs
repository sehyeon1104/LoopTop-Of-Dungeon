using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Text;
using UnityEngine.UI;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    private GameObject dialogueUI = null;

    [SerializeField]
    private GameObject dialoguePanel = null;
    [SerializeField]
    private TextMeshProUGUI contentTmp = null;
    [SerializeField]
    private float waitTime = 2f;
    [SerializeField]
    private Button acceptBtn = null;    // 선택지가 있을 경우 한정
    [SerializeField]
    private Button refuseBtn = null;    // 선택지가 있을 경우 한정

    private TextMeshProUGUI acceptBtnTmp = null;
    private TextMeshProUGUI refuseBtnTmp = null;

    private Vector3 dialoguePos;

    private WaitForSeconds waitForSeconds;

    private string[] contentArr;

    private BoxCollider2D col = null;

    public bool isDialogue { get; private set; } = false;

    private void Awake()
    {
        Init();
        waitForSeconds = new WaitForSeconds(waitTime);
    }

    private void Init()
    {
        if(!transform.Find("DialogueUI"))
        {
            dialogueUI = Managers.Resource.Instantiate("Assets/03.Prefabs/UI/DialogueUI.prefab");
        }
        else
        {
            dialogueUI = transform.Find("DialogueUI").gameObject;
        }

        acceptBtn = dialogueUI.transform.Find("DialoguePanel/AcceptBtn").GetComponent<Button>();
        acceptBtn.gameObject.SetActive(false);
        acceptBtnTmp = acceptBtn.GetComponent<TextMeshProUGUI>();
        refuseBtn = dialogueUI.transform.Find("DialoguePanel/RefuseBtn").GetComponent<Button>();
        refuseBtn.gameObject.SetActive(false);
        refuseBtnTmp = refuseBtn.GetComponent<TextMeshProUGUI>();

        refuseBtn.onClick.AddListener(ToggleDialoguePanel);

        dialogueUI.SetActive(false);
    }

    private void Start()
    {
        dialoguePanel = dialogueUI.transform.Find("DialoguePanel").gameObject;
        contentTmp = dialogueUI.transform.Find("DialoguePanel/Content").GetComponent<TextMeshProUGUI>();
        //DialoguePanel = UIManager.Instance.playerUI.transform.Find("DialoguePanel").gameObject;
        //contentTmp = UIManager.Instance.playerUI.transform.Find("Content").gameObject.GetComponent<TextMeshProUGUI>();

        dialoguePanel.gameObject.SetActive(false);
    }

    public void ToggleDialoguePanel()
    {
        if(!dialoguePanel.gameObject.activeSelf)
            UIManager.Instance.PushPanel(dialoguePanel);
        else
        {
            UIManager.Instance.PopPanel();
            isDialogue = false;
        }
        //dialoguePanel.gameObject.SetActive(!dialoguePanel.gameObject.activeSelf);
    }

    public void SetContentNPos(string contents, GameObject obj)
    {
        isDialogue = true;
        col = obj.GetComponent<BoxCollider2D>();

        if (col == null)
        {
            Debug.LogWarning("BoxCollider is null");
            return;
        }
        contentArr = new string[contents.Length];
        for (int i = 0; i < contents.Length; ++i)
        {
            contentArr[i] += contents[i];
        }

        ToggleDialoguePanel();
        dialoguePos = Camera.main.WorldToScreenPoint(new Vector3(obj.transform.position.x, obj.transform.position.y + col.size.y / 2));

        dialoguePanel.transform.position = dialoguePos;

        StartCoroutine(IEStartDialogue());
    }

    public void SetContentNPos(string[] contents, GameObject obj)
    {
        isDialogue = true;
        col = obj.GetComponent<BoxCollider2D>();

        if(col == null)
        {
            Debug.LogWarning("BoxCollider is null");
            return;
        }

        contentArr = new string[contents.Length];
        for(int i = 0; i < contents.Length; ++i)
        {
            contentArr[i] += contents[i];
        }

        ToggleDialoguePanel();
        dialoguePos = Camera.main.WorldToScreenPoint(new Vector3(obj.transform.position.x, obj.transform.position.y + col.size.y / 2));

        dialoguePanel.transform.position = dialoguePos;

        StartCoroutine(IEStartDialogue());
    }

    public void ToggleNSetBtnsText(string _acceptText, string _refusceText)
    {
        acceptBtnTmp.text = _acceptText;
        acceptBtn.gameObject.SetActive(true);

        refuseBtnTmp.text = _refusceText;
        refuseBtn.gameObject.SetActive(true);
    }

    public IEnumerator IEStartDialogue()
    {
        contentTmp.SetText("");

        for(int i = 0; i < contentArr.Length; ++i)
        {
            contentTmp.DOText($"{contentArr[i]}", 1.5f);
            yield return waitForSeconds;
            yield return new WaitUntil(() => Input.anyKeyDown);
            contentTmp.SetText("");
        }

        //ToggleDialoguePanel();
        //isDialogue = false;
        yield break;
    }
}
