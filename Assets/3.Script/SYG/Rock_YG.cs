using Mirror;
using System.Collections;
using UnityEngine;

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
    [SerializeField] private RockManager_YG manager;
    [SerializeField] private BoxCollider2D BG_col;

    private void Start()
    {
        if (isClient)
        {
            CmdGetComponent();
            CmdLine_setting();
        }
    }

    [Command]
    public void CmdGetComponent()
    {
        // �������� ����Ǵ� �ڵ�
        RpcSetComponent();
        //  Debug.Log("CmdGetComponent");
    }

    [ClientRpc]
    public void RpcSetComponent()
    {
        // Ŭ���̾�Ʈ���� ����Ǵ� �ڵ�
        Get_component();
        // Debug.Log("RpcSetComponent");
    }

    public void Get_component()
    {
        //�Ҵ�
        lineRenderer = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        RockManager_YG[] rockmanager = FindObjectsOfType<RockManager_YG>();
        foreach (var tmp_manager in rockmanager)
        {
            if (tmp_manager.isOwned == isOwned)
            {
                manager = tmp_manager;
                break;
            }
        }
        if (spriteRenderer == null)
        {
            Debug.Log("spriteRenderer_null");
        }
        BG_col = GameObject.FindGameObjectWithTag("BG").GetComponent<BoxCollider2D>();

        rock_pos = gameObject.transform.position;
    }

    [Command]
    public void CmdLine_setting()
    {
        // �������� ����Ǵ� �ڵ�
        RpcLine_setting();
        //Debug.Log("CmdLine_setting");
    }

    [ClientRpc]
    public void RpcLine_setting()
    {
        if (isServer)
        {
            // Ŭ���̾�Ʈ���� ����Ǵ� �ڵ�
            Line_setting();
            // Debug.Log("RpcLine_setting");
        }
    }

    [ClientRpc]
    public void Line_setting()
    {
        if (lineRenderer == null)
        {
            //Debug.LogError("lineRenderer is null!");
            return;
        }
        //���η����� ����
        lineRenderer.positionCount = 2;
        //lineRenderer.enabled = false;
        //Debug.Log("Line_setting");
    }

    public void change_sprite(int index) //���� ������ ���� ��������Ʈ �ٲٱ�
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    private void Update_mousepos() //���콺 ��ġ ������Ʈ -> �ڷ�ƾ���� �����ص� ��������
    {
        //Debug.Log("3");
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3(pos.x, pos.y, 0);
        mouse_pos = pos;
    }

    private void OnGUI() //�巡�� ����
    {
        if (!is_selected) 
        {
            return;
        }

        Event m_event = Event.current;

        if (m_event.type == EventType.MouseDrag) //�巡��
        {
            //Ŀ�� �̹��� �ٲٱ�
            Cursor.SetCursor(TurnManager_YG.instance.cursorimgs[1], Vector2.zero, CursorMode.Auto);
            rock_pos = transform.position;
            
            //���η�����
            lineRenderer.SetPosition(0, rock_pos);
            lineRenderer.SetPosition(1, mouse_pos);

            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }
        }

        if (m_event.type == EventType.MouseUp) //�巡�� ��
        {
            Cursor.SetCursor(TurnManager_YG.instance.cursorimgs[0], Vector2.zero, CursorMode.Auto);
            if (isClient)
            {
                TurnManager_YG.instance.Cmdchange_turn();
                check_distance();
            }
        }
    }

    [Client]
    private void check_distance()
    {
        lineRenderer.enabled = false;
        distance = Vector2.Distance(rock_pos, mouse_pos);
        CmdGo_rock(rock_pos, mouse_pos, distance);
    }

    [Command]
    public void CmdGo_rock(Vector2 start, Vector2 end, float distance)
    {
        RpcGo_rock(rock_pos, mouse_pos, distance);
    }

    [ClientRpc]
    public void RpcGo_rock(Vector2 start, Vector2 end, float distance)
    {
        Go_rock(rock_pos, mouse_pos, distance);
    }

    private void Go_rock(Vector2 start, Vector2 end, float distance) //addforce�� �� �����̰� �ϱ�
    {
        is_selected = false;
        rigid.AddForce(new Vector2(start.x - end.x, start.y - end.y) * distance, ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D col) //�� ������ ����
    {
        if (col.Equals(BG_col) && isClient)
        {
            Dead_rock();
        }
    }

    [Command]
    private void Network_destroy()
    {
        Debug.Log("Network_destroy");
        NetworkServer.Destroy(gameObject);
    }

    [Client]
    private void Dead_rock()//(GameObject obj)
    {
        if (isOwned && !manager.is_gameover)
        {
            manager.rocklist_count--;
        }
        Debug.Log($"manager.rocklist_count-- : {manager.rocklist_count}");
        Network_destroy();
        manager.Check_rockcount();
    }

    private IEnumerator Check_velocity()
    {
        while (rigid.velocity != Vector2.zero)
        {
            Debug.Log(rigid.velocity);
            yield return null;
        }
        TurnManager_YG.instance.Change_turn();
    }
}
