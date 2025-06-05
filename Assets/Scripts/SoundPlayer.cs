using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private bool played = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        audioSource.clip = Resources.Load<AudioClip>(name);
        audioSource.Play();
        played = true;
    }

    private void Update()
    {
        if (played && !audioSource.isPlaying)
        {
            Destroy(this);
        }
    }
}