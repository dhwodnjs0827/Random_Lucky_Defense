/// <summary>
/// 마법사 영웅 클래스
/// </summary>
public class MagicianHero : BaseHero
{
    public override HeroClassType ClassType => HeroClassType.Magician;
    
    protected override void Awake()
    {
        base.Awake();
        effectHandler = new MagicianCardEffectHandler(this);
    }

    public override void OnGet()
    {
    }

    public override void OnRelease()
    {
    }
}
