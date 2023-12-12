using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Network Room Manager�� ����� ���� Player Prefab�� �ο��� ��ũ��Ʈ
public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Canvas _emojiCanvasPrefab; // ���� �����ʵ� ��� ������ ������ �Ű� �� ��Ȳ �ƴ�

    void Start()
    {
        if (!isLocalPlayer) return;

        Debug.Log("���� id��" + GameManager.Instance.LocalUserData.Id);

        CmdGetOpponentData(GameManager.Instance.LocalUserData.Id);
    }

    [Command]
    private void CmdGetOpponentData(string id)
    {
        Debug.Log("���� id��" + id);

        Debug.Log("My Network Player Start. My Network Room Manager�� User Data ���� " + MyNetworkRoomManager.singleton.UserDatum.Count);

        var myData = MyNetworkRoomManager.singleton.UserDatum[id];
        var oppData = MyNetworkRoomManager.singleton.UserDatum.Where(x => x.Key != id).FirstOrDefault().Value;

        Debug.Log($"myData == null {myData == null}\noppData == null {oppData == null}");

        if (myData != null)
            Debug.Log(myData.ToString());
        else if (oppData != null)
            Debug.Log(oppData.ToString());

        TargetGetOpponentData(oppData.Id, oppData.Pwd, oppData.Nick, oppData.Lvl, oppData.Exp, oppData.Win, oppData.Lose, oppData.Draw);
    }

    // �ϵ��ڵ�
    [TargetRpc]
    private void TargetGetOpponentData(string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw)
    {
        var oppData = UserDataModel_KYS.GetCopy(id, pwd, nick, lvl, exp, win, lose, draw);

        Debug.Log("oppoData : " + oppData.ToString());
        Debug.Log("myData : " + GameManager.Instance.LocalUserData.ToString());

        Debug.Log("�ش� �������� ĵ���� ���� = " + FindObjectsOfType<Canvas>().Length);
        Debug.Log("ù ��° ĵ���� �̸� = " + FindObjectOfType<Canvas>().name);

        // ���� ä���
        Canvas canvas = Instantiate(_emojiCanvasPrefab);
        canvas.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform); // �̹� ���� �ִ� Canvas�� �ڽ����� ��

        PlayerProfile[] playerProfiles = canvas.GetComponentsInChildren<PlayerProfile>();
        playerProfiles[0].Init(nick, lvl); // opp
        playerProfiles[1].Init(GameManager.Instance.LocalUserData.Nick, GameManager.Instance.LocalUserData.Lvl); // my


        Debug.Log("playerProfiles ���� = " + playerProfiles.Length);


        // record containers
        GameObject[] recordContainers = GameObject.FindGameObjectsWithTag("Container");
        Debug.Log("recordContainers ���� = " + recordContainers.Length);

        //TMP_Text[] oppoRecordTxts = recordContainers[0].GetComponents<TMP_Text>(); // opp

        List<TMP_Text> oppoRecordTxts = GameObject.FindGameObjectsWithTag("RecordText").Where(x => x.transform.parent == recordContainers[0].transform).Select(x => x.GetComponent<TMP_Text>()).ToList();
        Debug.Log("oppoRecordTxts ���� = " + oppoRecordTxts.Count);

        // ��

        //oppoRecordTxts[0].text = win.ToString(); // oppo win
        //oppoRecordTxts[1].text = draw.ToString(); // oppo draw
        //oppoRecordTxts[2].text = lose.ToString(); // oppo lose
        oppoRecordTxts[0].text = GameManager.Instance.LocalUserData.Win.ToString(); // my win��
        oppoRecordTxts[1].text = GameManager.Instance.LocalUserData.Draw.ToString(); // my draw
        oppoRecordTxts[2].text = GameManager.Instance.LocalUserData.Lose.ToString(); // my lose

        List<TMP_Text> myRecordTxts = GameObject.FindGameObjectsWithTag("RecordText").Where(x => x.transform.parent == recordContainers[1].transform).Select(x => x.GetComponent<TMP_Text>()).ToList();
        Debug.Log("myRecordTxts ���� = " + myRecordTxts.Count);
        //myRecordTxts[0].text = GameManager.Instance.LocalUserData.Win.ToString(); // my win
        //myRecordTxts[1].text = GameManager.Instance.LocalUserData.Draw.ToString(); // my draw
        //myRecordTxts[2].text = GameManager.Instance.LocalUserData.Lose.ToString(); // my lose
        myRecordTxts[0].text = win.ToString(); // oppo win
        myRecordTxts[1].text = draw.ToString(); // oppo draw
        myRecordTxts[2].text = lose.ToString(); // oppo lose
    }
}
