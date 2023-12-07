using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Rock_YG : NetworkBehaviour
{
    #region ����ġ

    /*
    <1��>
    [Ŭ���̾�Ʈ]
     1. ���콺 �巡�� �Է¹ޱ�(Ŀ��, �� ��ġ �� �Ÿ� �׸���) -> �Ϸ�
     2. �巡�� ���� ���� Ŀ��, �� ��ġ �� �Ÿ� �����ؼ� ������ ������ -> �Ϸ�

    [����]
     1. ���� �Ÿ��� ���� addforce �� ������ �� ������ -> �Ϸ�
     2. ���� �� �ۿ� �ִٸ�, �� ���ֱ� -> �Ϸ�
     */
    #endregion

    //����
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
        //�Ҵ�
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
        //���η����� ����
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        Debug.Log("Line_setting");
    }

    private void change_sprite(int index) //���� ������ ���� ��������Ʈ �ٲٱ�
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
    private void Update_mousepos() //���콺 ��ġ ������Ʈ -> �ڷ�ƾ���� �����ص� ��������
    {
        //Debug.Log("3");
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        mouse_pos = pos;
    }

    private void OnGUI() //�巡�� ����
    {
        Debug.Log("OnGUI");
        if (!is_selected)
        {
            return;
        }

        Event m_event = Event.current;

        if (m_event.type == EventType.MouseDrag) //�巡��
        {
            rock_pos = transform.position;
            lineRenderer.SetPosition(0, rock_pos);
            lineRenderer.SetPosition(1, mouse_pos);

            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }
        }

        if (m_event.type == EventType.MouseUp) //�巡�� ��
        {
            lineRenderer.enabled = false;
            distance = Vector2.Distance(rock_pos, mouse_pos);
            Go_rock();
        }
    }

    private void Go_rock() //addforce�� �� �����̰� �ϱ�
    {
        is_selected = false;
        rigid.AddForce(new Vector2(rock_pos.x - mouse_pos.x, rock_pos.y - mouse_pos.y) * distance, ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D col) //�� ������ ����
    {
        if (col.Equals(BG_col))
        {
            Dead_rock();
        }
    }

    private void Dead_rock() //�� �װ��ϴ� �޼���
    {
        Destroy(gameObject);
    }
}