using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using Newtonsoft.Json;

/// <summary>
/// Firebase Firestore 저장소 사용 핸들러
/// </summary>
public class FirestoreHandler : IDataSaveLoadHandler
{
    private const string COLLECTION_USERS = "users";
    private const string FIELD_SAVE_DATA = "saveData";
    private const string FIELD_UPDATED_AT = "updatedAt";

    private FirebaseFirestore Firestore => FirebaseManager.Instance.Firestore;
    private string UserId => FirebaseManager.Instance.CurrentUser?.UserId;

    public async UniTask SaveAsync(SaveData data)
    {
        if (string.IsNullOrEmpty(UserId))
        {
            CDebug.LogError("[FirestoreHandler] 로그인되지 않았습니다!");
            return;
        }

        try
        {
            var docRef = Firestore.Collection(COLLECTION_USERS).Document(UserId);
            var jsonData = JsonConvert.SerializeObject(data);

            var documentData = new Dictionary<string, object>
            {
                { FIELD_SAVE_DATA, jsonData },
                { FIELD_UPDATED_AT, FieldValue.ServerTimestamp }
            };

            await docRef.SetAsync(documentData, SetOptions.MergeAll);
            CDebug.Log("[FirestoreHandler] 데이터 저장 성공");
        }
        catch (System.Exception e)
        {
            CDebug.LogError($"[FirestoreHandler] 데이터 저장 실패: {e.Message}");
        }
    }

    public async UniTask<SaveData> LoadAsync()
    {
        if (string.IsNullOrEmpty(UserId))
        {
            CDebug.LogError("[FirestoreHandler] 로그인되지 않았습니다!");
            return null;
        }

        try
        {
            var docRef = Firestore.Collection(COLLECTION_USERS).Document(UserId);
            var snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists && snapshot.TryGetValue(FIELD_SAVE_DATA, out string jsonData))
            {
                var saveData = JsonConvert.DeserializeObject<SaveData>(jsonData);
                CDebug.Log("[FirestoreHandler] 데이터 로드 성공");
                return saveData;
            }

            CDebug.Log("[FirestoreHandler] 저장된 데이터가 없습니다");
            return null;
        }
        catch (System.Exception e)
        {
            CDebug.LogError($"[FirestoreHandler] 데이터 로드 실패: {e.Message}");
            return null;
        }
    }

    public async UniTask DeleteAsync()
    {
        if (string.IsNullOrEmpty(UserId))
        {
            CDebug.LogError("[FirestoreHandler] 로그인되지 않았습니다!");
            return;
        }

        try
        {
            var docRef = Firestore.Collection(COLLECTION_USERS).Document(UserId);
            await docRef.DeleteAsync();
            CDebug.Log("[FirestoreHandler] 데이터 삭제 성공");
        }
        catch (System.Exception e)
        {
            CDebug.LogError($"[FirestoreHandler] 데이터 삭제 실패: {e.Message}");
        }
    }
}