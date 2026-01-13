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

public enum CurrencyType
{
    Gold,
    Gem,
    Diamond
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
    public List<HeroGameData> AcquiredHeroes;
}

[Serializable]
public struct HeroGameData
{
    public int ID; // 영웅 ID
    public HeroClassType Class;
    public HeroGradeType Grade;
    public HeroRankType Rank;
    public bool IsAcquiredHero; // 영웅 획득 여부 
    public int Level; // 영웅 레벨
    public int AcquiredStack; // 영웅 획득 스택
    public bool isSelected; // 사용 선택 여부
}