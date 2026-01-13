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
    public int StartLevel;
    public int StartEXP;

    [Header("Starting Heroes")]
    [Tooltip("직접 수정 금지!, 데이터 확인용"), SerializeField] public HeroConfigGroup Heroes;
    [SerializeField] private List<HeroDataSO> allHeroes;
    [SerializeField] private List<HeroDataSO> initialHeroes;
    
    private void OnValidate()
    {
        SetHeroConfigGroup();
    }

    private void SetHeroConfigGroup()
    {
        if (allHeroes == null) return;

        Heroes = new HeroConfigGroup();

        foreach (HeroDataSO hero in allHeroes)
        {
            var heroConfig = new InitialHeroConfig
            {
                HeroData = hero,
                IsAcquired = initialHeroes != null && initialHeroes.Contains(hero)
            };

            switch (hero.ClassType)
            {
                case HeroClassType.Magician:
                    Heroes.Magicians.Add(heroConfig);
                    break;

                case HeroClassType.Archer:
                    Heroes.Archers.Add(heroConfig);
                    break;

                case HeroClassType.Warrior:
                    Heroes.Warriors.Add(heroConfig);
                    break;
            }
        }
    }
}

[Serializable]
public class HeroConfigGroup
{
    public List<InitialHeroConfig> Magicians = new();
    public List<InitialHeroConfig> Archers = new();
    public List<InitialHeroConfig> Warriors = new();
}

[Serializable]
public struct InitialHeroConfig
{
    public HeroDataSO HeroData;
    public bool IsAcquired;
}