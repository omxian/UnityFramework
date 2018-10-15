using UnityEngine;
using System.Collections;
using Framework.Notify;

public class AudioManager : MonoSingleton<AudioManager>
{
    [Range(0, 1)]
    public float soundVolume = 1f;
    [Range(0, 1)]
    public float bgmVolume = 1f;

    private AudioManager() { }

    private AudioNotifyArg currentBgm = null;

    public override void StartUp()
    {
        NotifyManager.Instance.AddNotify(NotifyIds.PLAY_AUDIO, OnPlayAudio);
    }

    private void OnPlayAudio(NotifyArg arg)
    {
        AudioNotifyArg audioArg = (arg as AudioNotifyArg);
        if (currentBgm != null && currentBgm.IsSame(audioArg))
        {
            return;
        }

        AudioClip audioClip = ResourceManager.Instance.LoadAudioClip(audioArg.AudioName, audioArg.IsBgm, audioArg.Folder);
        AudioSource audioSource = audioArg.IsBgm ? FrameworkRoot.bgmAudioSource : FrameworkRoot.soundAudioSource;
        audioSource.clip = audioClip;
        audioSource.volume = audioArg.IsBgm ? bgmVolume : soundVolume;
        audioSource.loop = audioArg.IsBgm;
        currentBgm = audioArg.IsBgm ? audioArg : currentBgm;
        audioSource.Play();
    }
}
