using LitJson;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEngine;

// server only
// async
public class DbAccessManager_KYS
{
    #region Singleton Instance
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
    #endregion

    #region DB Connection Info
    //private const string IP_ADDRESS = "172.30.1.19";
    private string ipAddress = "172.30.1.32";
    private const string DB_ID = "root";
    private const string DB_PWD = "1234";
    private const string DB_NAME = "Alggagi";

    //private readonly string connStr = $"server={IP_ADDRESS};uid={DB_ID};pwd={DB_PWD};database={DB_NAME};charset=utf8 ;";
    private string getConStr => $"server={ipAddress};uid={DB_ID};pwd={DB_PWD};database={DB_NAME};charset=utf8 ;";
    #endregion


    public DbAccessManager_KYS()
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

        ipAddress = itemdata[0]["Server_IP"].ToString();
    }


    // int value only
    public bool Update(string id, string column, string value)
    {
        bool successfullyExecuted = false;

        string cmdTxt = $"UPDATE user SET {column} = {value} WHERE id = \"{id}\"";

        using (MySqlConnection connection = new MySqlConnection(getConStr))
        {
            try
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(cmdTxt, connection))
                {
                    int rowAffected = command.ExecuteNonQuery();
                    successfullyExecuted = rowAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("DB Error: " + ex.Message);
            }
        }

        return successfullyExecuted;
    }


    /// <summary>
    /// 파라미터 cmdTxt를 실행했을 때 데이터를 찾았을 경우 지정한 타입 T의 형태로 반환, 그렇지 않을 경우 타입 T의 default 값(null 등)을 반환
    /// </summary>
    public T Select<T>(string cmdTxt) where T : new()
    {
        T rowModel = default(T);

        using (MySqlConnection connection = new MySqlConnection(getConStr))
        {
            try
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(cmdTxt, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            rowModel = Activator.CreateInstance<T>();

                            foreach (PropertyInfo prop in typeof(T).GetProperties())
                            {
                                object rawValue = reader[prop.Name];

                                object convertedValue = Convert.ChangeType(rawValue, prop.PropertyType);
                                prop.SetValue(rowModel, convertedValue);
                            }

                            //for (int i = 0; i < reader.FieldCount; i++)
                            //{
                            //    string columnName = reader.GetName(i);
                            //    string columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();

                            //    // Set the property dynamically using reflection
                            //    PropertyInfo property = typeof(T).GetProperty(columnName);
                            //    if (property != null)
                            //    {
                            //        // Convert the column value to the property type if needed
                            //        object convertedValue = Convert.ChangeType(columnValue, property.PropertyType);
                            //        property.SetValue(rowModel, convertedValue);
                            //    }
                            //}
                        }

                        //while (reader.Read())
                        //{
                        //    // Iterate through all columns dynamically
                        //    for (int i = 0; i < reader.FieldCount; i++)
                        //    {
                        //        string columnName = reader.GetName(i);
                        //        string columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                        //        Debug.Log($"{columnName}: {columnValue}");
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("DB Error: " + ex.Message);
            }
        }

        return rowModel;
    }

    /// <summary>
    /// 파라미터 cmdTxt를 실행했을 때 데이터를 찾았을 경우 true를 반환, 그렇지 않을 경우 false를 반환
    /// </summary>
    public bool Select(string cmdTxt)
    {
        bool exist = false;
        
        using (MySqlConnection connection = new MySqlConnection(getConStr))
        {
            try
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(cmdTxt, connection))
                {
                    object res = command.ExecuteScalar();
                    exist = res != null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("DB Error: " + ex.Message);
            }
        }

        return exist;
    }

    public bool Insert(string cmdTxt)
    {
        bool success = false;

        using (MySqlConnection connection = new MySqlConnection(getConStr))
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
