[System.Serializable]
public struct StructPerValoreStatistica
{
    public TipoStatistica tipoStatistica;
    public OperatoriDiComparamento operatori;
    public int valoreStatistica;
}
public enum TipoStatistica
{
    Strenght,
    Speed,
    Aim,
    Constitution,
    Luck
}