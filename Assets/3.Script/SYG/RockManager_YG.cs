using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log(_isMyTurn);
            if (_isMyTurn)
            {
                Selecting_rock();
            }
        }
    }

    [Header("Init_rock")]
    private int init_count = 8;
    private bool isstart_changeturn;
    public List<GameObject> rock_list = new List<GameObject>();
    public int rocklist_count;
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

    private void Start()
    {
        Find_player();
        if (isServer && netId == 8)
        {
            TurnManager_YG.instance.Change_turn();
            //StartCoroutine(Time());
        }

        //if (isClient)
        //{
        //    TurnManager_YG.instance.Check_count();
        //}
    }
    //private IEnumerator Time()
    //{
    //    while (isstart_changeturn)
    //    {
    //        Debug.Log("Time 코루틴 돌아가는 중");
    //        Debug.Log("체크카운트 준비중");
    //        yield return null;
    //    }
    //    TurnManager_YG.instance.Check_count();
    //}

    public override void OnStartAuthority()
    {
        Init_pannel();
        Init_rock();

        base.OnStartAuthority();
    }

    //public override void OnStopAuthority()
    //{
    //    Reset_rock();
    //    base.OnStopAuthority();
    //}

    public void Find_player()
    {
        Debug.Log("Find_player" + isOwned);
        MyNetworkRoomPlayer[] players = FindObjectsOfType<MyNetworkRoomPlayer>();
        Debug.Log("players.Length : " + players.Length);
        foreach (MyNetworkRoomPlayer player in players)
        {
            if (player.isOwned == isOwned)
            {
                Debug.Log("Find_player2");
                network_player = player;
                player.Get_rockmanager(this);
                isstart_changeturn = true;
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
            rocklist_count = rock_list.Count;
            NetworkServer.Spawn(rock, connectionToClient);
            Change_Rocksetting(rock);
            Cmd_camset();
        }
        if (isServer)
        {
            Set_rocklistcount();
        }
    }

    [ServerCallback]
    public void Set_rocklistcount()
    {
        rocklist_count = rock_list.Count;
        Add_rocklistcount(rocklist_count);
    }

    [Command]
    public void cmd_Set_rocklistcount()
    {
        rocklist_count = rock_list.Count;
        Add_rocklistcount(rocklist_count);
    }

    [ClientRpc]
    private void Add_rocklistcount(int count)
    {
        rocklist_count = count;
    }

    [ClientRpc]
    private void Change_Rocksetting(GameObject rock)
    {
        //돌 위치 지정하기
        Debug.Log("돌위치지정 : network_player.netId" + network_player.netId);
        if (network_player == null)
        {
            Debug.Log("network_player == null");
        }
        if (network_player != null && network_player.netId == 2)
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
        if (network_player != null && network_player.netId == 2)
        {
            foreach (GameObject tmp_rock in all_rocks)
            {
                tmp_rock.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            }
        }

    }

    private void Cmd_camset()
    {
        Debug.Log($"{network_player != null} || {network_player.netId == 2}");
        if (network_player != null && network_player.netId == 2)
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
        while (is_myturn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                find_rock();

            }
            yield return null;
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
                Rock_YG select_rock;
                if (click_obj.TryGetComponent<Rock_YG>(out select_rock))
                {
                    select_rock.is_selected = true;
                    if (select_rock.isOwned)
                    {
                        StopCoroutine(click_co());
                        select_rock.is_selected = true;
                        is_myturn = false;
                    }
                    return;
                }
            }
        }
    }

    public void Check_rockcount()
    {
        if (rock_list.Count == 0)
        {
            TurnManager_YG.instance.Gameover(this);
        }
    }
    #region game_end


    public void Win()
    {
        Get_exp();
        user_data.Win += 1;
    }

    public void Lose()
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
