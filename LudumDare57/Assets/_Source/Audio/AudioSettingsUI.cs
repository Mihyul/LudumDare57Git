using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AudioSystem
{
    public class AudioSettingsUI : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [Space]
        [SerializeField] private AudioGroupVolumeSlider[] audioGroupVolumeSliders = new AudioGroupVolumeSlider[0];

        private readonly Dictionary<Slider, Action<float>> _volumeSlaiderActions = new();

        private void Start()
        {
            AudioSettingsController audioSettingsController = new(audioMixer);

            foreach (var audioGroupVolumeSlider in audioGroupVolumeSliders)
            {
                void SetVolume(float volume)
                {
                    audioSettingsController.SetVolume(audioGroupVolumeSlider.AudioGroupName, volume);
                    PlayerPrefs.SetFloat(audioGroupVolumeSlider.AudioGroupName, volume);
                };

                Action<float> setVolume = new(SetVolume);
                audioGroupVolumeSlider.VolumeSlider.onValueChanged.AddListener(setVolume.Invoke);
                _volumeSlaiderActions.TryAdd(audioGroupVolumeSlider.VolumeSlider, setVolume);

                float volume = PlayerPrefs.GetFloat(audioGroupVolumeSlider.AudioGroupName);
                audioGroupVolumeSlider.VolumeSlider.SetValueWithoutNotify(volume);
                audioSettingsController.SetVolume(audioGroupVolumeSlider.AudioGroupName, volume);
            }
        }

        [Serializable]
        private struct AudioGroupVolumeSlider
        {
            [field: SerializeField] public string AudioGroupName { get; private set; }
            [field: SerializeField] public Slider VolumeSlider { get; private set; }
        }
    }
}
