public class WarriorCardEffectHandler : BaseHeroCardEffectHandler
{
    public WarriorCardEffectHandler(BaseHero baseHero) : base(baseHero)
    {
    }

    public override void RegisterCardEffect()
    {
        InGameManager.Instance.CardEffectFactory.RegisterCardEffectHandler(BuffEffectType.WarriorIncreaseMoveSpeed, this);
    }

    public override void UnregisterCardEffect()
    {
        InGameManager.Instance.CardEffectFactory.UnregisterCardEffectHandler(BuffEffectType.WarriorIncreaseMoveSpeed, this);
    }

    public override void ApplyEffect(BuffCardContainer cardContainer)
    {
        if (cardContainer.CardData.BuffEffectType == BuffEffectType.WarriorIncreaseMoveSpeed)
        {
            CDebug.Log("[WarriorCardEffectHandler] 전사 이동 속도 증가]");
        }
    }
}