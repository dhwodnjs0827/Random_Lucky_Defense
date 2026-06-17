/// <summary>
/// 게임 상수 모음
/// </summary>
public static class GameConstants
{
    // 게임 시작 씬
    public const SceneType START_SCENE = SceneType.TitleScene;
    
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
