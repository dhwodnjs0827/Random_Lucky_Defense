using System.Collections.Generic;
using Generated;

public partial class PlayerDataManager
{
    private const string HERO_DATA_RESOURCE_PATH = "Data/SO/HeroData/";
    
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> ownedHeroes = new();
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> selectedHeroes = new();
    private readonly Dictionary<int, int> currentHeroLevels = new(); // <heroID, level>
    private readonly Dictionary<int, int> currentHeroStacks = new();  // <heroID, stack>

    public IDictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> OwnedHeroes => ownedHeroes;
    public IDictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> SelectedHeroes => selectedHeroes;
    public IDictionary<int, int> CurrentHeroLevels => currentHeroLevels;
    public IDictionary<int, int> CurrentHeroStacks => currentHeroStacks;

    /// <summary>
    /// 초기 선택 영웅 데이터 초기화
    /// </summary>
    private void InitializeHeroData(HeroSaveData data)
    {
        var selectedHeroIDs = data.SelectedHeroIDs;
        
        selectedHeroes.Clear();
        selectedHeroes.Add(HeroClassType.Magician, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Archer, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Warrior, new Dictionary<HeroGradeType, HeroDataSO>());

        foreach (var heroID in selectedHeroIDs)
        {
            var so = ResourceManager.Instance.Load<HeroDataSO>($"{HERO_DATA_RESOURCE_PATH}{heroID}");
            selectedHeroes[so.ClassType].Add(so.GradeType, so);
        }
        
        var ownedHeroIDs = data.OwnedHeroIDs;
        ownedHeroes.Clear();
        ownedHeroes.Add(HeroClassType.Magician, new Dictionary<HeroGradeType, HeroDataSO>());
        ownedHeroes.Add(HeroClassType.Archer, new Dictionary<HeroGradeType, HeroDataSO>());
        ownedHeroes.Add(HeroClassType.Warrior, new Dictionary<HeroGradeType, HeroDataSO>());

        foreach (var heroID in ownedHeroIDs)
        {
            var so = ResourceManager.Instance.Load<HeroDataSO>($"{HERO_DATA_RESOURCE_PATH}{heroID}");
            ownedHeroes[so.ClassType].Add(so.GradeType, so);
        }

        currentHeroLevels.Clear();
        foreach (var heroLevel in data.HeroLevels)
        {
            currentHeroLevels.Add(heroLevel.Key, heroLevel.Value);
        }
        
        currentHeroStacks.Clear();
        foreach (var heroStack in data.OwnedHeroStacks)
        {
            currentHeroStacks.Add(heroStack.Key, heroStack.Value);
        }
    }
}