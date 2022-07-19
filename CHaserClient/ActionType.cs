namespace DoctorFe.CHaser
{
    /// <summary>
    /// 行動の種類の列挙型
    /// </summary>
    public enum ActionType : byte
    {
        Walk = 0x77,
        Look = 0x6c,
        Search = 0x73,
        Put = 0x70
    }
}