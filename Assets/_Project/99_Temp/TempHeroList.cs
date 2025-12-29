using Generated;
using UnityEngine;
using UnityEngine.UI;

public class TempHeroList : MonoBehaviour
{
    [SerializeField] Button saveButton;
    
    [Header("선택한 마법사 데이터")]
    [SerializeField] private HeroDataSO normalMagician;
    [SerializeField] private HeroDataSO superiorMagician;
    [SerializeField] private HeroDataSO rareMagician;
    [SerializeField] private HeroDataSO ancientMagician;
    [SerializeField] private HeroDataSO relicMagician;
    [SerializeField] private HeroDataSO legendMagician;
    [SerializeField] private HeroDataSO epicMagician;
    [SerializeField] private HeroDataSO mythMagician;
    [SerializeField] private HeroDataSO godMagician;
    
    [Header("선택한 궁수 데이터")]
    [SerializeField] private HeroDataSO normalArcher;
    [SerializeField] private HeroDataSO superiorArcher;
    [SerializeField] private HeroDataSO rareArcher;
    [SerializeField] private HeroDataSO ancientArcher;
    [SerializeField] private HeroDataSO relicArcher;
    [SerializeField] private HeroDataSO legendArcher;
    [SerializeField] private HeroDataSO epicArcher;
    [SerializeField] private HeroDataSO mythArcher;
    [SerializeField] private HeroDataSO godArcher;
    
    [Header("선택한 전사 데이터")]
    [SerializeField] private HeroDataSO normalWarrior;
    [SerializeField] private HeroDataSO superiorWarrior;
    [SerializeField] private HeroDataSO rareWarrior;
    [SerializeField] private HeroDataSO ancientWarrior;
    [SerializeField] private HeroDataSO relicWarrior;
    [SerializeField] private HeroDataSO legendWarrior;
    [SerializeField] private HeroDataSO epicWarrior;
    [SerializeField] private HeroDataSO mythWarrior;
    [SerializeField] private HeroDataSO godWarrior;

    private void Awake()
    {
        saveButton.onClick.AddListener(SaveSelectedHeroData);
    }

    private void SaveSelectedHeroData()
    {
        var manager = PlayerDataManager.Instance;
        
        // 마법사
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = normalMagician.ClassType,
            HeroGradeType = normalMagician.GradeType,
            HeroDataSO = normalMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = superiorMagician.ClassType,
            HeroGradeType = superiorMagician.GradeType,
            HeroDataSO = superiorMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = rareMagician.ClassType,
            HeroGradeType = rareMagician.GradeType,
            HeroDataSO = rareMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = ancientMagician.ClassType,
            HeroGradeType = ancientMagician.GradeType,
            HeroDataSO = ancientMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = relicMagician.ClassType,
            HeroGradeType = relicMagician.GradeType,
            HeroDataSO = relicMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = legendMagician.ClassType,
            HeroGradeType = legendMagician.GradeType,
            HeroDataSO = legendMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = epicMagician.ClassType,
            HeroGradeType = epicMagician.GradeType,
            HeroDataSO = epicMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = mythMagician.ClassType,
            HeroGradeType = mythMagician.GradeType,
            HeroDataSO = mythMagician
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = godMagician.ClassType,
            HeroGradeType = godMagician.GradeType,
            HeroDataSO = godMagician
        });
        
        // 궁수
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = normalArcher.ClassType,
            HeroGradeType = normalArcher.GradeType,
            HeroDataSO = normalArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = superiorArcher.ClassType,
            HeroGradeType = superiorArcher.GradeType,
            HeroDataSO = superiorArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = rareArcher.ClassType,
            HeroGradeType = rareArcher.GradeType,
            HeroDataSO = rareArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = ancientArcher.ClassType,
            HeroGradeType = ancientArcher.GradeType,
            HeroDataSO = ancientArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = relicArcher.ClassType,
            HeroGradeType = relicArcher.GradeType,
            HeroDataSO = relicArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = legendArcher.ClassType,
            HeroGradeType = legendArcher.GradeType,
            HeroDataSO = legendArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = epicArcher.ClassType,
            HeroGradeType = epicArcher.GradeType,
            HeroDataSO = epicArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = mythArcher.ClassType,
            HeroGradeType = mythArcher.GradeType,
            HeroDataSO = mythArcher
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = godArcher.ClassType,
            HeroGradeType = godArcher.GradeType,
            HeroDataSO = godArcher
        });
        
        // 전사
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = normalWarrior.ClassType,
            HeroGradeType = normalWarrior.GradeType,
            HeroDataSO = normalWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = superiorWarrior.ClassType,
            HeroGradeType = superiorWarrior.GradeType,
            HeroDataSO = superiorWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = rareWarrior.ClassType,
            HeroGradeType = rareWarrior.GradeType,
            HeroDataSO = rareWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = ancientWarrior.ClassType,
            HeroGradeType = ancientWarrior.GradeType,
            HeroDataSO = ancientWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = relicWarrior.ClassType,
            HeroGradeType = relicWarrior.GradeType,
            HeroDataSO = relicWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = legendWarrior.ClassType,
            HeroGradeType = legendWarrior.GradeType,
            HeroDataSO = legendWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = epicWarrior.ClassType,
            HeroGradeType = epicWarrior.GradeType,
            HeroDataSO = epicWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = mythWarrior.ClassType,
            HeroGradeType = mythWarrior.GradeType,
            HeroDataSO = mythWarrior
        });
        manager.SaveSelectedHeroData(new SelectedHeroData
        {
            HeroClassType = godWarrior.ClassType,
            HeroGradeType = godWarrior.GradeType,
            HeroDataSO = godWarrior
        });
        
        CDebug.Log("[TempHeroList] 데이터 저장!");
    }
}
