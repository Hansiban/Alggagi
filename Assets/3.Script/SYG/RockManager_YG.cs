using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockManager_YG : NetworkBehaviour
{
    #region 스케치

    /*
    메소드
    1.rock 생성 후 배치
    2. 돌 선택하기
    3. 남은 돌이 없는 플레이어가 있다면, 승패처리하기
    4. 승패처리 : 이긴사람 - 승수 올리기, 진 사람 - 패수 올리기, 경험치 부여하기
     */

    #endregion


    [Header("Gameplay")]
    //private TurnManager_YG trun_manager;
    private bool _isMyTurn;
    public bool is_myturn
    {
        get
        {
            return _isMyTurn;
        }
        set
        {
            _isMyTurn = value;
            if (_isMyTurn)
            {
                Selecting_rock();
            }
        }
    }

    [Header("Init_rock")]
    private int init_count = 8;
    [SerializeField] private List<GameObject> rock_list = new List<GameObject>();
    [SerializeField] private Rock_YG selected_rock;
    [SerializeField] private GameObject rock_prefab;
    [SerializeField] private GameObject panel_prefab;
    private NetworkRoomPlayer network_player;
    Vector2 first_initpos = new Vector2(-6.64f, -3f);
    Vector2 second_initpos = new Vector2(-6.64f, -1.5f);
    float x_distance = 2f;

    [Header("Selected_rock")]

    [Header("Game_End")]
    private UserDataModel_KYS user_data;

    public override void OnStartAuthority()
    {
        Find_player();
        Init_pannel();
        Init_rock();
        Cmd_camset();

        base.OnStartAuthority();
    }

    private void Find_player()
    {
        NetworkRoomPlayer[] players = FindObjectsOfType<NetworkRoomPlayer>();
        foreach (NetworkRoomPlayer player in players)
        {
            if (player.isOwned)
            {
                network_player = player;
                Debug.Log(network_player.netId);
                return;
            }
        }
    }

    [Command]
    private void Init_pannel()
    {
        GameObject pannel = Instantiate(panel_prefab);
        NetworkServer.Spawn(pannel);
    }

    [Command]
    //돌 생성하기
    private void Init_rock()
    {
        for (int i = 0; i < init_count; i++)
        {
            Vector3 pos = new Vector3();
            if (i < init_count / 2)
            {
                pos = new Vector3(first_initpos.x + i * x_distance, first_initpos.y, 0);
            }

            else
            {
                pos = new Vector3(second_initpos.x + (i - (init_count / 2)) * x_distance, second_initpos.y, 0);
            }

            GameObject rock = Instantiate(rock_prefab, pos, Quaternion.identity);
            rock_list.Add(rock);
            NetworkServer.Spawn(rock, connectionToClient);
            Change_Rocksetting(rock);
        }
    }

    [ClientRpc]
    private void Change_Rocksetting(GameObject rock)
    {

        //돌 위치 지정하기
        if (network_player != null && network_player.netId == 1)
        {
            rock.transform.position += new Vector3(0, 4.5f, 0);
        }

        //색 바꾸기
        GameObject[] all_rocks = GameObject.FindGameObjectsWithTag("Rock");
        foreach (GameObject tmp_rock in all_rocks)
        {
            Rock_YG tmprock_component = tmp_rock.GetComponent<Rock_YG>();

            if (tmp_rock.gameObject.GetComponent<NetworkIdentity>().isOwned == true)
            {
                tmprock_component.change_sprite(0);
            }
            else
            {
                tmprock_component.change_sprite(1);
            }
        }

        //카메라 뒤집힌 플레이어는 스프라이트도 뒤집어줌
        if (network_player != null && network_player.netId == 1)
        {
            foreach (GameObject tmp_rock in all_rocks)
            {
                tmp_rock.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            }
        }
            
    }

    private void Cmd_camset()
    {
        if (network_player != null && network_player.netId == 1)
        {
            Camera.main.transform.position = new Vector3(-7.5f, 0, -10);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private void Reset_rock()
    {
        if (isServer)
        {
            for (int i = 0; i < rock_list.Count; i++)
            {
                GameObject rock = rock_list[i];
                rock_list.Remove(rock);
                Destroy(rock);
            }
            Debug.Log("Reset_rock");
        }
    }

    private void Selecting_rock()
    {
        //돌 클릭받기
        StartCoroutine(click_co());
    }

    private IEnumerator click_co()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                find_rock();
            }
        }
    }

    private void find_rock()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(pos, Vector2.zero);
        foreach (RaycastHit2D r in hit)
        {
            if (r.transform.CompareTag("Rock"))
            {
                GameObject click_obj = r.transform.gameObject;
                // Rock_YG select_rock;
                //click_obj가 rock_yg컴포넌트를 가지고 있다면
                //rock_yg.is_selected를 true로 변경
                if (click_obj.TryGetComponent<Rock_YG>(out selected_rock))
                {
                    foreach (var tmp_rock in rock_list)
                    {
                        tmp_rock.GetComponent<Rock_YG>().is_selected = false;
                    }
                    selected_rock.is_selected = true;
                    StopCoroutine(click_co());
                    return;
                }
            }
        }

        if (isServer)
        {
            if (rock_list.Count == 0)
            {
                TurnManager_YG.instance.Gameover();
            }
            TurnManager_YG.instance.Change_turn();
        }
    }

    #region game_end
    private void Game_end(bool is_win)
    {
        if (is_win)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    private void Win()
    {
        Get_exp();
        user_data.Win += 1;
    }

    private void Lose()
    {
        user_data.Lose += 1;
    }

    private void Get_exp()
    {
        //남은 말 수 체크하기
        int num = rock_list.Count;
        //경험치 얻기
        user_data.Exp += num;
    }
    #endregion
}
