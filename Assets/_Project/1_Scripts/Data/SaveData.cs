using System;
using System.Collections.Generic;
using Generated;

/// <summary>
/// 게임 저장 데이터
/// </summary>
[Serializable]
public class SaveData
{
    public CurrencySaveData CurrencyData;
    public ProfileSaveData ProfileData;
    public HeroSaveData HeroData;
}

[Serializable]
public class CurrencySaveData
{
    public int Gold;
    public int Gem;
    public int Diamond;
}

[Serializable]
public class ProfileSaveData
{
    public string PlayerName;
    public int Level;
    public int Exp;
}

[Serializable]
public class HeroSaveData
{
    public List<PlayerHeroSaveData> AllHeroes;
}

/// <summary>
/// 영웅 데이터 저장용
/// </summary>
[Serializable]
public class PlayerHeroSaveData
{
    public int ID; // 영웅 ID
    
    public bool IsAcquiredHero; // 영웅 획득 여부 
    public int Level; // 영웅 레벨
    public int AcquiredStack; // 영웅 획득 스택
    public bool IsSelected; // 사용 선택 여부
    
    /// <summary>
    /// 런타임 용으로 변환
    /// </summary>
    public HeroRuntimeData Convert()
    {
        HeroRuntimeData data = new()
        {
            HeroData = ResourceManager.Instance.Load<HeroDataSO>($"Data/SO/HeroData/{ID}"),
            IsAcquiredHero = IsAcquiredHero,
            Level = Level,
            AcquiredStack = AcquiredStack,
            IsSelected = IsSelected
        };
        return data;
    }
}