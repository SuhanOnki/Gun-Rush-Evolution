#if UNITY_IOS
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
public class MyBuildPostprocessor
{
    [PostProcessBuildAttribute]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            string xcodeProjectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);
#if UNITY_2019_3_OR_NEWER
            string xcodeTarget = xcodeProject.GetUnityMainTargetGuid();
#else
            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");
#endif
            HandlePlistIosChanges(pathToBuiltProject);
            xcodeProject.AddFrameworkToProject(xcodeTarget, "AppTrackingTransparency.framework", true);
            xcodeProject.WriteToFile(xcodeProjectPath);
            Debug.Log("Info.plist updated with NSAdvertisingAttributionReportEndpoint");
        }
    }

    private static void HandlePlistIosChanges(string projectPath)
    {
        string plistPath = projectPath + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        PlistElementDict rootDict = plist.root;
        rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://appsflyer-skadnetwork.com/");

        File.WriteAllText(plistPath, plist.WriteToString());
    }
}
#endif