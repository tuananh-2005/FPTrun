using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioClip _startSound;
    [SerializeField] private AudioClip _randomSound1;
    [SerializeField] private AudioClip _randomSound2;
    [SerializeField] private AudioClip _fixedIntervalSound;

    private bool _isPlaying = false;

    private void Start()
    {
        _soundSource.PlayOneShot(_startSound);
        StartCoroutine(PlayLoopingSounds());
    }

    private IEnumerator PlayLoopingSounds()
    {
        yield return new WaitForSeconds(_startSound.length);
        _isPlaying = true;

        while (_isPlaying)
        {
            AudioClip randomClip = (Random.value > 0.5f) ? _randomSound1 : _randomSound2;
            _soundSource.PlayOneShot(randomClip);

            yield return new WaitForSeconds(randomClip.length + 2f);
            _soundSource.PlayOneShot(_fixedIntervalSound);

            yield return new WaitForSeconds(10f - 2f);
        }
    }

    public void StopSounds()
    {
        _isPlaying = false;
        StopAllCoroutines();
        _soundSource.Stop();
    }
}

