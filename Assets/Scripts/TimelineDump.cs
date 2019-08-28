using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


namespace TimelineDiff
{
    [CreateAssetMenu]
    public class TimelineDump : ScriptableObject
    {
        private static readonly string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public string CreateAt = "";
        public List<TimelineTrackDump> trackDumpList = null;

        public void GenerateDumpData(TimelineAsset timelineAsset)
        {
            CreateAt = DateTime.Now.ToString(DateTimeFormat);
            trackDumpList = new List<TimelineTrackDump>();

            foreach (var track in timelineAsset.GetOutputTracks())
            {
                var trackDump = new TimelineTrackDump();
                //Todo 親子関係を考慮したデータ作成
                trackDump.parentIndex = -1;
                trackDump.TrackName = track.name;
                trackDump.SetTrackType(track);
                trackDump.clipDumpList = track.GetClips().Select(x => new TimelineClipDump(x)).ToList();
                trackDumpList.Add(trackDump);
            }
        }
    }

    [Serializable]
    public class TimelineTrackDump
    {
        [SerializeField] private string trackType = "";
        public string TrackName = "";
        public int parentIndex = -1;
        public List<TimelineClipDump> clipDumpList = null;

        public bool SetTrackType(TrackAsset track)
        {
            if (track == null)
            {
                trackType = "";
                return false;
            }
            else
            {
                trackType = track.GetType().ToString();
                return true;
            }
        }

        public Type GetTrackType()
        {
            var type = Type.GetType(trackType);
            if (type.IsSubclassOf(typeof(TrackAsset)))
            {
                return type;
            }
            else
            {
                return null;
            }
        }
    }

    [Serializable]
    public class TimelineClipDump
    {
        public ClipObjectMetaData objectMetaData = default;
        public TimelineClip timelineClip = default;
        public TimelineClipDump(TimelineClip clip)
        {
            objectMetaData = new ClipObjectMetaData(clip.asset);
            timelineClip = clip;
        }
    }

    [Serializable]
    public struct ClipObjectMetaData
    {
        public bool IsNull;
        public string Name;
        public int HashCode;
        public ClipObjectMetaData(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                IsNull = true;
                Name = "";
                HashCode = 0;
            }
            else
            {
                IsNull = false;
                Name = asset.name;
                HashCode = asset.GetHashCode();
            }
        }
    }
}