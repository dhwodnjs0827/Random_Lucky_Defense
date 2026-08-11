/// <summary>
/// EventManager에서 사용하는 Event 종류
/// </summary>
public enum GameEventType
{
    ApplicationStart, // 앱 시작
    ApplicationQuit, // 앱 종료
    GameInitializeProgress, // 게임 초기화 진행률
    
    ChangeSelectedHero, // 선택 영웅 변경
    LevelUpHero, // 영웅 레벨 업
    
    GameStart, // 게임 시작
    GameFinish, // 게임 종료
    GameExit, // 게임 나가기
    
    WaveStart, // 웨이브 시작
    
    SpawnHero, // 영웅 소환
    SpawnRedDragon,
    SpawnAncientStatue,
    SpawnLightning,
    
    SpawnEnemy, // 적 소환
    EnemySpawned,
    EnemyDie, // 적 사망
    SpawnNormalEnemy, // 일반 적 소환
    NormalEnemyDie, // 일반 적 사망
    SpawnBossEnemy, // 보스 적 소환
    BossEnemyDie, // 보스 적 사망
    
    InGameHeroLevelUpRequest, // 영웅 레벨 업 요청
    InGameHeroLevelUpCompleted, // 영웅 레벨 업 완료
    AbilitySelected, // 재능 선택
}