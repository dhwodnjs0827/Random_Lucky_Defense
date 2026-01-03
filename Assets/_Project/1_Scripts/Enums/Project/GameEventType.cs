/// <summary>
/// EventManager에서 사용하는 Event 종류
/// </summary>
public enum GameEventType
{
    ApplicationStart, // 앱 시작
    GameStart, // 게임 시작
    GameVictory, // 게임 승리
    GameOver, // 게임 패배
    ApplicationQuit, // 앱 종료
    
    WaveStart, // 웨이브 시작
    
    SpawnHero, // 영웅 소환
    LevelUpMagician, // 마법사 레벨 업
    LevelUpArcher, // 궁수 레벨 업
    LevelUpWarrior, // 전사 레벨 업
    
    SpawnEnemy, // 적 소환
    EnemyDie, // 적 사망
    SpawnNormalEnemy, // 일반 적 소환
    NormalEnemyDie, // 일반 적 사망
    SpawnBossEnemy, // 보스 적 소환
    BossEnemyDie, // 보스 적 사망
}