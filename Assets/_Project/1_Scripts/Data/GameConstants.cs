/// <summary>
/// 게임 상수 모음
/// </summary>
public static class GameConstants
{
    // 게임 시작 씬
    public const SceneType START_SCENE = SceneType.TitleScene;

    // 게임 내 영웅 소환 확률
    public const int NORMAL_HERO_SPAWN_CHANCE = 5000; // 50%
    public const int SUPERIOR_HERO_SPAWN_CHANCE = 3300; // 33%
    public const int RARE_HERO_SPAWN_CHANCE = 1020; // 10.2%
    public const int ANCIENT_HERO_SPAWN_CHANCE = 510; // 5.10%
    public const int RELIC_HERO_SPAWN_CHANCE = 80; // 0.80%
    public const int LEGEND_HERO_SPAWN_CHANCE = 50; // 0.50%
    public const int EPIC_HERO_SPAWN_CHANCE = 20; // 0.20%
    public const int MYTH_HERO_SPAWN_CHANCE = 8; // 0.08%
    public const int GOD_HERO_SPAWN_CHANCE = 2; // 0.02%
    
    // 게임 내 초기 SP
    public const int INITIAL_HERO_SPAWN_POINT = 100;
    // 게임 내 영웅 소환 비용
    public const int HERO_SPAWN_POINT_COST = 1;

    // 카드 선택 등장 웨이브 배수 값
    public const int CARD_SELECTION_STAGE_INTERVAL = 1;
    // 카드 레벨 최대치
    public const int CARD_MAX_LEVEL = 5;
}
