using UnityEngine;

public class Lightning : MonoBehaviour
{
    private HeroClassType classType;
    
    private float attackPower;
    private float attackSpeed;
    private float splashRange;
    
    public HeroStat LevelUpStat => InGameManager.Instance.HeroBuffController.LevelUpStats[classType];
    public HeroStat AbilityEffectStat => InGameManager.Instance.HeroBuffController.AbilityEffectStats[classType];
    public float AcquiredHeroBonusDamage => InGameManager.Instance.HeroBuffController.AcquiredHeroBonusDamages[classType];
    
    private void Awake()
    {
        classType = HeroClassType.Knight;
    }

    private void Update()
    {
        
    }

    public void IncreaseStat(AbilityContainer abilityContainer)
    {
        attackSpeed = abilityContainer.AbilityLevelData.value;
        attackPower = abilityContainer.AbilityLevelData.value1;
    }
    
    private void DetectEnemy()
    {
        
    }

    private void Attack()
    {
        
    }
}
