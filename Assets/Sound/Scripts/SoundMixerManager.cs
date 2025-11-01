using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float savedMasterVolume;
    [SerializeField] private float savedSoundFXVolume;
    [SerializeField] private float savedMusicVolume;

    void Awake() {
        savedMasterVolume = PlayerPrefs.GetFloat("masterVolume");

        //Sets the volume to 100% during very first execution
        //Explanation: savedMasterVolume will only be 0f if it doesn't yet exist in PlayerPrefs
        if(savedMasterVolume == 0f) {
            savedMasterVolume = 1f;
            savedSoundFXVolume = 1f;
            savedMusicVolume = 1f;
        }
        else {
        savedSoundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
        savedMusicVolume = PlayerPrefs.GetFloat("musicVolume");
        }
    }

    void Start()
    {
        SetMasterVolume(savedMasterVolume);
        SetSoundFXVolume(savedSoundFXVolume);
        SetMusicVolume(savedMusicVolume);
        
        /* 
        Debug.Log("Saved Master Volume: " + savedMasterVolume);
        Debug.Log("Saved FX Volume: " + savedSoundFXVolume);
        Debug.Log("Saved Music Volume: " + savedMusicVolume);
         */
    }

    public void SetMasterVolume(float level) {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("masterVolume", level);
        PlayerPrefs.Save();
    }
    public void SetSoundFXVolume(float level) {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("soundFXVolume", level);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float level) {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat("musicVolume", level);
        PlayerPrefs.Save();
    }
    public float GetSavedMasterVolume() {
        return savedMasterVolume;
    }
    public float GetSavedSoundFXVolume() {
        return savedSoundFXVolume;
    }
    public float GetSavedMusicVolume() {
        return savedMusicVolume;
    }
}
