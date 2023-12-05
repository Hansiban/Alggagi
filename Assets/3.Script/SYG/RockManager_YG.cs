using System.Collections.Generic;
using UnityEngine;

public class RockManager_YG : MonoBehaviour
{
    #region 스케치

    /*
     
    메소드
    1.rock 생성 후 배치
    2. 남은 돌이 없는 플레이어가 있다면, 승패처리하기
    3. 승패처리 : 이긴사람 - 승수 올리기, 진 사람 - 패수 올리기, 경험치 부여하기

     */

    #endregion

    private bool is_myturn; //내차롄지 확인

    [Header("Init_rock")]
    private int init_count = 7;
    [SerializeField] private List<GameObject> rock_list = new List<GameObject>();
    [SerializeField] private GameObject rock_prefab;
    [SerializeField] Vector2 first_initpos = new Vector2(-5.64f, -3f);
    [SerializeField] Vector2 second_initpos = new Vector2(-6.64f, -1.5f);
    [SerializeField] float x_distance = 2f;
    [SerializeField] float y_distance = 1.5f;

    private void Awake()
    {
        Init_rock();
    }

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
                pos = new Vector3(second_initpos.x + (i-3) * x_distance, second_initpos.y, 0);
            }
            GameObject rock = Instantiate(rock_prefab, pos, Quaternion.identity); //position, rotation 설정
            rock_list.Add(rock);
        }
    }

    private void Selected_rock()
    {

    }

}
