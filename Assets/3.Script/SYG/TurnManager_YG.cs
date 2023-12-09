using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum turn
{
    none = 0,
    player1,
    player2
}

public class TurnManager_YG : NetworkBehaviour
{
    [SyncVar(hook = nameof(Turn_setting))]
    public turn player_turn = turn.player1;
    public List<RockManager_YG> all_rockmanager = new List<RockManager_YG>();
    Text select_text;

    private void Awake()
    {
        select_text = FindObjectOfType<Text>();
    }

    [Command]
    public void Change_turn()
    {
        player_turn = (player_turn == turn.player1) ? turn.player2 : turn.player1;
    }

    public void Game()
    {
        
    }
    private void Turn_setting(turn _, turn __)
    {
        if (player_turn == turn.player1)
        {
            all_rockmanager[0].is_myturn = true;
            all_rockmanager[1].is_myturn = false;
            select_text.text = "player1";
        }
        else
        {
            all_rockmanager[0].is_myturn = false;
            all_rockmanager[1].is_myturn = true;
            select_text.text = "player2";
        }
    }
}
