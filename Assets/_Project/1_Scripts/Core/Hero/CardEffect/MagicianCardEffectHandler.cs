public class MagicianCardEffectHandler : BaseHeroCardEffectHandler
{
    public MagicianCardEffectHandler(BaseHero baseHero) : base(baseHero)
    {
        
    }

    public override void RegisterCardEffect()
    {
        InGameManager.Instance.CardEffectFactory.RegisterCardEffectHandler(BuffEffectType.MagicianIncreaseMoveSpeed, this);
    }

    public override void UnregisterCardEffect()
    {
        InGameManager.Instance.CardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.MagicianIncreaseMoveSpeed, this);
    }

    public override void ApplyEffect(BuffCardContainer cardContainer)
    {
        if (cardContainer.CardData.BuffEffectType == BuffEffectType.MagicianIncreaseMoveSpeed)
        {
            CDebug.Log("[MagicianCardEffectHandler] 마법사 이동 속도 증가]");
        }
    }
}