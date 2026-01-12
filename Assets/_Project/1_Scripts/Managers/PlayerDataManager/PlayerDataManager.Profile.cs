public partial class PlayerDataManager
{
    public ProfileSaveData ProfileSaveData {get; private set;}
    
    private void InitializeProfileData(ProfileSaveData data)
    {
        ProfileSaveData = data;
    }
}