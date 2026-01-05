using UnityEngine;

/// <summary>
/// 데미지 계산 유틸 클래스
/// </summary>
public static class DamageCalculator
{
    /// <summary>
    /// 최종 데미지 계산 (크리티컬 + 방어력 적용)
    /// </summary>
    public static DamageResult CalculateFinalDamage(ProjectileData projectileData, float defense)
    {
        float baseDamage = projectileData.AttackPower;

        // 1. 크리티컬 계산
        bool isCritical = TryCalculateCriticalDamage(
            baseDamage,
            projectileData.CriticalRate,
            projectileData.CriticalDamage,
            out float criticalDamage);

        // 2. 방어력 적용
        float finalDamage = ApplyDefense(criticalDamage, defense);

        // 3. 최소 데미지 보장
        finalDamage = Mathf.Max(finalDamage, 1f);

        return new DamageResult(finalDamage, isCritical);
    }

    /// <summary>
    /// 방어력 적용 (예: 방어력 100 = 50% 감소)
    /// </summary>
    public static float ApplyDefense(float damage, float defense)
    {
        // 공식 예시: 데미지 감소율 = 방어력 / (방어력 + 100)
        float reduction = defense / (defense + 100f);
        return damage * (1f - reduction);
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
    public static float CalculateStatMultiplier(float baseValue, float multiplier)
    {
        return baseValue * multiplier;
    }

    /// <summary>
    /// 스탯 합연산 (기본 수치 + 추가 수치)
    /// </summary>
    public static float CalculateStatAdditive(float baseValue, float additionalValue)
    {
        return baseValue + additionalValue;
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