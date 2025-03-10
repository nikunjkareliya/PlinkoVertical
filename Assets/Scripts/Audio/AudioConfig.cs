using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Core
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "ScriptableObjects/Audio")]
    public class AudioConfig : ScriptableObject
    {        
        public List<AudioClipData> audios;
    }

    [System.Serializable]
    public class AudioClipData
    {
        public string clipID;
        public AudioClip clip;
        public bool isLoop;
    }
}