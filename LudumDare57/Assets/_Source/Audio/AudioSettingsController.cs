using System;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem
{
    public class AudioSettingsController
    {
        private readonly AudioMixer _audioMixer;

        public AudioSettingsController(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer != null ? audioMixer : throw new ArgumentNullException(nameof(audioMixer));
        }

        public void SetVolume(string audioGroupName, float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1.0f);
            _audioMixer.SetFloat(audioGroupName, Mathf.Log10(volume) * 20);
        }
    }
}
