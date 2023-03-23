using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoSingleton<SaveManager>
{
    private string SAVE_PATH = "";
    private string SAVE_FILENAME = "/SaveFile.json";

    [SerializeField] private PlayerData playerData = null;

    public PlayerData CurrentData { get { return playerData; } }

    private void Awake()
    {
        SAVE_PATH = Application.dataPath + "/Json";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
            File.WriteAllText(SAVE_PATH + SAVE_FILENAME, JsonUtility.ToJson(new PlayerData()), System.Text.Encoding.UTF8);
        }

        LoadFromJson();
    }

    private void Start()
    {
        SaveToJson();
        InvokeRepeating("SaveToJson", 1f, 60f);
    }

    [ContextMenu("LoadFromJson")]
    private void LoadFromJson()
    {
        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            string json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            // 복호화
            json = Crypto.AESDecrypt128(json);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Rito.Debug.Log("로딩완료");
        }
    }

    [ContextMenu("SaveToJson")]
    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(playerData, true);
        // 암호화
        json = Crypto.AESEncrypt128(json);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
        Rito.Debug.Log("저장완료");

    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }

}