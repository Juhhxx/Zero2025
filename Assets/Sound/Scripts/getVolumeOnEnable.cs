using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class getVolumeOnEnable : MonoBehaviour
{
    [SerializeField] private int sliderType;
    private SoundMixerManager soundMixerManager;
    private Slider volumeSlider;
    private bool firstActivation = true;
    private float previousVolume;

    private void OnEnable() {

        if(firstActivation) {
            switch (sliderType) {
                case 0:
                    volumeSlider.value = soundMixerManager.GetSavedMasterVolume();
                    break;
                case 1:
                    volumeSlider.value = soundMixerManager.GetSavedSoundFXVolume();
                    break;
                case 2:
                    volumeSlider.value = soundMixerManager.GetSavedMusicVolume();
                    break;
            }

            firstActivation = false;
        }
        else {
            volumeSlider.value = previousVolume;
        }
    }

    private void OnDisable() {
        previousVolume = volumeSlider.value;
    }



    void Awake() {
        volumeSlider = GetComponent<Slider>();
        soundMixerManager = GameObject.FindGameObjectWithTag("SoundMixerManager").GetComponent<SoundMixerManager>();
    }
}
