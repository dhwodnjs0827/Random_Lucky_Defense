using Generated;

public interface IBuffCardEffect
{
    public void RegisterCardEffect(CardEffectFactory cardEffectFactory);
    public void UnregisterCardEffect(CardEffectFactory cardEffectFactory);
    public void ApplyCardEffect(BuffCardContainer cardContainer);
}
