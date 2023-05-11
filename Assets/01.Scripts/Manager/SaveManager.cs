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
    /// 유저 데이터 저장
    /// </summary>
    public static void Save<T>(ref T userSaveData)
    {
        string SAVE_FILENAME = typeof(T).FullName + ".json";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        string jsonData = JsonUtility.ToJson(userSaveData, true);
        //jsonData = Crypto.AESEncrypt128(jsonData);
        File.WriteAllText(Path.Combine(SAVE_PATH, SAVE_FILENAME), jsonData, System.Text.Encoding.UTF8);
        Debug.Log($"[SaveManager] {typeof(T).ToString()} 저장완료");
    }

    /// <summary>
    /// 유저 데이터 불러오기
    /// </summary>
    public static void Load<T>(ref T userSaveData)
    {
        string SAVE_FILENAME = typeof(T).FullName + ".json";

        string filePath = Path.Combine(SAVE_PATH, SAVE_FILENAME);

        if (File.Exists(filePath))
        {
            // Debug.Log(filePath);
            string jsonData = File.ReadAllText(filePath);
            //jsonData = Crypto.AESDecrypt128(jsonData);
            T saveData = JsonUtility.FromJson<T>(jsonData);
            userSaveData = saveData;
            Debug.Log($"[SaveManager] {typeof(T).ToString()} 로딩완료");
        }
        else
        {
            Debug.Log($"[SaveManager] {filePath} 경로 없음");
        }
    }

    /// <summary>
    /// 세이브한 적이 있는지 체크
    /// </summary>
    /// <returns></returns>
    public static bool GetCheckDataBool(string FILENAME)
    {
        string SAVE_FILENAME = FILENAME + ".json";

        return File.Exists(Path.Combine(SAVE_PATH, SAVE_FILENAME));
    }

    public static void DeleteAllData()
    {
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(SAVE_PATH);
        foreach (System.IO.FileInfo File in di.GetFiles())
        {
            Debug.Log(File.Name + "제거");
            File.Delete();
        }

    }
}