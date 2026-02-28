using System.Collections.Generic;
using UnityEngine;

namespace CursedDepths.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        public class AudioEntry
        {
            public string key;
            public AudioClip clip;
            public float volume = 1f;
        }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private List<AudioEntry> music = new List<AudioEntry>();
        [SerializeField] private List<AudioEntry> sfx = new List<AudioEntry>();

        private readonly Dictionary<string, AudioEntry> musicLookup = new Dictionary<string, AudioEntry>();
        private readonly Dictionary<string, AudioEntry> sfxLookup = new Dictionary<string, AudioEntry>();

        public void Initialize()
        {
            foreach (var entry in music)
            {
                if (!musicLookup.ContainsKey(entry.key))
                {
                    musicLookup.Add(entry.key, entry);
                }
            }

            foreach (var entry in sfx)
            {
                if (!sfxLookup.ContainsKey(entry.key))
                {
                    sfxLookup.Add(entry.key, entry);
                }
            }

            PlayMusic("Main");
        }

        public void PlayMusic(string key)
        {
            if (musicSource == null || !musicLookup.TryGetValue(key, out var entry))
            {
                return;
            }

            musicSource.clip = entry.clip;
            musicSource.volume = entry.volume;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlaySfx(string key)
        {
            if (sfxSource == null || !sfxLookup.TryGetValue(key, out var entry))
            {
                return;
            }

            sfxSource.PlayOneShot(entry.clip, entry.volume);
        }
    }
}
