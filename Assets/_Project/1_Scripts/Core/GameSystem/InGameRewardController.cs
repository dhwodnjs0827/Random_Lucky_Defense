using System;

public class InGameRewardController : IEventListener
{
    //TODO: RewardData 아직 없음
    //private RewardDataSO rewardData;

    private const string REWARD_DATA_SO_PATH = "Data/SO/RewardData";
    
    private Action<InGameFinishEventData> onGameFinish;
    
    public InGameRewardController()
    {
        //TODO: RewardData 아직 없음
        //rewardData = ResourceManager.Instance.LoadAll<RewardDataSO>(REWARD_DATA_SO_PATH);
    }
    
    #region IEventListener implementation
    
    public void SubscribeEvents()
    {
        onGameFinish += ProcessReward;
        EventManager.Subscribe(GameEventType.GameFinish, onGameFinish);
    }

    public void UnsubscribeEvents()
    {
        onGameFinish -= ProcessReward;
        EventManager.Unsubscribe(GameEventType.GameFinish, onGameFinish);
    }
    
    #endregion

    private void ProcessReward(InGameFinishEventData evnetData)
    {
        
    }
}
