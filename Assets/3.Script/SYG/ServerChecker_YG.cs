using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using kcp2k;
using LitJson;
using System;
using System.IO;

public enum Type
{
    Empty = 0,
    Server,
    Client
}

public class Item
{
    public string Lisence;
    public string Server_IP;
    public string Port;

    public Item(string L_index, string IPvalue, string port)
    {
        Lisence = L_index;
        Server_IP = IPvalue;
        Port = port;
    }

}

public class ServerChecker_YG : MonoBehaviour
{
    public Type type;
    private NetworkLoginManager_KYS manager;
    private KcpTransport kcp;

    private string Path = string.Empty;
    public string Server_Ip { get; private set; }
    public string Server_Port { get; private set; }

    private void Awake()
    {
        if (Path.Equals(string.Empty))
        {
            Path = Application.dataPath + "/License";
        }
        if (!File.Exists(Path)) //폴더 검사
        {
            Directory.CreateDirectory(Path);
        }
        if (!File.Exists(Path + "/License.json")) //파일 검사
        {
            Default_Data(Path);
        }
        manager = GetComponent<NetworkLoginManager_KYS>();
        kcp = (KcpTransport)manager.transport;
    }

    private void Default_Data(string path)
    {
        List<Item> item = new List<Item>();
        item.Add(new Item("1", "43.200.181.67", "7777"));

        JsonData data = JsonMapper.ToJson(item);
        File.WriteAllText(path + "/License.json", data.ToString());
    }

    private Type License_type()
    {
        Type type = Type.Empty;

        try
        {
            string Json_string = File.ReadAllText(Path + "/License.json");
            JsonData itemdata = JsonMapper.ToObject(Json_string);

            string string_type = itemdata[0]["Lisence"].ToString();
            string str_severIP = itemdata[0]["Server_IP"].ToString();
            string str_Port = itemdata[0]["Port"].ToString();
            Server_Ip = str_severIP;
            Server_Port = str_Port;
            type = (Type)Enum.Parse(typeof(Type), string_type);

            manager.networkAddress = Server_Ip;
            kcp.port = ushort.Parse(Server_Port);

            return type;

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return Type.Empty;
        }
    }

    void Start()
    {
        type = License_type();

        if (type.Equals(Type.Server))
        {
            Start_Server();
        }
        else
        {
            Start_Client();
        }
    }

    public void Start_Game()
    {
        type = License_type();

        if (type.Equals(Type.Server))
        {
            Start_Server();
        }
        else
        {
            Start_Client();
        }
    }

    public void Start_Server() //서버 입장
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL cannot be Server");
        }
        else
        {
            manager.StartServer();
            Debug.Log($"{manager.networkAddress} StartServer...");
            NetworkServer.OnConnectedEvent += (NetworkConnectionToClient) =>
            {
                Debug.Log($"new client Connect : {NetworkConnectionToClient.address}");
            };
            NetworkServer.OnDisconnectedEvent += (NetworkConnectionToClient) =>
            {
                Debug.Log($"client Disconnect : {NetworkConnectionToClient.address}");
            };
        }
    }

    public void Start_Client() //클라이언트 입장
    {
        Debug.Log($"{manager.networkAddress} : StartClient");
        manager.StartClient();
    }

    private void OnApplicationQuit()
    {
        if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
        if (NetworkClient.active)
        {
            manager.StopServer();
        }
    }
}
