using UnityEngine;
using System.Collections;

public class AudioPlay : MonoBehaviour
{

    public class SourceWithTime
    {
        public AudioSource source;
        public float time;
        public SourceWithTime(AudioSource source, float time)
        {
            this.source = source;
            this.time = time;
        }
    }

    public void PlaySound(AudioSource _source, AudioClip _sound)
    {
        if (_source != null && _sound != null)
        {
            _source.Stop();
            _source.clip = _sound;
            _source.Play();
        }
    }

    public void PlayRandomSound(AudioSource _source, AudioClip[] _sounds)
    {
        int randomNum = UnityEngine.Random.Range(0, _sounds.Length);
        Debug.Log("Random: " + randomNum);
        Debug.Log("length: " + _sounds.Length);
        AudioClip _sound = null;
        if (_sounds.Length > 0)
        {
            _sound = _sounds[randomNum];
        }
        
        if (_source != null && _sound != null)
        {
            _source.Stop();
            _source.clip = _sound;
            _source.Play();
        }
    }

    public void PlayLoopedSoundForTime(AudioSource _source, AudioClip _sound, float time)
    {
        if (_source != null && _sound != null)
        {
            _source.Stop();
            _source.clip = _sound;
            SourceWithTime soundWT = new SourceWithTime(_source, time);
            StartCoroutine("PlayForTime", soundWT);
        }
    }

    public IEnumerator PlayForTime(SourceWithTime soundWT)
    {
        soundWT.source.loop = true;
        soundWT.source.Play();
        yield return new WaitForSeconds(soundWT.time);
        soundWT.source.Stop();
        soundWT.source.loop = false;
    }
}