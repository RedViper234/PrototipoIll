using System.Collections.Generic;

[System.Serializable]
public class StructPerListaFlags
{
    public List<FlagsSO> flagsDaSettare;
    public List<FlagNecessarie> flagsNecessarie;
    [System.Serializable]
    public struct FlagNecessarie
    {
        public List<FlagsSO> flagNecessarie;
    }
}