public enum AbilityEffectType
{
    /// <summary>
    /// 크리티컬 확률 {value}% 증가
    /// </summary>
    IncreaseCriticalRate,
    /// <summary>
    /// 크리티컬 데미지 {value}% 증가
    /// </summary>
    IncreaseCriticalDamage,
    /// <summary>
    /// 피해량 {value}% 증가
    /// </summary>
    IncreaseDamage,
    /// <summary>
    /// 모든 클래스 공격 범위 {value} 증가
    /// </summary>
    IncreaseAttackRange,
    /// <summary>
    /// 스플래시 데미지 범위 {value} 증가 (스플래시 공격만 해당)
    /// </summary>
    IncreaseSplashRange,
    /// <summary>
    /// 적 방어력 {value}% 무시함
    /// </summary>
    IncreasePenetratingPower,
    /// <summary>
    /// 즉시 행운석을 {value}개 획득
    /// </summary>
    AcquireLuckyStone,
    /// <summary>
    /// 영웅뽑기 확률 {value}% 증가
    /// </summary>
    IncreaseSummonRate,
    /// <summary>
    /// {value}초 마다 SP {value1} 획득
    /// </summary>
    IncreaseSpawnPointGainRate,
    
    /// <summary>
    /// 마법사 기본 공격력의 {value}% 증가
    /// </summary>
    MagicianIncreaseAttackPower,
    /// <summary>
    /// 마법사 이동속도 {value}% 증가
    /// </summary>
    MagicianIncreaseMoveSpeed,
    /// <summary>
    /// 레드 드래곤이 출현하여 {value}초마다 {value1} 스플래시 데미지를 가함 (마법사 업그레이드 적용)
    /// </summary>
    MagicianSummonRedDragon,
    
    /// <summary>
    /// 궁수 기본 공격력의 {value}% 증가
    /// </summary>
    ArcherIncreaseAttackPower,
    /// <summary>
    /// 궁수 이동속도 {value}% 증가
    /// </summary>
    ArcherIncreaseMoveSpeed,
    /// <summary>
    /// 고대석상이 출현하여 적에게 {value}초마다 레이저로 {value1} 데미지를 가함 (방어력 무시) (궁수 업그레이드 적용)
    /// </summary>
    ArcherSummonAncientStatue,
    
    /// <summary>
    /// 전사 기본 공격력의 {value}% 증가
    /// </summary>
    KnightIncreaseAttackPower,
    /// <summary>
    /// 전사 이동속도 {value}% 증가
    /// </summary>
    KnightIncreaseMoveSpeed,
    /// <summary>
    /// {value}초마다 번개를 무작위 적에게 내리쳐 {value1} 스플래시 데미지를 가함 (전사 업그레이드 적용)
    /// </summary>
    KnightLightning
}