using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

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

    private bool is_myturn = true; //내차롄지 확인

    [Header("Init_rock")]
    private int init_count = 7;
    [SerializeField] private List<GameObject> rock_list = new List<GameObject>();
    [SerializeField] private GameObject rock_prefab;
    Vector2 first_initpos = new Vector2(-5.64f, -3f);
    Vector2 second_initpos = new Vector2(-6.64f, -1.5f);
    float x_distance = 2f;

    [Header("Selected_rock")]
    [SerializeField] Text select_text;

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
        Debug.Log("Init_rock");
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
            NetworkServer.Spawn(rock);
        }
    }

        [Command]
    private void Reset_rock()
    {
        Debug.Log("Init_rock");
        for (int i = 0; i < init_count; i++)
        {
            GameObject rock = rock_list[i];
            rock_list.Remove(rock);
            Destroy(rock);
        }
        Debug.Log("Reset_rock");
    }

    private void Selecting_rock()
    {
        //텍스트 키기
        select_text.enabled = true;
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
            yield return null;
        }
    }

    private void find_rock()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(pos);
        RaycastHit2D[] hit = Physics2D.RaycastAll(pos, Vector2.zero);
        foreach(RaycastHit2D r in hit)
        {
            if (r.transform.CompareTag("Rock"))
            {
                GameObject click_obj = r.transform.gameObject;
                Rock_YG select_rock;
                //click_obj가 rock_yg컴포넌트를 가지고 있다면
                //rock_yg.is_selected를 true로 변경
                if (click_obj.TryGetComponent<Rock_YG>(out select_rock))
                {
                    select_rock.is_selected = true;
                    StopCoroutine(click_co());
                    select_text.enabled = false;
                }
            }
            else
            {
                Debug.Log("클릭했는데 돌이 없음");
            }
        }
        is_myturn = false;
    }

}
