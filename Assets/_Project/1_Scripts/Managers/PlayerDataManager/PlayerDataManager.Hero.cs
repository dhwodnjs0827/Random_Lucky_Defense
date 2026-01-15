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

    /// <summary>
    /// 영웅 획득
    /// </summary>
    public void AcquireHero(HeroRuntimeData acquiredHero, bool isAutoSave = true)
    {
        if (!acquiredHero.IsAcquiredHero)
        {
            acquiredHero.IsAcquiredHero = true;
        }
        else
        {
            acquiredHero.AcquiredStack++;
        }

        if (isAutoSave)
        {
            SaveHeroData();
        }
    }

    /// <summary>
    /// 사용할 영웅 변경
    /// </summary>
    public void ChangeSelectedHero(HeroRuntimeData unequipHero, HeroRuntimeData equipHero, bool isAutoSave = true)
    {
        if (unequipHero != null)
        {
            unequipHero.IsSelected = false;
        }

        if (equipHero != null)
        {
            equipHero.IsSelected = true;
        }

        if (isAutoSave)
        {
            SaveHeroData();
        }
    }

    /// <summary>
    /// 영웅 데이터 저장
    /// </summary>
    private void SaveHeroData()
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

    /// <summary>
    /// 영웅 보유 데미지 증가 계산
    /// </summary>
    /// <returns>보너스 데미지 증가 배율(% 아님!)</returns>
    public float CalculateHeroAcquiredBonusDamage(HeroClassType heroClassType)
    {
        float bonusDamage = 0;
        var acquiredHeroes = allHeroes.Where(hero => hero.Class == heroClassType).Where(hero => hero.IsAcquiredHero)
            .ToArray();
        foreach (var hero in acquiredHeroes)
        {
            switch (hero.Rank)
            {
                case HeroRankType.B:
                    bonusDamage += hero.Level * GameConstants.RANK_B_ACQUIRED_BONUS_DAMAGE;
                    break;
                case HeroRankType.A:
                    bonusDamage += hero.Level * GameConstants.RANK_A_ACQUIRED_BONUS_DAMAGE;
                    break;
                case HeroRankType.S:
                    bonusDamage += hero.Level * GameConstants.RANK_S_ACQUIRED_BONUS_DAMAGE;
                    break;
            }
        }

        return bonusDamage;
    }
}