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

    public override string ToString()
        => $"Id:{Id}\tPwd:{Pwd}\tNick:{Nick}\tLvl:{Lvl}\tExp:{Exp}\tWin{Win}\tLose:{Lose}\tDraw:{Draw}";
}
