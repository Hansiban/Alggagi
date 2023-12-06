using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class User
{
    public string Id { get; set; }
    public string Password { get; set; }
    public string Nickname { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
    public int Draw { get; set; }
}


public class DbAccessManager_KYS
{
    private static DbAccessManager_KYS _instance;
    public static DbAccessManager_KYS Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DbAccessManager_KYS();

            return _instance;
        }
    }

    static string ipAddress = "172.30.1.19";
    static string db_id = "root";
    static string db_pw = "1234";
    static string db_name = "Alggagi_Test";

    string strConn = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8 ;", ipAddress, db_id, db_pw, db_name);

    public string Select(string cmdTxt, string tableName)
    {
        string result = string.Empty;

        using (MySqlConnection connection = new MySqlConnection(strConn))
        {
            try
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(cmdTxt, connection))
                {
                    object exeRes = command.ExecuteScalar();

                    if (exeRes != null)
                        result = exeRes.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("DB Error: " + ex.Message);
            }
        }

        return result;
    }

    public bool Insert(string cmdTxt)
    {
        bool success = false;

        using (MySqlConnection connection = new MySqlConnection(strConn))
        {
            try
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(cmdTxt, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        success = true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("DB Error: " + ex.Message);
            }
        }

        return success;
    }
}
