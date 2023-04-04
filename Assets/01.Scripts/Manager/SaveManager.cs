using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Debug = Rito.Debug;

public static class SaveManager
{
	private static string SAVE_PATH = Application.dataPath + "/Json/";
	private static string SAVE_FILENAME = "";

	/// <summary>
	/// ���� ������ ����
	/// </summary>
	public static void Save<T>(ref T userSaveData)
	{
		SAVE_FILENAME = typeof(T).FullName + ".json";

		if (!File.Exists(SAVE_PATH + SAVE_FILENAME))
		{
			Debug.Log("[SaveManager] �������� ����");
			Directory.CreateDirectory(SAVE_PATH);
		}
        else
        {
			Debug.Log("[SaveManager] �������� ����");
		}
		string jsonData = JsonUtility.ToJson(userSaveData, true);
		jsonData = Crypto.AESEncrypt128(jsonData);
		File.WriteAllText(SAVE_PATH + SAVE_FILENAME, jsonData, System.Text.Encoding.UTF8);
		Debug.Log("[SaveManager] ����Ϸ�");
		
	}

	/// <summary>
	/// ���� ������ �ҷ�����
	/// </summary>
	public static void Load<T>(ref T userSaveData)
	{
		SAVE_FILENAME = typeof(T).FullName + ".json";

		if (File.Exists(SAVE_PATH + SAVE_FILENAME))
		{
			string jsonData = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
			jsonData = Crypto.AESDecrypt128(jsonData);
			T saveData = JsonUtility.FromJson<T>(jsonData);
			userSaveData = saveData;
			Debug.Log("[SaveManager] �ε��Ϸ�");
		}
        else
        {
			Debug.Log($"[SaveManager] {SAVE_PATH + SAVE_FILENAME} ��� ����");
        }
	}

	/// <summary>
	/// ���̺��� ���� �ִ��� üũ
	/// </summary>
	/// <returns></returns>
	public static bool GetCheckBool()
	{
		return File.Exists(SAVE_PATH + "PlayerData.json");
	}
}