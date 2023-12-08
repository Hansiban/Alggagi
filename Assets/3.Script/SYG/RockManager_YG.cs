using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class RockManager_YG : NetworkBehaviour
{
    #region ����ġ

    /*
    �޼ҵ�
    1.rock ���� �� ��ġ
    2. �� �����ϱ�
    3. ���� ���� ���� �÷��̾ �ִٸ�, ����ó���ϱ�
    4. ����ó�� : �̱��� - �¼� �ø���, �� ��� - �м� �ø���, ����ġ �ο��ϱ�
     */

    #endregion

    private bool is_myturn = true; //�������� Ȯ��
    private NetworkConnection conn;

    [Header("Init_rock")]
    private int init_count = 7;
    [SerializeField] private List<GameObject> rock_list = new List<GameObject>();
    [SerializeField] private GameObject rock_prefab;
    [SerializeField] private GameObject panel_prefab;
    Vector2 first_initpos = new Vector2(-5.64f, -3f);
    Vector2 second_initpos = new Vector2(-6.64f, -1.5f);
    float x_distance = 2f;

    [Header("Selected_rock")]
    [SerializeField] Text select_text;

    [Header("Game_End")]
    private UserDataModel_KYS user_data;


    private void Awake()
    {
        //Init_rock();
        //select_text = FindObjectOfType<Text>();
        //select_text.enabled = false;
    }

    private void Start()
    {
        //Selecting_rock();
    }
    public override void OnStartAuthority()
    {
        Init_rock();
        select_text = FindObjectOfType<Text>();
        select_text.enabled = false;
        Selecting_rock();
        base.OnStartAuthority();
    }

    public override void OnStopAuthority()
    {
        Reset_rock();
        base.OnStopAuthority();
    }

    private void OnApplicationQuit()
    {
        Reset_rock();
    }

    [Command]
    private void Init_rock()
    {
        GameObject pannel = Instantiate(panel_prefab);
        NetworkServer.Spawn(pannel);
        
       // Debug.Log("Init_rock");
        for (int i = 0; i < init_count; i++)
        {
            Vector3 pos = new Vector3();
            if (i < init_count / 2)
            {
                pos = new Vector3(first_initpos.x + i * x_distance, first_initpos.y, 0);
            }
            else
            {
                pos = new Vector3(second_initpos.x + (i - 3) * x_distance, second_initpos.y, 0);
            }
            
            GameObject rock = Instantiate(rock_prefab, pos, Quaternion.identity);
            rock_list.Add(rock);
            NetworkServer.Spawn(rock, connectionToClient);
            // ������ ��ü�� NetworkIdentity�� �����ͼ� ���� ����
            NetworkIdentity rockIdentity = rock.GetComponent<NetworkIdentity>();
            rockIdentity.AssignClientAuthority(connectionToClient);
        }
    }

    private void Reset_rock()
    {
        for (int i = 0; i < rock_list.Count; i++)
        {
            GameObject rock = rock_list[i];
            rock_list.Remove(rock);
            Destroy(rock);
        }
        Debug.Log("Reset_rock");
    }

    private void Selecting_rock()
    {
        //�ؽ�Ʈ Ű��
        select_text.enabled = true;
        //�� Ŭ���ޱ�
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
            yield return null;
        }
    }

    private void find_rock()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(pos, Vector2.zero);
        foreach(RaycastHit2D r in hit)
        {
            if (r.transform.CompareTag("Rock"))
            {
                GameObject click_obj = r.transform.gameObject;
                Rock_YG select_rock;
                //click_obj�� rock_yg������Ʈ�� ������ �ִٸ�
                //rock_yg.is_selected�� true�� ����
                if (click_obj.TryGetComponent<Rock_YG>(out select_rock))
                {
                    select_rock.is_selected = true;
                    StopCoroutine(click_co());
                    select_text.enabled = false;
                    return;
                }
            }
        }
        is_myturn = false;
    }


    #region game_end
    private void Game_end(bool win)
    {
        if (win)
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
        //���� �� �� üũ�ϱ�
        int num = rock_list.Count;
        //����ġ ���
        user_data.Exp += num;
    }
    #endregion
}
