using System.Collections.Generic;
using System.Linq;
using Generated;
using UnityEngine;

public partial class PlayerDataManager
{
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> ownedHeroes = new();
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> selectedHeroes = new();

    public IDictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> OwnedHeroes => ownedHeroes;

    public IDictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> SelectedHeroes => selectedHeroes;

    /// <summary>
    /// 초기 선택 영웅 데이터 초기화
    /// </summary>
    private void InitializeSelectedHeroes()
    {
        var initialGameConfig = Resources.Load<InitialGameConfig>("Data/SO/InitialGameConfig");

        selectedHeroes.Clear();
        selectedHeroes.Add(HeroClassType.Magician, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Archer, new Dictionary<HeroGradeType, HeroDataSO>());
        selectedHeroes.Add(HeroClassType.Warrior, new Dictionary<HeroGradeType, HeroDataSO>());

        selectedHeroes[HeroClassType.Magician] =
            initialGameConfig.StartHeroes.magicians.ToDictionary(k => k.GradeType, v => v);
        selectedHeroes[HeroClassType.Archer] =
            initialGameConfig.StartHeroes.archers.ToDictionary(k => k.GradeType, v => v);
        selectedHeroes[HeroClassType.Warrior] =
            initialGameConfig.StartHeroes.warriors.ToDictionary(k => k.GradeType, v => v);
    }
}