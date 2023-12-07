public class UserDataModel_KYS : IQueryDataModel_KYS
{
    public string Id { get; set; }
    public string Password { get; set; }
    public string Nickname { get; set; }

    public int Level { get; set; }
    public int Experience { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
    public int Draw { get; set; }

    // tostring
}
