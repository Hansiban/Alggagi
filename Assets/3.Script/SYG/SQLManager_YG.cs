using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using LitJson;

/*
 sql 연결단계
1. connection 생성
2. connection 오픈

데이터 가져오기
1. connection open 상황인지 확인
2. reader 상태가 읽고 있는 상황인지 확인 -> 한 쿼리문 당 1개씩 리더가 쓰임
3. 데이터를 다 읽었으면 reader의 상태 확인 후 close (안닫으면 다시 못씀)
4. 
 */

public class user_info
{
    public string User_name { get; private set; }
    public string User_Password { get; private set; }

    public user_info(string name, string password)
    {
        User_name = name;
        User_Password = password;
    }
}

public class SQLManager_YG : MonoBehaviour
{
    public user_info info;

    public MySqlConnection connection;
    public MySqlDataReader reader;

    public string DB_Path = string.Empty;

    public static SQLManager_YG instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Application.dataPath + "/Database";
        string serverinfo = ServerSet(DB_Path);
        try
        {
            if (serverinfo.Equals(string.Empty))
            {
                Debug.Log("SQL Server Json Error!");
                return;
            }
            connection = new MySqlConnection(serverinfo);
            connection.Open();
            Debug.Log("SQL Server Open!");

        }

        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string ServerSet(string path)
    {
        if (!File.Exists(path)) //그 경로에 파일이 없나요?
        {
            Directory.CreateDirectory(path); //없으면 폴더 생성
        }
        string Jsonstring = File.ReadAllText(path + "/config.json"); //데이터를 통으로 스트링으로 받아옴
        JsonData itemdata = JsonMapper.ToObject(Jsonstring);//제이슨 형태로 데이터를 바꿔줌
        string serverInfo =
            $"Server={itemdata[0]["IP"]};" + //0번째 있는 ip값(key)을 불러오기
            $"Database={itemdata[0]["TableName"]};" +
            $"Uid={itemdata[0]["ID"]};" +
            $"Pwd={itemdata[0]["PW"]};" +
            $"Port={itemdata[0]["PORT"]};" +
            $"CharSet = utf8;";
        return serverInfo;
    }

    private bool connection_check(MySqlConnection con)
    {
        //현재 MySqlConnection open 상태가 아니라면?
        if (con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if (con.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }

    public bool Login(string id, string password)
    {
        //직접적으로 DB에서 데이터를 가지고 오는 메소드
        //조회되는 데이터가 없다면, false
        //있다면, 위에서 선언한 info에다가 담은 다음에 true

        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            string SQL_command = string.Format(@"SELECT User_name, User_Password FROM user_info WHERE User_name = '{0}' AND User_Password = '{1}';", id, password);
            MySqlCommand cmd = new MySqlCommand(SQL_command, connection);
            reader = cmd.ExecuteReader();
            //reader 읽은 데이터가 1개 이상 존재해?
            if (reader.HasRows)
            {
                //읽은 데이터를 하나씩 나열
                while (reader.Read())
                {
                    /*
                     삼항연산자
                     */
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_Name"].ToString();
                    string pass = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_Password"].ToString();
                    if (!name.Equals(string.Empty) || !pass.Equals(string.Empty)) //정상적으로 데이터를 불러온 상황
                    {
                        info = new user_info(name, pass);
                        if (!reader.IsClosed) reader.Close();
                        return true;
                    }
                    else //로그인 실패
                    {
                        break;
                    }
                }//while끝
            }//if끝
            if (!reader.IsClosed) reader.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }

}
