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
	/// 유저 데이터 저장
	/// </summary>
	public static void Save<T>(ref T userSaveData)
	{
		SAVE_FILENAME = typeof(T).FullName + ".json";

		if (!File.Exists(SAVE_PATH + SAVE_FILENAME))
		{
			Debug.Log("[SaveManager] 저장파일 없음");
			Directory.CreateDirectory(SAVE_PATH);
		}
        else
        {
			Debug.Log("[SaveManager] 저장파일 있음");
		}
		string jsonData = JsonUtility.ToJson(userSaveData, true);
		jsonData = Crypto.AESEncrypt128(jsonData);
		File.WriteAllText(SAVE_PATH + SAVE_FILENAME, jsonData, System.Text.Encoding.UTF8);
		Debug.Log("[SaveManager] 저장완료");
		
	}

	/// <summary>
	/// 유저 데이터 불러오기
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
			Debug.Log("[SaveManager] 로딩완료");
		}
        else
        {
			Debug.Log($"[SaveManager] {SAVE_PATH + SAVE_FILENAME} 경로 없음");
        }
	}

	/// <summary>
	/// 세이브한 적이 있는지 체크
	/// </summary>
	/// <returns></returns>
	public static bool GetCheckBool()
	{
		return File.Exists(SAVE_PATH + "PlayerData.json");
	}
}