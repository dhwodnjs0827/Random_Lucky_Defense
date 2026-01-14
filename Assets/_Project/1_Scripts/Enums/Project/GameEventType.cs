/// <summary>
/// EventManager에서 사용하는 Event 종류
/// </summary>
public enum GameEventType
{
    ApplicationStart, // 앱 시작
    ApplicationQuit, // 앱 종료
    
    ChangeSelectedHero, // 선택 영웅 변경
    LevelUpHero, // 영웅 레벨 업
    
    InGameStart, // 게임 시작
    InGameFinish, // 게임 종료
    
    WaveStart, // 웨이브 시작
    
    SpawnHero, // 영웅 소환
    
    SpawnEnemy, // 적 소환
    EnemyDie, // 적 사망
    SpawnNormalEnemy, // 일반 적 소환
    NormalEnemyDie, // 일반 적 사망
    SpawnBossEnemy, // 보스 적 소환
    BossEnemyDie, // 보스 적 사망
    
    InGameHeroLevelUpRequest, // 영웅 레벨 업 요청
    InGameHeroLevelUpCompleted, // 영웅 레벨 업 완료
    BuffCardSelected, // 버프 카드 선택
}