/// <summary>
/// 궁수 영웅 클래스
/// </summary>
public class ArcherHero : BaseHero
{
    public override HeroClassType ClassType => HeroClassType.Archer;

    protected override void Awake()
    {
        base.Awake();
        effectHandler = new ArcherCardEffectHandler(this);
    }

    public override void OnGet()
    {
        
    }

    public override void OnRelease()
    {
    }
}
