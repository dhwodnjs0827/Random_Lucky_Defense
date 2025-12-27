/// <summary>
/// EventManager에서 사용하는 Event 종류
/// </summary>
public enum GameEventType
{
    ApplicationStart, // 앱 시작
    GameStart, // 게임 시작
    GameOver, // 게임 종료
    ApplicationQuit, // 앱 종료
    
    SpawnHero, // 영웅 소환
    
    SpawnEnemy, // 적 소환
    EnemyDie, // 적 사망
}