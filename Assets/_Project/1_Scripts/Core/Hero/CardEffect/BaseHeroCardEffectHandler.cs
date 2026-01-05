public abstract class BaseHeroCardEffectHandler : IBuffCardEffect
{
    protected BaseHero hero;
    
    protected BaseHeroCardEffectHandler(BaseHero baseHero)
    {
        hero = baseHero;
    }

    public abstract void RegisterCardEffect();

    public abstract void UnregisterCardEffect();
    
    public abstract void ApplyEffect(BuffCardContainer cardContainer);
}
