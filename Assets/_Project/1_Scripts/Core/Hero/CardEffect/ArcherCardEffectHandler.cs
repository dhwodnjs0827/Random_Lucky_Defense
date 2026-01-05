public class ArcherCardEffectHandler : BaseHeroCardEffectHandler
{
    public ArcherCardEffectHandler(BaseHero baseHero) : base(baseHero)
    {
    }

    public override void RegisterCardEffect()
    {
        InGameManager.Instance.CardEffectFactory.RegisterCardEffectHandler(BuffEffectType.ArcherIncreaseMoveSpeed, this);
    }

    public override void UnregisterCardEffect()
    {
        InGameManager.Instance.CardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.ArcherIncreaseMoveSpeed, this);
    }

    public override void ApplyEffect(BuffCardContainer cardContainer)
    {
        if (cardContainer.CardData.BuffEffectType == BuffEffectType.ArcherIncreaseMoveSpeed)
        {
            CDebug.Log("[ArcherCardEffectHandler] 궁수 이동 속도 증가]");
        }
        else if (cardContainer.CardData.BuffEffectType == BuffEffectType.ArcherSummonAncientStatue)
        {
            CDebug.Log("[ArcherCardEffectHandler] 고대 석상 소환]");
        }
    }
}