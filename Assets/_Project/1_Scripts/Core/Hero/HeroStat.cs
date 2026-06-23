using Generated;

public class HeroStat
{
    public float AttackPower { get; private set; } // 공격력
    public float AttackSpeed { get; private set; } // 공격속도
    public float AttackRange { get; private set; } // 공격범위
    public float SplashRange { get; private set; } // 스플래쉬 범위
    public float CriticalRate { get; private set; } // 크리티컬 확률
    public float CriticalDamage { get; private set; } // 크리티컬 데미지
    public float MoveSpeed { get; private set; } // 이동속도

    public float AttackPowerMultiplier { get; private set; } // 공격력 배율
    public float AttackSpeedMultiplier { get; private set; } // 공격속도 배율
    public float AttackRangeMultiplier { get; private set; } // 공격범위 배율
    public float SplashRangeMultiplier { get; private set; } // 스플래쉬 범위 배율
    public float MoveSpeedMultiplier { get; private set; } // 이동속도 배율

    public HeroStat()
    {
        AttackPower = 0f;
        AttackSpeed = 0f;
        AttackRange = 0f;
        SplashRange = 0f;
        CriticalRate = 0f;
        CriticalDamage = 0f;
        MoveSpeed = 0f;

        AttackPowerMultiplier = 1f;
        AttackSpeedMultiplier = 1f;
        AttackRangeMultiplier = 1f;
        SplashRangeMultiplier = 1f;
        MoveSpeedMultiplier = 1f;
    }

    public HeroStat(HeroDataSO heroData)
    {
        AttackPower = heroData.AttackPower;
        AttackSpeed = heroData.AttackSpeed;
        AttackRange = heroData.AttackRange / 50f;
        SplashRange = heroData.SplashRange / 50f;
        CriticalRate = 0f;
        CriticalDamage = 0f;
        MoveSpeed = 5f;

        AttackPowerMultiplier = 1f;
        AttackSpeedMultiplier = 1f;
        AttackRangeMultiplier = 1f;
        SplashRangeMultiplier = 1f;
        MoveSpeedMultiplier = 1f;
    }

    public void IncreaseAttackPower(float value)
    {
        AttackPower += value;
        CDebug.Log($"[HeroStat] IncreaseAttackPower {value} 적용 전: {AttackPower - value} 적용 후: {AttackPower}");
    }

    public void IncreaseAttackSpeed(float value)
    {
        AttackSpeed += value;
        CDebug.Log($"[HeroStat] IncreaseAttackSpeed {value} 적용 전: {AttackSpeed - value} 적용 후: {AttackSpeed}");
    }

    public void IncreaseAttackRange(float value)
    {
        AttackRange += value;
        CDebug.Log($"[HeroStat] IncreaseAttackRange {value} 적용 전: { - value} 적용 후: {AttackRange}");
    }

    public void IncreaseSplashRange(float value)
    {
        SplashRange += value;
        CDebug.Log($"[HeroStat] IncreaseSplashRange {value} 적용 전: { - value} 적용 후: {SplashRange}");
    }

    public void IncreaseCriticalRate(float value)
    {
        CriticalRate += value;
        CDebug.Log($"[HeroStat] IncreaseCriticalRate {value} 적용 전: { - value} 적용 후: {CriticalRate}");
    }

    public void IncreaseCriticalDamage(float value)
    {
        CriticalDamage += value;
        CDebug.Log($"[HeroStat] IncreaseCriticalDamage {value} 적용 전: { - value} 적용 후: {CriticalDamage}");
    }

    public void IncreaseMoveSpeed(float value)
    {
        MoveSpeed += value;
        CDebug.Log($"[HeroStat] IncreaseMoveSpeed {value} 적용 전: {MoveSpeed - value} 적용 후: {MoveSpeed}");
    }

    public void IncreaseAttackPowerMultiplier(float value)
    {
        AttackPowerMultiplier += value;
        CDebug.Log($"[HeroStat] IncreaseAttackPowerMultiplier {value} 적용 전: {AttackPowerMultiplier - value} 적용 후: {AttackPowerMultiplier}");
    }

    public void IncreaseAttackSpeedMultiplier(float value)
    {
        AttackSpeedMultiplier += value;
        CDebug.Log($"[HeroStat] IncreaseAttackSpeedMultiplier {value} 적용 전: {AttackSpeedMultiplier - value} 적용 후: {AttackSpeedMultiplier}");
    }

    public void IncreaseAttackRangeMultiplier(float value)
    {
        AttackRangeMultiplier += value;
        CDebug.Log($"[HeroStat] IncreaseAttackRangeMultiplier {value} 적용 전: {AttackRangeMultiplier - value} 적용 후: {AttackRangeMultiplier}");
    }

    public void IncreaseSplashRangeMultiplier(float value)
    {
        SplashRangeMultiplier += value;
        CDebug.Log($"[HeroStat] IncreaseSplashRangeMultiplier {value} 적용 전: {SplashRangeMultiplier - value} 적용 후: {SplashRangeMultiplier}");
    }

    public void IncreaseMoveSpeedMultiplier(float value)
    {
        MoveSpeedMultiplier += value;
        CDebug.Log($"[HeroStat] IncreaseMoveSpeedMultiplier {value} 적용 전: {MoveSpeedMultiplier - value} 적용 후: {MoveSpeedMultiplier}");
    }
}