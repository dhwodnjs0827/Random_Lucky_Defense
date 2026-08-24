using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

/// <summary>
/// iOS 빌드 시 GoogleService-Info.plist의 REVERSED_CLIENT_ID를 Info.plist의 URL Scheme으로 자동 등록.
/// 이 등록이 없으면 Google Sign-In 로그인 후 콜백이 앱으로 돌아오지 못함.
/// </summary>
public static class GoogleSignInIOSPostProcessBuild
{
    private const string GOOGLE_SERVICE_INFO_PLIST_NAME = "GoogleService-Info.plist";
    private const string URL_SCHEME_NAME = "google-signin";

    [PostProcessBuild(100)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        string sourcePlistPath = Path.Combine(Application.dataPath, GOOGLE_SERVICE_INFO_PLIST_NAME);
        if (!File.Exists(sourcePlistPath))
        {
            Debug.LogError($"[GoogleSignInIOSPostProcessBuild] {GOOGLE_SERVICE_INFO_PLIST_NAME} 파일을 찾을 수 없습니다: {sourcePlistPath}");
            return;
        }

        var sourcePlist = new PlistDocument();
        sourcePlist.ReadFromFile(sourcePlistPath);
        PlistElement reversedClientIdElement = sourcePlist.root["REVERSED_CLIENT_ID"];
        if (reversedClientIdElement == null)
        {
            Debug.LogError($"[GoogleSignInIOSPostProcessBuild] {GOOGLE_SERVICE_INFO_PLIST_NAME}에서 REVERSED_CLIENT_ID를 찾을 수 없습니다.");
            return;
        }

        string reversedClientId = reversedClientIdElement.AsString();

        string infoPlistPath = Path.Combine(buildPath, "Info.plist");
        var infoPlist = new PlistDocument();
        infoPlist.ReadFromFile(infoPlistPath);

        PlistElementArray urlTypes = infoPlist.root.values.TryGetValue("CFBundleURLTypes", out PlistElement existing)
            ? existing.AsArray()
            : infoPlist.root.CreateArray("CFBundleURLTypes");

        PlistElementDict urlTypeDict = urlTypes.AddDict();
        urlTypeDict.SetString("CFBundleURLName", URL_SCHEME_NAME);
        PlistElementArray urlSchemes = urlTypeDict.CreateArray("CFBundleURLSchemes");
        urlSchemes.AddString(reversedClientId);

        infoPlist.WriteToFile(infoPlistPath);

        Debug.Log($"[GoogleSignInIOSPostProcessBuild] URL Scheme 등록 완료: {reversedClientId}");
    }
}
