using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Network Room Manager를 사용할 때에 Player Prefab에 부여될 스크립트
public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Canvas _emojiCanvasPrefab; // 유저 프로필도 담고 있으나 변수명 신경 쓸 상황 아님

    void Start()
    {
        if (!isLocalPlayer) return;

        Debug.Log("나의 id는" + GameManager.Instance.LocalUserData.Id);

        CmdGetOpponentData(GameManager.Instance.LocalUserData.Id);
    }

    [Command]
    private void CmdGetOpponentData(string id)
    {
        Debug.Log("받은 id는" + id);

        Debug.Log("My Network Player Start. My Network Room Manager의 User Data 수는 " + MyNetworkRoomManager.singleton.UserDatum.Count);

        var myData = MyNetworkRoomManager.singleton.UserDatum[id];
        var oppData = MyNetworkRoomManager.singleton.UserDatum.Where(x => x.Key != id).FirstOrDefault().Value;

        Debug.Log($"myData == null {myData == null}\noppData == null {oppData == null}");

        if (myData != null)
            Debug.Log(myData.ToString());
        else if (oppData != null)
            Debug.Log(oppData.ToString());

        TargetGetOpponentData(oppData.Id, oppData.Pwd, oppData.Nick, oppData.Lvl, oppData.Exp, oppData.Win, oppData.Lose, oppData.Draw);
    }

    // 하드코딩
    [TargetRpc]
    private void TargetGetOpponentData(string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw)
    {
        var oppData = UserDataModel_KYS.GetCopy(id, pwd, nick, lvl, exp, win, lose, draw);

        Debug.Log("oppoData : " + oppData.ToString());
        Debug.Log("myData : " + GameManager.Instance.LocalUserData.ToString());

        Debug.Log("해당 씬에서의 캔버스 개수 = " + FindObjectsOfType<Canvas>().Length);
        Debug.Log("첫 번째 캔버스 이름 = " + FindObjectOfType<Canvas>().name);

        // 정보 채우기
        Canvas canvas = Instantiate(_emojiCanvasPrefab);
        canvas.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform); // 이미 씬에 있는 Canvas에 자식으로 들어감

        PlayerProfile[] playerProfiles = canvas.GetComponentsInChildren<PlayerProfile>();
        playerProfiles[0].Init(nick, lvl); // opp
        playerProfiles[1].Init(GameManager.Instance.LocalUserData.Nick, GameManager.Instance.LocalUserData.Lvl); // my


        Debug.Log("playerProfiles 개수 = " + playerProfiles.Length);


        // record containers
        GameObject[] recordContainers = GameObject.FindGameObjectsWithTag("Container");
        Debug.Log("recordContainers 개수 = " + recordContainers.Length);

        //TMP_Text[] oppoRecordTxts = recordContainers[0].GetComponents<TMP_Text>(); // opp

        List<TMP_Text> oppoRecordTxts = GameObject.FindGameObjectsWithTag("RecordText").Where(x => x.transform.parent == recordContainers[0].transform).Select(x => x.GetComponent<TMP_Text>()).ToList();
        Debug.Log("oppoRecordTxts 개수 = " + oppoRecordTxts.Count);

        // 왠

        //oppoRecordTxts[0].text = win.ToString(); // oppo win
        //oppoRecordTxts[1].text = draw.ToString(); // oppo draw
        //oppoRecordTxts[2].text = lose.ToString(); // oppo lose
        oppoRecordTxts[0].text = GameManager.Instance.LocalUserData.Win.ToString(); // my win지
        oppoRecordTxts[1].text = GameManager.Instance.LocalUserData.Draw.ToString(); // my draw
        oppoRecordTxts[2].text = GameManager.Instance.LocalUserData.Lose.ToString(); // my lose

        List<TMP_Text> myRecordTxts = GameObject.FindGameObjectsWithTag("RecordText").Where(x => x.transform.parent == recordContainers[1].transform).Select(x => x.GetComponent<TMP_Text>()).ToList();
        Debug.Log("myRecordTxts 개수 = " + myRecordTxts.Count);
        //myRecordTxts[0].text = GameManager.Instance.LocalUserData.Win.ToString(); // my win
        //myRecordTxts[1].text = GameManager.Instance.LocalUserData.Draw.ToString(); // my draw
        //myRecordTxts[2].text = GameManager.Instance.LocalUserData.Lose.ToString(); // my lose
        myRecordTxts[0].text = win.ToString(); // oppo win
        myRecordTxts[1].text = draw.ToString(); // oppo draw
        myRecordTxts[2].text = lose.ToString(); // oppo lose
    }
}
