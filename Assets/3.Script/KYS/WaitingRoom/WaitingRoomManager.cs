using LitJson;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomManager : MonoBehaviour
{
    public void Btn_TempGoToMainMenu(string sceneName) => SceneManager.LoadScene(sceneName);

    private MyNetworkRoomManager manager;

    private Type type;

    private void Start()
    {
        type = License_type();

        manager = FindObjectOfType<MyNetworkRoomManager>();

        Debug.Log(SceneManager.GetActiveScene().name + "이 클라 쪽에서 로드됐습니다");

        Debug.Log("networkAddress before" + manager.networkAddress);
        manager.networkAddress = "172.30.1.32";
        Debug.Log("networkAddress after" + manager.networkAddress);

        if (type.Equals(Type.Client))
            manager.StartClient();
    }


    private Type License_type()
    {
        try
        {
            string Json_string = File.ReadAllText(Application.dataPath + "/License.json");
            JsonData itemdata = JsonMapper.ToObject(Json_string);

            string string_type = itemdata[0]["Lisence"].ToString();
            manager.networkAddress = itemdata[0]["Server_IP"].ToString();
            type = (Type)Enum.Parse(typeof(Type), string_type);

            return type;

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return Type.Empty;
        }
    }

}
