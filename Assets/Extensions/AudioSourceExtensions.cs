using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions
{
    public static void PlayClip(this AudioSource audioSource, AudioClip clip, float volume = 1, float pitch = 1){
        // Plays an audio clip on an audio source as a one shot, randomly shifted down in pitch, returns the new pitch

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
    }



    public static float PlayClipPitchShifted(this AudioSource audioSource, AudioClip clip, float volume, float minPitchMultiplier, float maxPitchMultiplier){
        // Plays an audio clip on an audio source as a one shot, randomly shifted in pitch either up or down, returns the new pitch
        float newPitch = Random.Range(minPitchMultiplier, maxPitchMultiplier);

        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(clip, volume);
        //audioSource.pitch = 1;

        return newPitch;
    }

    public static float PlayClipPitchShifted(this AudioSource audioSource, AudioClip clip, float volume, float maxPitchMultiplierRange){
        // Plays an audio clip on an audio source as a one shot, randomly shifted in pitch either up or down, returns the new pitch
        float newPitch = Random.Range(1/maxPitchMultiplierRange, maxPitchMultiplierRange);

        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(clip, volume);
        //audioSource.pitch = 1;

        return newPitch;
    }


    public static float PlayClipPitchedUp(this AudioSource audioSource, AudioClip clip, float volume, float maxPitchMultiplier){
        // Plays an audio clip on an audio source as a one shot, randomly shifted up in pitch, returns the new pitch
        float newPitch = Random.Range(1, maxPitchMultiplier);

        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(clip, volume);
        //audioSource.pitch = 1;
        
        return newPitch;
    }


    public static float PlayClipPitchedDown(this AudioSource audioSource, AudioClip clip, float volume, float minPitchMultiplier){
        // Plays an audio clip on an audio source as a one shot, randomly shifted down in pitch, returns the new pitch
        float newPitch = Random.Range(minPitchMultiplier, 1);

        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(clip, volume);
        //audioSource.pitch = 1;
        
        return newPitch;
    }


    public static void StopAudio(this AudioSource audioSource){
        audioSource.loop = false;
        audioSource.clip = null;
        audioSource.Stop();
    }


    public static void PlayLoop(this AudioSource audioSource, AudioClip clip){
        if(clip){
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.Play();
        }
        else{
            audioSource.StopAudio();
        }
    }
    public static void PlayLoop(this AudioSource audioSource, AudioClip clip, float volume){
        if(clip){
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }
        else{
            audioSource.StopAudio();
        }
    }
}
