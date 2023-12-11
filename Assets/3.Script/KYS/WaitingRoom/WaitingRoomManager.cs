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

    private void Start()
    {

        string path = Application.dataPath + "/License";

        if (!File.Exists(path)) //폴더 검사
        {
            Directory.CreateDirectory(path);
        }
        if (!File.Exists(path + "/License.json")) //파일 검사
        {
            List<Item> item = new List<Item>();
            item.Add(new Item("1", "172.30.1.32", "8596"));

            JsonData data = JsonMapper.ToJson(item);
            File.WriteAllText(path + "/License.json", data.ToString());
        }

        string Json_string = File.ReadAllText(Application.dataPath + "/License" + "/License.json");
        JsonData itemdata = JsonMapper.ToObject(Json_string);
        
        
        string ipAddress = itemdata[0]["Server_IP"].ToString();
        MyNetworkRoomManager.singleton.networkAddress = ipAddress;


        string string_type = itemdata[0]["Lisence"].ToString();

        Type myType = (Type)Enum.Parse(typeof(Type), string_type);

        if (myType.Equals(Type.Client))
            MyNetworkRoomManager.singleton.StartClient();
        else if (myType.Equals(Type.Server))
            MyNetworkRoomManager.singleton.StartServer();
    }
}
