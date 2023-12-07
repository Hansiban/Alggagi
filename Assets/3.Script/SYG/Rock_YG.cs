using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Rock_YG : NetworkBehaviour
{
    #region 스케치

    /*
    <1턴>
    [클라이언트]
     1. 마우스 드래그 입력받기(커서, 돌 위치 간 거리 그리기) -> 완료
     2. 드래그 놓는 순간 커서, 돌 위치 간 거리 측정해서 서버로 보내기 -> 완료

    [서버]
     1. 받은 거리에 따라 addforce 힘 적용해 돌 보내기 -> 완료
     2. 돌이 맵 밖에 있다면, 돌 없애기 -> 완료
     */
    #endregion

    //변수
    [Header("sprite_rock")]
    [SerializeField] private Sprite[] sprite_rock;
    private SpriteRenderer spriteRenderer;

    [Header("Go_rock")]
    [SerializeField] private Vector2 mouse_pos;
    [SerializeField] private Vector2 rock_pos;
    public bool is_selected = false;
    private LineRenderer lineRenderer;
    private float distance;
    private Rigidbody2D rigid;

    [Header("Dead_rock")]
    [SerializeField] private BoxCollider2D BG_col;

    //public override void OnStartAuthority()
    //{
    //    Get_component();
    //    Line_setting();
    //    base.OnStartAuthority();
    //}

    private void Start()
    {
        Get_component();
        Line_setting();
    }

    //[Command]
    private void Get_component()
    {
        //할당
        lineRenderer = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        BG_col = GameObject.FindGameObjectWithTag("BG").GetComponent<BoxCollider2D>();

        rock_pos = gameObject.transform.position;

        Debug.Log("Get_component");
    }

    //[Command]
    private void Line_setting()
    {
        //라인렌더러 설정
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        Debug.Log("Line_setting");
    }

    private void change_sprite(int index) //유저 정보에 따라 스프라이트 바꾸기
    {
        spriteRenderer.sprite = sprite_rock[index];
    }

    private void Update()
    {
        //Debug.Log("1");
        if (!is_selected)
        {
            return;
        }
        //Debug.Log("2");
        Update_mousepos();
    }

    //[Command]
    private void Update_mousepos() //마우스 위치 업데이트 -> 코루틴으로 변경해도 괜찮을듯
    {
        //Debug.Log("3");
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        mouse_pos = pos;
    }

    private void OnGUI() //드래그 감지
    {
        Debug.Log("OnGUI");
        if (!is_selected)
        {
            return;
        }

        Event m_event = Event.current;

        if (m_event.type == EventType.MouseDrag) //드래그
        {
            rock_pos = transform.position;
            lineRenderer.SetPosition(0, rock_pos);
            lineRenderer.SetPosition(1, mouse_pos);

            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }
        }

        if (m_event.type == EventType.MouseUp) //드래그 끝
        {
            lineRenderer.enabled = false;
            distance = Vector2.Distance(rock_pos, mouse_pos);
            Go_rock();
        }
    }

    private void Go_rock() //addforce로 돌 움직이게 하기
    {
        is_selected = false;
        rigid.AddForce(new Vector2(rock_pos.x - mouse_pos.x, rock_pos.y - mouse_pos.y) * distance, ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D col) //돌 죽으면 들어옴
    {
        if (col.Equals(BG_col))
        {
            Dead_rock();
        }
    }

    private void Dead_rock() //돌 죽게하는 메서드
    {
        Destroy(gameObject);
    }
}
