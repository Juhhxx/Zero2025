using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    [SerializeField] private AudioClip errorSoundFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXResource(AudioResource audioResource, Transform spawnTransform, float volume, float length)
    {
        Debug.Log(spawnTransform.name);
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.resource = audioResource;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(audioSource.gameObject, length);
    }

    public void PlayErrorSoundFX(Transform spawnTransform, float volume)
    {
        PlaySoundFXClip(errorSoundFX, spawnTransform, volume);
    }
}
