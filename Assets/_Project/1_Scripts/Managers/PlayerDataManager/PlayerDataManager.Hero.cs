using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

public partial class PlayerDataManager
{
    private List<HeroRuntimeData> allHeroes = new();
    
    public IList<HeroRuntimeData> AllHeroes => allHeroes;
    
    /// <summary>
    /// 초기 선택 영웅 데이터 초기화
    /// </summary>
    private void InitializeHeroData(HeroSaveData data)
    {
        foreach (var hero in data.AllHeroes)
        {
            allHeroes.Add(hero.Convert());
        }
    }

    public void AcquireHero(int acquiredHeroID, bool isAutoSave = true)
    {
        var index = allHeroes.FindIndex(hero => hero.ID == acquiredHeroID);
        if (index < 0) return;
        
        if (!allHeroes[index].IsAcquiredHero)
        {
            allHeroes[index].IsAcquiredHero = true;
        }
        else
        {
            allHeroes[index].AcquiredStack++;
        }

        if (isAutoSave)
        {
            SaveHeroData();
        }
    }

    public void ChangeSelectedHero(int currentHeroID, int newHeroID, bool isAutoSave = true)
    {
        var currentIndex = allHeroes.FindIndex(hero => hero.ID == currentHeroID);
        var newIndex = allHeroes.FindIndex(hero => hero.ID == newHeroID);
        if (currentIndex < 0 || newIndex < 0) return;

        allHeroes[currentIndex].IsSelected = false;
        allHeroes[newIndex].IsSelected = true;

        if (isAutoSave)
        {
            SaveHeroData();
        }
    }

    public HeroRuntimeData GetHeroData(HeroClassType heroClass, HeroGradeType heroGrade, HeroRankType heroRank)
    {
        return allHeroes.Find(hero => hero.Class == heroClass && hero.Grade == heroGrade && hero.Rank == heroRank);
    }
    
    public void SaveHeroData()
    {
        var saveData = SaveLoadManager.Instance.SaveData;
        List<PlayerHeroSaveData> playerHeroSaveData = new List<PlayerHeroSaveData>();
        foreach (var hero in allHeroes)
        {
            playerHeroSaveData.Add(hero.Convert());
        }
        saveData.HeroData.AllHeroes = playerHeroSaveData;
        SaveLoadManager.Instance.SaveAsync(saveData).Forget();
    }
}