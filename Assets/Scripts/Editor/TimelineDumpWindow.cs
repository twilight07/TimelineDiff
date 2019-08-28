using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace TimelineDiff
{
    public class TimelineDumpWindow : EditorWindow
    {
        [SerializeField] private PlayableDirector director = null;

        [MenuItem("TimelineDiff/DumpWindow")]
        private static void Init()
        {
            var window = GetWindow<TimelineDumpWindow>();
            window.titleContent.text = "TimelineDumpWindow";
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            director = EditorGUILayout.ObjectField(director, typeof(PlayableDirector), true) as PlayableDirector;
            EditorGUILayout.Space();
            if (GUILayout.Button("Dump") && director != null)
            {
                if (director.playableAsset != null)
                {
                    var dump = ScriptableObject.CreateInstance<TimelineDump>();
                    dump.GenerateDumpData((TimelineAsset)director?.playableAsset);
                    AssetDatabase.CreateAsset(dump, "Assets/Sandbox/DumpData/TimelineDump.asset");
                }
            }
        }
    }
}