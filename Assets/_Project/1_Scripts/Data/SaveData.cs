using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public CurrencySaveData CurrencyData;
    public ProfileSaveData ProfileData;
    public HeroSaveData HeroData;
}

[Serializable]
public struct CurrencySaveData
{
    public int Gold;
    public int Gem;
    public int Diamond;
}

[Serializable]
public struct ProfileSaveData
{
    public string PlayerName;
    public int Level;
    public int Exp;
}

[Serializable]
public struct HeroSaveData
{
    public List<int> OwnedHeroIDs;
    public List<int> SelectedHeroIDs;
    public Dictionary<int, int> HeroLevels; // <heroID, level>
}