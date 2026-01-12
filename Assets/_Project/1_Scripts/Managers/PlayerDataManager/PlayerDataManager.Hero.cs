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
        var heroGameData = acquiredHeroes[acquiredHeroID];
        if (!heroGameData.IsAcquiredHero)
        {
            heroGameData.IsAcquiredHero = true;
        }
        else
        {
            heroGameData.AcquiredStack++;
        }
        acquiredHeroes[acquiredHeroID] = heroGameData;

        if (isAutoSave)
        {
            SaveHeroData();
        }
    }

    public void ChangeSelectedHero(int currentHeroID, int newHeroID, bool isAutoSave = true)
    {
         var currentHero = acquiredHeroes[currentHeroID];
         currentHero.isSelected = false;
         acquiredHeroes[currentHeroID] = currentHero;
         
         var newHero = acquiredHeroes[newHeroID];
         newHero.isSelected = true;
         acquiredHeroes[newHeroID] = newHero;
         
         if (isAutoSave)
         {
             SaveHeroData();
         }
    }
    
    public void SaveHeroData()
    {
        var saveData = SaveLoadManager.Instance.SaveData;
        saveData.HeroData.AcquiredHeroes = acquiredHeroes;
        SaveLoadManager.Instance.SaveAsync(saveData).Forget();
    }
}