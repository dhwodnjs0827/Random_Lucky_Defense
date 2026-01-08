using System;
using System.Collections.Generic;
using Generated;
using UnityEngine;

/// <summary>
/// 게임 초기 설정 데이터
/// </summary>
[CreateAssetMenu(fileName = "InitialGameConfig", menuName = "GameConfig/Initial Game Config")]
public class InitialGameConfig : ScriptableObject
{
    [Header("Starting Currency")] public int StartGold;
    public int StartGem;
    public int Diamond;
    
    [Header("Starting Profile")]
    public string PlayerName;
    public int StartLevel;
    public int StartEXP;

    [Header("Starting Heroes")] public SelectedHeroes StartHeroes;
}

[Serializable]
public struct SelectedHeroes
{
    public List<HeroDataSO> magicians;
    public List<HeroDataSO> archers;
    public List<HeroDataSO> warriors;
}