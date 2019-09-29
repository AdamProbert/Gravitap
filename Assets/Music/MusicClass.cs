using UnityEngine;

public class MusicClass : MonoBehaviour
{
    public static MusicClass instance;
    private AudioSource _audioSource;

    // Needs to be a singleton to allow only one instance of the background music to play
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
