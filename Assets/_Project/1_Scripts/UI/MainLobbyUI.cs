using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MainLobbyUI : BaseUI
{
    //TODO: 임시
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private GameStartButtonComponent gameStartButton;
    [SerializeField] private LobbyBottomButtonGroups bottomButtonGroups;

    protected override void Opened(params object[] args)
    {
        //TODO: 임시
        userNameText.text = PlayerDataManager.Instance.ProfileSaveData.PlayerName;
        goldText.text = PlayerDataManager.Instance.CurrencySaveData.Gold.ToString();
        levelText.text = PlayerDataManager.Instance.ProfileSaveData.Level.ToString();
    }

    protected override void Closed(params object[] args)
    {
    }

    //TODO: 임시로 작성
    public void TmpDeleteData()
    {
#if FIREBASE_ENABLED
        FirebaseManager.Instance.DeleteUserAsync().Forget();
#else
        SaveLoadManager.Instance.DeleteAsync().Forget();
#endif
    }
}