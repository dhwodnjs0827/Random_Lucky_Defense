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

    public HeroStat()
    {
        AttackPower = 0f;
        AttackSpeed = 0f;
        AttackRange = 0f;
        SplashRange = 0f;
        CriticalRate = 0f;
        CriticalDamage = 0f;
        MoveSpeed = 0f;
    }
    
    public HeroStat(HeroDataSO heroData)
    {
        AttackPower = heroData.AttackPower;
        AttackSpeed = heroData.AttackSpeed;
        AttackRange =  heroData.AttackRange / 50f;
        SplashRange = heroData.SplashRange / 50f;
        CriticalRate = 0f;
        CriticalDamage = 0f;
        MoveSpeed = 5f;
    }

    public void IncreaseAttackPower(float value)
    {
        AttackPower +=  value;
    }
    
    public void IncreaseAttackSpeed(float value)
    {
        AttackSpeed +=  value;
    }
    
    public void IncreaseAttackRange(float value)
    {
        AttackRange +=  value;
    }
    
    public void IncreaseSplashRange(float value)
    {
        SplashRange +=  value;
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
        MoveSpeed +=  value;
    }
}