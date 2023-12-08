using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

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
    private const string IP_ADDRESS = "172.30.1.19";
    private const string DB_ID = "root";
    private const string DB_PWD = "1234";
    private const string DB_NAME = "Alggagi_Test";

    private readonly string connStr = $"server={IP_ADDRESS};uid={DB_ID};pwd={DB_PWD};database={DB_NAME};charset=utf8 ;";
    #endregion

    // Gamemanager가 User 데이터 관리할 수도 있음
    //public UserDataModel_KYS UserData { get; private set; } = null;

    // 여러 씬에서 UserData에 접근하기에 생성한 다른 씬 test용 데이터
    public UserDataModel_KYS UserData { get; private set; } = new UserDataModel_KYS()
    {
        Draw = 0,
        Id = "test_id",
        Level = 0,
        Experience = 0,
        Lose = 0,
        Nickname = "test_nickname",
        Password = "test_password",
        Win = 0
    };

    public bool InsertUserData(UserDataModel_KYS incomingData)
    {
        if (UserData != null)
            return false;

        UserData = incomingData;
        
        return true;
    }

    public void RemoveUserData()
    {
        UserData = null;
    }

    /// <summary>
    /// 파라미터 cmdTxt를 실행했을 때 데이터를 찾았을 경우 지정한 타입 T의 형태로 반환, 그렇지 않을 경우 타입 T의 default 값(null 등)을 반환
    /// </summary>
    public T Select<T>(string cmdTxt) where T : new()
    {
        T rowModel = default(T);

        using (MySqlConnection connection = new MySqlConnection(connStr))
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
        
        using (MySqlConnection connection = new MySqlConnection(connStr))
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

        using (MySqlConnection connection = new MySqlConnection(connStr))
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
