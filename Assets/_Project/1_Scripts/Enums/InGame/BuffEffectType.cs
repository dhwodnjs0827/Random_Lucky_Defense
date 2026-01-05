public enum BuffEffectType
{
    /// <summary>
    /// 크리티컬 확률 {value}% 증가
    /// </summary>
    IncreaseCriticalRate,
    /// <summary>
    /// 크리팈컬 데미지 {value}% 증가
    /// </summary>
    IncreaseCriticalDamage,
    /// <summary>
    /// {value}초 마다 SP {value1} 획득
    /// </summary>
    IncreaseSpawnPointGainRate,
    
    /// <summary>
    /// 마법사 이동속도 {value}% 증가
    /// </summary>
    MagicianIncreaseMoveSpeed,
    
    /// <summary>
    /// 궁수 이동속도 {value}% 증가
    /// </summary>
    ArcherIncreaseMoveSpeed,
    /// <summary>
    /// 고대석상이 출현하여 적에게 {value}초마다 레이저로 {value1} 데미지를 가함 (방어력 무시) (궁수 업그레이드 적용)
    /// </summary>
    ArcherSummonAncientStatue,
    
    /// <summary>
    /// 전사 이동속도 {value}% 증가
    /// </summary>
    WarriorIncreaseMoveSpeed,
}