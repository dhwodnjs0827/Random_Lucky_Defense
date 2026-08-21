/// <summary>
/// 게임 상수 모음
/// </summary>
public static class GameConstants
{
    // 게임 시작 씬
    public const SceneType START_SCENE = SceneType.TitleScene;
    
    public const string LOCALIZATION_TABLE_NAME = "Localization";
    
    // 영웅 보유 효과
    public const float RANK_B_ACQUIRED_BONUS_DAMAGE = 0.002f;
    public const float RANK_A_ACQUIRED_BONUS_DAMAGE = 0.006f;
    public const float RANK_S_ACQUIRED_BONUS_DAMAGE = 0.02f;
    
    // 랭크별 레벨 업 요구 초기 스택
    public const int INITAIL_RANK_B_LEVEL_UP_REQUIRMENT_STACK = 5;
    public const int INITAIL_RANK_A_LEVEL_UP_REQUIRMENT_STACK = 3;
    public const int INITAIL_RANK_S_LEVEL_UP_REQUIRMENT_STACK = 1;
    
    // 랭크별 레벨 업 요구 스택 증가량
    public const int INCREASE_RANK_B_LEVEL_UP_REQUIRMENT_STACK = 3;
    public const int INCREASE_RANK_A_LEVEL_UP_REQUIRMENT_STACK = 1;
    public const int INCREASE_RANK_S_LEVEL_UP_REQUIRMENT_STACK = 0;
    
    // 레벨 업 초기 골드 요구량
    public const int INITIAL_LEVEL_UP_REQUIRMENT_GOLD = 500;
    
    // 랭크별 레벨 업 골드 요구 증가량
    public const int INCREASE_RANK_B_LEVEL_UP_REQUIRMENT_GOLD = 200;
    public const int INCREASE_RANK_A_LEVEL_UP_REQUIRMENT_GOLD = 250;
    public const int INCREASE_RANK_S_LEVEL_UP_REQUIRMENT_GOLD = 300;
    
    // 상점 영웅 가챠 랭크 확률
    public const int RANK_B_CHANCE = 72; // B 랭크 - 72%
    public const int RANK_A_CHANCE = 25; // A 랭크 - 25%
    public const int RANK_S_CHANCE = 3; // S 랭크 - 3%

    // 인게임 내 영웅 소환 확률
    public const int NORMAL_HERO_SPAWN_CHANCE = 5000; // 일반 - 50%
    public const int SUPERIOR_HERO_SPAWN_CHANCE = 3300; // 고급 - 33%
    public const int RARE_HERO_SPAWN_CHANCE = 1020; // 희귀 - 10.2%
    public const int ANCIENT_HERO_SPAWN_CHANCE = 510; // - 고대 5.10%
    public const int RELIC_HERO_SPAWN_CHANCE = 80; // 유물 - 0.80%
    public const int LEGEND_HERO_SPAWN_CHANCE = 50; // 전설 - 0.50%
    public const int EPIC_HERO_SPAWN_CHANCE = 20; // 에픽 - 0.20%
    public const int MYTH_HERO_SPAWN_CHANCE = 8; // 신화 - 0.08%
    public const int GOD_HERO_SPAWN_CHANCE = 2; // 태초 - 0.02%
    
    // 투사체 속도
    public const float PROJECTILE_SPEED = 20f;
    
    // 게임 내 초기 SP
    public const int INITIAL_HERO_SPAWN_POINT = 40;
    // 게임 내 영웅 소환 비용
    public const int HERO_SPAWN_POINT_COST = 20;

    // 재능 선택 등장 웨이브 배수 값
    public const int ABILITY_SELECTION_STAGE_INTERVAL = 7;
    // 재능 레벨 최대치
    public const int ABILITY_MAX_LEVEL = 5;
    
    // 인게임 최대 적 수
    public const int MAX_ENEMY_COUNT = 100;
}

/// <summary>
/// Resources 경로
/// </summary>
public static class ResourcesPath
{
    public const string INITIAL_GAME_CONFIG = "Data/SO/InitialGameConfig";
}

/// <summary>
/// 어드레서블 라벨
/// </summary>
public static class AddressableLabels
{
    public const string ABILITY_DATA = "AbilityData";
    public const string ABILITY_LEVEL_DATA = "AbilityLevelData";
    public const string DAMAGE_RATE_BY_CLASS_DATA = "DamageRateByClassData";
    public const string ENEMY_DATA = "EnemyData";
    public const string HERO_DATA = "HeroData";
    public const string INGAME_HERO_LEVEL_UP_DATA = "InGameHeroLevelUpData";
    public const string SUMMON_DATA = "SummonData";
    public const string WAVE_DATA = "WaveData";
}

public static class ResDirPath
{
    public const string PREFAB_ENEMY = "Prefabs/Enemy/";
}

public static class LocalizationKeys
{
    /// <summary>
    /// ko: 준비 중입니다.
    /// <para>en: Preparing...</para>
    /// </summary>
    public const string UI_PREPARING = "ui_preparing";
    
    /// <summary>
    /// ko: 요구량이 부족합니다.
    /// <para>en: Not enough required amount</para>
    /// </summary>
    public const string UI_INSUFFICIENT_AMOUNT = "ui_insufficient_amount";
    
    /// <summary>
    /// ko: 로딩 중
    /// <para>en: Loading</para>
    /// </summary>
    public const string UI_LOADING = "ui_loading";
}

public static class AudioResources
{
    public const string TITLE_BGM = "Audio/Tilte_BGM";
    public const string INGAME_BGM = "Audio/InGame_BGM";
}