public class UserDataModel_KYS
{
    public string Id { get; set; }
    public string Pwd { get; set; }
    public string Nick { get; set; }

    public int Lvl { get; set; }
    public int Exp { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
    public int Draw { get; set; }

    public static UserDataModel_KYS GetCopy(UserDataModel_KYS original)
    {
        return new UserDataModel_KYS()
        {
            Id = original.Id,
            Pwd = original.Pwd,
            Nick = original.Nick,
            Lvl = original.Lvl,
            Exp = original.Exp,
            Win = original.Win,
            Lose = original.Lose,
            Draw = original.Draw
        };
    }
    public static UserDataModel_KYS GetCopy(string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw)
    {
        return new UserDataModel_KYS()
        {
            Id = id,
            Pwd = pwd,
            Nick = nick,
            Lvl = lvl,
            Exp = exp,
            Win = win,
            Lose = lose,
            Draw = draw
        };
    }
    public override string ToString()
        => $"Id:{Id}\tPwd:{Pwd}\tNick:{Nick}\tLvl:{Lvl}\tExp:{Exp}\tWin{Win}\tLose:{Lose}\tDraw:{Draw}";
}
