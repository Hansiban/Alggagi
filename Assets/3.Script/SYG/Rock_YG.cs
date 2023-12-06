using UnityEngine;

public class Rock_YG : MonoBehaviour
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

    private void Start()
    {
        //�Ҵ�
        lineRenderer = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rock_pos = gameObject.transform.position;

        //���η����� ����
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void change_sprite(int index) //���� ������ ���� ��������Ʈ �ٲٱ�
    {
        spriteRenderer.sprite = sprite_rock[index];
    }

    private void Update()
    {
        Update_mousepos();
    }

    private void Update_mousepos() //���콺 ��ġ ������Ʈ -> �ڷ�ƾ���� �����ص� ��������
    {
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
            lineRenderer.enabled = false;
            distance = Vector2.Distance(rock_pos, mouse_pos);
            Go_rock();
        }
    }

    private void Go_rock() //addforce�� �� �����̰� �ϱ�
    {
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
