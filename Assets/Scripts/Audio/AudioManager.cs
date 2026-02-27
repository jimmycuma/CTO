using UnityEngine;
using System.Collections.Generic;

namespace CursedDepths.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [System.Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 1f;
            [Range(0.1f, 3f)] public float pitch = 1f;
            public bool loop = false;
            
            [HideInInspector] public AudioSource source;
        }

        public Sound[] musicSounds;
        public Sound[] sfxSounds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            InitializeAudioSources();
        }

        private void InitializeAudioSources()
        {
            GameObject musicObject = new GameObject("Music");
            musicObject.transform.SetParent(transform);
            
            GameObject sfxObject = new GameObject("SFX");
            sfxObject.transform.SetParent(transform);

            foreach (Sound sound in musicSounds)
            {
                sound.source = musicObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }

            foreach (Sound sound in sfxSounds)
            {
                sound.source = sfxObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        public void PlayMusic(string name)
        {
            Sound sound = System.Array.Find(musicSounds, s => s.name == name);
            
            if (sound == null)
            {
                Debug.LogWarning($"Music sound '{name}' not found.");
                return;
            }

            sound.source.Play();
        }

        public void PlaySFX(string name)
        {
            Sound sound = System.Array.Find(sfxSounds, s => s.name == name);
            
            if (sound == null)
            {
                Debug.LogWarning($"SFX sound '{name}' not found.");
                return;
            }

            sound.source.PlayOneShot(sound.clip);
        }

        public void StopMusic(string name)
        {
            Sound sound = System.Array.Find(musicSounds, s => s.name == name);
            
            if (sound == null)
            {
                Debug.LogWarning($"Music sound '{name}' not found.");
                return;
            }

            sound.source.Stop();
        }

        public void StopAllMusic()
        {
            foreach (Sound sound in musicSounds)
            {
                sound.source.Stop();
            }
        }
    }
}
