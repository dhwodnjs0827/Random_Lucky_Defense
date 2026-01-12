#if FIREBASE_ENABLED
using Firebase.Firestore;

public partial class FirebaseManager
{
    private FirebaseFirestore firestore;

    public FirebaseFirestore Firestore => firestore;
}
#endif