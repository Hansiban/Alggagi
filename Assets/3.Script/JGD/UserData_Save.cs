using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserData_Save : MonoBehaviour
{
    public PlayerData playerData;



    //저장?
    void SavePlayerDataJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);

        string path = Path.Combine(Application.dataPath, "playerData.Json");

        File.WriteAllText(path, jsonData);
    }
    //불러오기?
    private void LoadPlayerDataToJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.Json");
        string JsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(JsonData);
    }
}
