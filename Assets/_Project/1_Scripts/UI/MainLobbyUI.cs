using UnityEngine;

public class MainLobbyUI : BaseUI
{
    [SerializeField] private GameStartButtonComponent gameStartButton;
    [SerializeField] private LobbyBottomButtonGroups bottomButtonGroups;
    
    protected override void Opened(params object[] args)
    {
    }

    protected override void Closed(params object[] args)
    {
    }
}
