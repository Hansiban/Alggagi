using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager_YG : NetworkBehaviour
{
    public static TurnManager_YG instance = null;
    public RockManager_YG[] all_rockmanager = null;
    [SyncVar(hook = nameof(Turn_setting))]
    public int turn_count;
    //public List<RockManager_YG> all_rockmanager = new List<RockManager_YG>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(this);
        }
    }

    [Command(requiresAuthority = false)]
    public void Cmdchange_turn()
    {
        Debug.Log("Cmdchange_turn()");
        Change_turn();
    }

    [Server]
    public void Change_turn()
    {
        turn_count = turn_count == 1 ? 2 : 1;
        Debug.Log($"�� �ٲٱ� {turn_count}����");
    }

    private void Turn_setting(int _old, int _new)
    {
        turn_count = _new;
        Check_count();
    }

    //private void Add_rockmanager(RockManager_YG manager)
    //{
    //    all_rockmanager.Add(manager);
    //    Debug.Log("manager �߰� ����");
    //    Debug.Log("all_rockmanager : " + all_rockmanager.Count);
    //}

    public void Check_count() //turncount == roomplayer.index��� ���� ����
    {
        //Debug.Log("üũī��Ʈ");
        //if (all_rockmanager == null || all_rockmanager.Length == 1)
        //{
        //    all_rockmanager = FindObjectsOfType<RockManager_YG>();
        //}

        //for (int i = 0; i < all_rockmanager.Length; i++)
        //{
        //    if (turn_count == MyNetworkRoomManager.singleton.roomSlots[i].index+1)
        //    {
        //        all_rockmanager[i].is_myturn = true;
        //    }
        //}

        foreach (NetworkRoomPlayer player in MyNetworkRoomManager.singleton.roomSlots)
        {
            Debug.Log("üũī��Ʈforeach");
            MyNetworkRoomPlayer myplayer;
            myplayer = player as MyNetworkRoomPlayer;
            Debug.Log(turn_count + "||" + player.index);
            if (turn_count == player.index + 1 && player.isOwned)
            {
                Debug.Log("turn_count == player.index");
                if (myplayer.rockmanager == null)
                {
                    Debug.Log(myplayer.rockmanager == null);
                    RockManager_YG rockManager = FindObjectOfType<RockManager_YG>();
                    rockManager.Find_player();
                }
                myplayer.rockmanager.is_myturn = true;
                Debug.Log("is_myturn = " + myplayer.rockmanager.is_myturn);
                break;
            }

            //    //else
            //    //{
            //    //    Debug.Log("turn_count != player.index");
            //    //    myplayer.rockmanager.is_myturn = false;
            //    //    Debug.Log("is_myturn = " + myplayer.rockmanager.is_myturn);
            //    //}

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
