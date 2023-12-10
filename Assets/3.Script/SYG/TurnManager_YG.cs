using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager_YG : NetworkBehaviour
{
    public static TurnManager_YG instance = null;
    public RockManager_YG[] all_rockmanager;
    [SyncVar(hook = nameof(Turn_setting))]
    public int player_turn;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this);
    //    }

    //    else
    //    {
    //        Destroy(this.gameObject);
    //        return;
    //    }
    //}
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(this.gameObject);
            return;
        }

        Debug.Log("Start1");
        Add_list();
        if (isClient)
        {
            Debug.Log("Start2");
            //Change_turn();
            Debug.Log("Start3");
        }
        if (isServer)
        {
            CmdChange_turn();
        }
        Debug.Log("Star4");
    }

    [Server]
    public void CmdChange_turn()
    {
        Debug.Log("CmdChange_turn");
        player_turn = (player_turn == 2) ? 1 : 2;
        Debug.Log("player_turn:" + player_turn);
    }

    public void Change_turn()
    {
        Debug.Log("Change_turn");
        player_turn = (player_turn == 2) ? 1 : 2;
        Debug.Log("player_turn:" + player_turn);
    }

    private void Turn_setting(int old_, int new_)
    {
        player_turn = new_;
        Debug.Log("all_rockmanager.Count:" + all_rockmanager.Length);
        if (player_turn == 1)
        {
            all_rockmanager[0].is_myturn = true;
            all_rockmanager[1].is_myturn = false;
        }
        else
        {
            all_rockmanager[0].is_myturn = false;
            all_rockmanager[1].is_myturn = true;
        }
    }

    [Command]
    public void Gameover()
    {
        Debug.Log("Gameover");
    }

    public void Add_list()
    {
        all_rockmanager = FindObjectsOfType<RockManager_YG>();
    }
}
