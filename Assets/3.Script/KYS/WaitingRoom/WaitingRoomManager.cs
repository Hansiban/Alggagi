using LitJson;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomManager : MonoBehaviour
{
    public void Btn_TempGoToMainMenu(string sceneName) => SceneManager.LoadScene(sceneName);

    private MyNetworkRoomManager manager;


    private Type type;
    private string ipAddress;

    private void Start()
    {
        type = License_type();

        manager = FindObjectOfType<MyNetworkRoomManager>();

        Debug.Log(SceneManager.GetActiveScene().name + "이 "+ type .ToString()+ " 쪽에서 로드됐습니다");

        Debug.Log("ipAddress " + ipAddress);
        manager.networkAddress = ipAddress;

        if (type.Equals(Type.Client))
            manager.StartClient();
        else if (type.Equals(Type.Server))
            manager.StartServer();
    }


    private Type License_type()
    {
        try
        {
            string Json_string = File.ReadAllText(Application.dataPath + "/License" + "/License.json");
            JsonData itemdata = JsonMapper.ToObject(Json_string);

            string string_type = itemdata[0]["Lisence"].ToString();
            ipAddress = itemdata[0]["Server_IP"].ToString();
            return (Type)Enum.Parse(typeof(Type), string_type);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return Type.Empty;
        }
    }

}
