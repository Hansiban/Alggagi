using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GameManager
{
    #region Singleton Instance
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();

            return _instance;
        }
    }
    #endregion

    //public UserDataModel_KYS UserData { get; private set; } = null;

    // 여러 씬에서 UserData에 접근하기에 생성한 다른 씬 test용 데이터
    public UserDataModel_KYS LocalUserData { get; private set; } = new UserDataModel_KYS()
    {
        Draw = -1,
        Id = "test_id",
        Lvl = 0,
        Exp = 0,
        Lose = 0,
        Nick = "test_nickname",
        Pwd = "test_password",
        Win = 0
    };

    // 일단은 두 개의 정보만 보내게끔 함. UserData를 그냥 통째로 서버에서 Rpc로 파라미터로 보내는 게 안 됨
    public void InsertLocalUserData(string nick, int level)
    {
        LocalUserData = new UserDataModel_KYS
        {
            Nick = nick,
            Lvl = level
        };
    }

    public bool InsertLocalUserData(UserDataModel_KYS incomingData)
    {
        if (LocalUserData != null
            && LocalUserData.Draw != -1) // UserData.Draw이 -1이면 테스트용 데이터가 들어가 있다는 소리. 나중에 뺄 거임
            return false;

        LocalUserData = incomingData;

        return true;
    }

    public void RemoveLocalUserData()
    {
        LocalUserData = null;
    }
}