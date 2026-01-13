using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public partial class PlayerDataManager
{
    private const string HERO_DATA_RESOURCE_PATH = "Data/SO/HeroData/";
    
    private List<HeroGameData> acquiredHeroes = new();
    
    public IList<HeroGameData> AcquiredHeroes => acquiredHeroes;
    
    /// <summary>
    /// 초기 선택 영웅 데이터 초기화
    /// </summary>
    private void InitializeHeroData(HeroSaveData data)
    {
        acquiredHeroes = new(data.AcquiredHeroes);
    }

    public void AcquireHero(int acquiredHeroID, bool isAutoSave = true)
    {
        var index = acquiredHeroes.FindIndex(hero => hero.ID == acquiredHeroID);
        if (index < 0) return;

        var heroGameData = acquiredHeroes[index];
        if (!heroGameData.IsAcquiredHero)
        {
            heroGameData.IsAcquiredHero = true;
        }
        else
        {
            heroGameData.AcquiredStack++;
        }
        acquiredHeroes[index] = heroGameData;

        if (isAutoSave)
        {
            SaveHeroData();
        }
    }

    public void ChangeSelectedHero(int currentHeroID, int newHeroID, bool isAutoSave = true)
    {
        var currentIndex = acquiredHeroes.FindIndex(hero => hero.ID == currentHeroID);
        var newIndex = acquiredHeroes.FindIndex(hero => hero.ID == newHeroID);
        if (currentIndex < 0 || newIndex < 0) return;

        var currentHero = acquiredHeroes[currentIndex];
        currentHero.isSelected = false;
        acquiredHeroes[currentIndex] = currentHero;

        var newHero = acquiredHeroes[newIndex];
        newHero.isSelected = true;
        acquiredHeroes[newIndex] = newHero;

        if (isAutoSave)
        {
            SaveHeroData();
        }
    }

    public HeroGameData GetHeroData(HeroClassType heroClass, HeroGradeType heroGrade, HeroRankType heroRank)
    {
        return acquiredHeroes.Find(hero => hero.Class == heroClass && hero.Grade == heroGrade && hero.Rank == heroRank);
    }
    
    public void SaveHeroData()
    {
        var saveData = SaveLoadManager.Instance.SaveData;
        saveData.HeroData.AcquiredHeroes = acquiredHeroes;
        SaveLoadManager.Instance.SaveAsync(saveData).Forget();
    }
}