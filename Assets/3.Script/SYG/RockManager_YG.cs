using System.Collections.Generic;
using UnityEngine;

public class RockManager_YG : MonoBehaviour
{
    #region ����ġ

    /*
     
    �޼ҵ�
    1.rock ���� �� ��ġ
    2. ���� ���� ���� �÷��̾ �ִٸ�, ����ó���ϱ�
    3. ����ó�� : �̱��� - �¼� �ø���, �� ��� - �м� �ø���, ����ġ �ο��ϱ�

     */

    #endregion

    private bool is_myturn; //�������� Ȯ��

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
            GameObject rock = Instantiate(rock_prefab, pos, Quaternion.identity); //position, rotation ����
            rock_list.Add(rock);
        }
    }

    private void Selected_rock()
    {

    }

}
