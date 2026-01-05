/// <summary>
/// 전사 영웅 클래스
/// </summary>
public class WarriorHero : BaseHero
{
    public override HeroClassType ClassType => HeroClassType.Warrior;
    
    protected override void Awake()
    {
        base.Awake();
        effectHandler = new WarriorCardEffectHandler(this);
    }

    public override void OnGet()
    {
    }

    public override void OnRelease()
    {
    }
}
