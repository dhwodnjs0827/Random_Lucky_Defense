/// <summary>
/// 특수 영웅 스킬 전용 인터페이스
/// </summary>
public interface IHeroSkill
{
    public HeroSkillType HeroSkillType { get; }

    /// <summary>
    /// 스킬 공격
    /// </summary>
    public void Execute(BaseEnemy target);
}
