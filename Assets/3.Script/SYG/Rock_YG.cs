using Mirror;
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
    [SerializeField] private BoxCollider2D BG_col;

    //public override void OnStartAuthority()
    //{
    //    Get_component();
    //    Line_setting();
    //    base.OnStartAuthority();
    //}

    private void Start()
    {
        if (isClient)
        {
            //Debug.Log("This script is running on a client.");

            // Ŭ���̾�Ʈ���� ����� ȣ���ϰų� �ٸ� ������ ������ �� �ֽ��ϴ�.
            CmdGetComponent();
            CmdLine_setting();
        }
        else
        {
           // Debug.Log("not client.");
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
        if (lineRenderer != null)
        {
            Debug.Log(lineRenderer.gameObject.name);
        }
        else
        {
           // Debug.Log($"lineRenderer == null");
        }
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        BG_col = GameObject.FindGameObjectWithTag("BG").GetComponent<BoxCollider2D>();

        rock_pos = gameObject.transform.position;

       // Debug.Log("Get_component");
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
            if (isClient)
            {
                check_distance();
            }
        }
    }

    [Client]
    private void check_distance()
    {
       // Debug.Log("check_distance");
        lineRenderer.enabled = false;
        distance = Vector2.Distance(rock_pos, mouse_pos);
        //Debug.Log(distance);
        CmdGo_rock(rock_pos, mouse_pos, distance);
    }

    [Command]
    public void CmdGo_rock(Vector2 start, Vector2 end, float distance)
    {
        // �������� ����Ǵ� �ڵ�
        RpcGo_rock(rock_pos, mouse_pos, distance);
        //Debug.Log("CmdGo_rock");
    }

    [ClientRpc]
    public void RpcGo_rock(Vector2 start, Vector2 end, float distance)
    {
        // �������� ����Ǵ� �ڵ�
        Go_rock(rock_pos, mouse_pos, distance);
        //Debug.Log("RpcGo_rock");
    }

    private void Go_rock(Vector2 start, Vector2 end, float distance) //addforce�� �� �����̰� �ϱ�
    {
        //Debug.Log("Go_rock");
        is_selected = false;
        rigid.AddForce(new Vector2(start.x - end.x, start.y - end.y) * distance, ForceMode2D.Impulse);
        //Debug.Log($"{rock_pos.x - mouse_pos.x} || {rock_pos.y - mouse_pos.y}");
    }

    private void OnTriggerExit2D(Collider2D col) //�� ������ ����
    {
        if (col.Equals(BG_col))
        {
            CmdDead_rock();
        }
    }

    [Command]
    private void CmdDead_rock()
    {
        GameObject obj = GetComponent<NetworkIdentity>().gameObject;
        Debug.Log("CmdDead_rock");
        RpcDead_rock(obj);
    }

    [ClientRpc]
    private void RpcDead_rock(GameObject obj)
    {
        Debug.Log("RpcDead_rock");
        Dead_rock(obj);
    }

    private void Dead_rock(GameObject obj) //�� �װ��ϴ� �޼���
    {
        Debug.Log("Dead_rock");
        NetworkServer.Destroy(obj);
    }
}
