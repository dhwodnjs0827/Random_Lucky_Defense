using Generated;
using UnityEngine;

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
    }

    public void IncreaseAttackSpeed(float value)
    {
        AttackSpeed += value;
    }

    public void IncreaseAttackRange(float value)
    {
        AttackRange += value;
    }

    public void IncreaseSplashRange(float value)
    {
        SplashRange += value;
    }

    public void IncreaseCriticalRate(float value)
    {
        CriticalRate += value;
    }

    public void IncreaseCriticalDamage(float value)
    {
        CriticalDamage += value;
    }

    public void IncreaseMoveSpeed(float value)
    {
        MoveSpeed += value;
    }

    public void IncreaseAttackPowerMultiplier(float value)
    {
        AttackPowerMultiplier += value;
    }

    public void IncreaseAttackSpeedMultiplier(float value)
    {
        AttackSpeedMultiplier += value;
    }

    public void IncreaseAttackRangeMultiplier(float value)
    {
        AttackRangeMultiplier += value;
    }

    public void IncreaseSplashRangeMultiplier(float value)
    {
        SplashRangeMultiplier += value;
    }

    public void IncreaseMoveSpeedMultiplier(float value)
    {
        MoveSpeedMultiplier += value;
    }
}