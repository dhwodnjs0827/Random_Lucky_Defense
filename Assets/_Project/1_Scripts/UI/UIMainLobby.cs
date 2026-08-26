using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// LobbyScene의 첫 등장 기본 UI 클래스
/// </summary>
public class UIMainLobby : UIBase
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
        goldText.text = PlayerDataManager.Instance.Currency[CurrencyType.Gold].ToString();
        levelText.text = PlayerDataManager.Instance.ProfileSaveData.Level.ToString();
    }

    protected override void Closed(params object[] args)
    {
    }

    //TODO: 임시로 작성
    public void TmpDeleteData()
    {
        FirebaseManager.Instance.DeleteUserAsync().Forget();
    }
}