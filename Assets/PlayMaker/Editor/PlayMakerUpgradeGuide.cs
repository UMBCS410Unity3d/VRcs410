using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEditor;

namespace HutongGames.PlayMakerEditor
{
    /// <summary>
    /// Shows critical upgrade info for each version
    /// </summary>
    [InitializeOnLoad]
    public class PlayMakerUpgradeGuide : EditorWindow
    {
        private const string urlReleaseNotes = "https://hutonggames.fogbugz.com/default.asp?W311";

        private bool showOnLoad;
        private Vector2 scrollPosition;

        static PlayMakerUpgradeGuide()
        {
            if (EditorPrefs.GetBool("Playmaker.ShowUpgradeGuide", true))
            {
                // Can't call GetWindow here, so use update callback
                EditorApplication.update -= OpenNextUpdate;
                EditorApplication.update += OpenNextUpdate;
            }         
        }

        static void OpenNextUpdate()
        {
            Open();

            EditorApplication.update -= OpenNextUpdate;
        }

        public static void Open()
        {
            GetWindow<PlayMakerUpgradeGuide>(true);
        }

        public void OnEnable()
        {
            title = "PlayMaker";
            position = new Rect(100,100,350,400);
            minSize = new Vector2(350,200);

            showOnLoad = EditorPrefs.GetBool("Playmaker.ShowUpgradeGuide", true);
        }

        public void OnGUI()
        {
            FsmEditorStyles.Init();

            FsmEditorGUILayout.ToolWindowLargeTitle(this, "Upgrade Guide");

            // Hack fix needed in 1.7.8 (fixed in 1.8.0)
            GUILayout.Space(20);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.HelpBox("Always BACKUP projects before updating!", MessageType.Error);

            GUILayout.Label("Version 1.7.8", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("This is a maintainance release for Unity 5 compatibility. " +
                                    "\nNew features and bug fixes coming soon in 1.8.0", MessageType.Info);
            EditorGUILayout.HelpBox("The Playmaker About Window still says 1.7.7 since the dlls were not recompiled for this version.", MessageType.Info);

            GUILayout.Label("Unity 5 Upgrade Notes", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Unity 5 removed component property shortcuts from GameObject. " +
                                    "\n\nThe Unity auto update process replaces these properties with GetComponent calls. " +
                                    "In many cases this is fine, but some third party actions and addons might need manual updating! " +
                                    "Please post on the PlayMaker forums and contact the original authors for help." +
                                    "\n\nIf you used these GameObject properties in Get Property or Set Property actions " +
                                    "they will be gone, and you need to reset them to point to the component directly. " +
                                    "E.g., Drag the component into the property field instead of the GameObject." +
                                    "\n", MessageType.Warning);

            GUILayout.Label("Unity 4.6 Upgrade Notes", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Find support for the new Unity GUI online in our Addons page.", MessageType.Info);
            EditorGUILayout.HelpBox("PlayMakerGUI is only needed if you use OnGUI Actions. " +
                                    "If you don't use OnGUI actions un-check Auto-Add PlayMakerGUI in PlayMaker Preferences.", MessageType.Info);
 

            GUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            EditorGUI.BeginChangeCheck();
            var dontShowAgain = GUILayout.Toggle(!showOnLoad, "Don't Show Again Until Next Update");
            if (EditorGUI.EndChangeCheck())
            {
                showOnLoad = !dontShowAgain;
                EditorPrefs.SetBool("Playmaker.ShowUpgradeGuide", showOnLoad);
            }

            if (GUILayout.Button("Online Release Notes"))
            {
                Application.OpenURL(urlReleaseNotes);
            }
        }
    }
}