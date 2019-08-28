#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Reflection;

public class TimelineSelect : MonoBehaviour
{
    [SerializeField] private PlayableDirector director = null;

    [ContextMenu("Select All")]
    private void SelectAll()
    {
        var timelineAsset = (TimelineAsset)director?.playableAsset;
        if (timelineAsset == null)
        {
            Debug.LogWarning("TimelineがNullです");
            return;
        }
        Type type = Type.GetType("UnityEditor.Timeline.EditorClip, UnityEditor.Timeline");
        var objectList = new List<UnityEngine.Object>();
        foreach (var track in timelineAsset.GetOutputTracks())
        {
            foreach (var clip in track.GetClips())
            {
                UnityEngine.Object editorclip = ScriptableObject.CreateInstance(type);
                PropertyInfo mclp = type.GetProperty("clip");
                mclp.SetValue(editorclip, clip);
                objectList.Add(editorclip);
            }
        }
        Selection.objects = objectList.ToArray();
    }
}
#endif