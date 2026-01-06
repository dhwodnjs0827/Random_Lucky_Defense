using System.Collections.Generic;
using System.Linq;
using Generated;
using UnityEngine;

/// <summary>
/// 데미지 계산 유틸 클래스
/// </summary>
public static class DamageCalculator
{
    private static Dictionary<(HeroClassType, MonsterType), DamageRateByClassDataSO> damageRateByClassData = new();
    
    /// <summary>
    /// 초기화
    /// </summary>
    static DamageCalculator()
    {
        var datas = ResourceManager.Instance.LoadAll<DamageRateByClassDataSO>("Data/SO/DamageRateByClassData");
        foreach (var data in datas)
        {
            if (!damageRateByClassData.ContainsKey((data.ClassType, data.MonsterType)))
            {
                damageRateByClassData.Add((data.ClassType, data.MonsterType), data);
            }
        }
    }

    /// <summary>
    /// 데미지 계산
    /// </summary>
    public static DamageResult CalculateDamage(DamageContext damageContext, MonsterType monsterType, float defense)
    {
        // 기본 공격력 (영웅 기본 공격력 * 레벨 업 공격력 배율 * 카드 효과 공격력 배율)
        var baseDamage = damageContext.BaseDamage;
        
        // 클래스-몬스터별 데미지 배율 계산
        ApplyClassDamageRate(baseDamage, damageContext.HeroClass, monsterType, out baseDamage);
        
        // 크리티컬 확률 계산 (크리티컬 발동 시, 크리티컬 데미지 배율 계산 됨)
        var isCritical = TryCalculateCriticalDamage(baseDamage, damageContext.CriticalRate, damageContext.CriticalDamage, out baseDamage);
        
        // 방어력 적용
        ApplyDefense(baseDamage, defense, out baseDamage);
        
        // 음수 방지 및 최소 데미지 적용
        baseDamage = Mathf.Max(baseDamage, 0.1f);
        
        // 최종 데미지 계산 결과(계산된 데미지, 치명 여부)
        return new DamageResult(baseDamage, isCritical);
    }

    /// <summary>
    /// 클래스별 데미지 비율 적용
    /// </summary>
    public static void ApplyClassDamageRate(float damage, HeroClassType classType, MonsterType monsterType, out float finalDamage)
    {
        var damageRateData = damageRateByClassData[(classType, monsterType)];
        finalDamage = CalculateMultipliers(damage, damageRateData.DamageRate);
    }
    
    /// <summary>
    /// 방어력 적용 (예: 방어력 100 = 50% 감소)
    /// </summary>
    public static void ApplyDefense(float damage, float defense, out float finalDamage)
    {
        // 공식 예시: 데미지 감소율 = 방어력 / (방어력 + 100)
        var reduction = defense / (defense + 100f);
        finalDamage =  damage * (1f - reduction);
    }

    /// <summary>
    /// 치명타 데미지 계산
    /// </summary>
    public static bool TryCalculateCriticalDamage(float damage, float criticalRate, float criticalDamage,
        out float finalDamage)
    {
        var isCritical = Random.Range(0f, 1f) < criticalRate;
        finalDamage = isCritical ? damage * (1f + criticalDamage) : damage;
        return isCritical;
    }

    /// <summary>
    /// 스탯 곱연산 (기본 수치 * 배율)
    /// </summary>
    public static float CalculateMultiplier(float baseValue, float multiplier)
    {
        return baseValue * multiplier;
    }
    
    /// <summary>
    /// 스탯 곱연산 (기본 수치 * 배율들)
    /// </summary>
    public static float CalculateMultipliers(float baseValue, params float[] multipliers)
    {
        return multipliers.Aggregate(baseValue, (current, multiplier) => current * multiplier);
    }

    /// <summary>
    /// 스탯 합연산 (기본 수치 + 추가 수치)
    /// </summary>
    public static float CalculateAdditive(float baseValue, float additionalValue)
    {
        return baseValue + additionalValue;
    }
    
    /// <summary>
    /// 스탯 합연산 (기본 수치 + 추가 수치들)
    /// </summary>
    public static float CalculateAdditives(float baseValue, params float[] additionalValues)
    {
        baseValue += additionalValues.Sum();
        return baseValue;
    }
}

/// <summary>
/// 데미지 계산 결과
/// </summary>
public readonly struct DamageResult
{
    public readonly float Damage;
    public readonly bool IsCritical;

    public DamageResult(float damage, bool isCritical)
    {
        Damage = damage;
        IsCritical = isCritical;
    }
}

/// <summary>
/// 데미지 계산용 Context
/// </summary>
public readonly struct DamageContext
{
    public readonly float BaseDamage;
    public readonly float CriticalRate;
    public readonly float CriticalDamage;
    public readonly HeroClassType HeroClass;

    public DamageContext(float baseDamage, float criticalRate, float criticalDamage, HeroClassType heroClass)
    {
        BaseDamage = baseDamage;
        CriticalRate = criticalRate;
        CriticalDamage = criticalDamage;
        HeroClass = heroClass;
    }
}