using UnityEngine;

public class VolumeHandler : MonoBehaviour
{

    // Reference to Audio Source component
    private AudioListener listener;

    // Music volume variable that will be modified
    // by dragging slider knob
    private float musicVolume = PlayerPrefs.GetFloat("volume");

    // Method that is called by slider game object
    // This method takes vol value passed by slider
    // and sets it as musicValue
    public void SetVolume(float vol)
    {
        musicVolume = vol;
        AudioListener.volume = musicVolume;
    }
}