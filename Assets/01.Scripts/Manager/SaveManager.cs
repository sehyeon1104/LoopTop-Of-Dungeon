using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Debug = Rito.Debug;

public static class SaveManager
{
    // PC
    private static string SAVE_PATH = Path.Combine(Application.dataPath, "Json/");
    // Android
    //private static string SAVE_PATH = Path.Combine(Application.persistentDataPath, "Json/");

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public static void Save<T>(ref T userSaveData)
    {
        string SAVE_FILENAME = typeof(T).FullName + ".json";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        string jsonData = JsonUtility.ToJson(userSaveData, true);
        jsonData = Crypto.AESEncrypt128(jsonData);
        File.WriteAllText(Path.Combine(SAVE_PATH, SAVE_FILENAME), jsonData, System.Text.Encoding.UTF8);
        Debug.Log("[SaveManager] ����Ϸ�");
    }

    /// <summary>
    /// ���� ������ �ҷ�����
    /// </summary>
    public static void Load<T>(ref T userSaveData)
    {
        string SAVE_FILENAME = typeof(T).FullName + ".json";

        string filePath = Path.Combine(SAVE_PATH, SAVE_FILENAME);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            jsonData = Crypto.AESDecrypt128(jsonData);
            T saveData = JsonUtility.FromJson<T>(jsonData);
            userSaveData = saveData;
            Debug.Log("[SaveManager] �ε��Ϸ�");
        }
        else
        {
            Debug.Log($"[SaveManager] {filePath} ��� ����");
        }
    }

    /// <summary>
    /// ���̺��� ���� �ִ��� üũ
    /// </summary>
    /// <returns></returns>
    public static bool GetCheckPlayerDataBool()
    {
        string SAVE_FILENAME = "PlayerData.json";

        return File.Exists(Path.Combine(SAVE_PATH, SAVE_FILENAME));
    }
    public static bool GetCheckGameDataBool()
    {
        string SAVE_FILENAME = "GameData.json";

        return File.Exists(Path.Combine(SAVE_PATH, SAVE_FILENAME));
    }
}