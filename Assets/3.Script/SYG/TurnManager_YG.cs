using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        Debug.Log($" {turn_count}바뀜");
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
        Debug.Log("체크카운트 들어오냐?");
        StartCoroutine(check_count_co());
        /*foreach (NetworkRoomPlayer player in MyNetworkRoomManager.singleton.roomSlots)
        {
            Debug.Log("foreach");
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
        }*/
    }

    public IEnumerator check_count_co() {
        foreach (NetworkRoomPlayer player in MyNetworkRoomManager.singleton.roomSlots)
        {
            Debug.Log("foreach");
            MyNetworkRoomPlayer myplayer;
            myplayer = player as MyNetworkRoomPlayer;
            Debug.Log(turn_count + "||" + player.index);
            if (turn_count == player.index + 1 && player.isOwned)
            {
                Debug.Log("turn_count == player.index");
                while (myplayer.rockmanager == null)
                {
                    yield return null;
                }
                myplayer.rockmanager.is_myturn = true;
                Debug.Log("is_myturn = " + myplayer.rockmanager.is_myturn);
                break;
            }
        }
    }

    public void Gameover(RockManager_YG manager)
    {
        Debug.Log("Gameover");
        foreach (var tmp_manager in all_rockmanager)
        {
            if (tmp_manager == manager)
            {
                tmp_manager.Lose();
            }
            else
            {
                tmp_manager.Win();
            }
        }
    }
}
