/// <summary>
/// 구글 계정 연동 결과
/// </summary>
public enum GoogleLinkResult
{
    Success,
    AlreadyInUse, // 이미 다른 계정에 연동된 구글 계정
    Canceled, // 사용자가 구글 로그인 창을 취소
    Failed,
}