/*
Simple Sound Manager (c) 2016 Digital Ruby, LLC
http://www.digitalruby.com

Source code may no longer be redistributed in source format. Using this in apps and games is fine.
*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;

// Be sure to add this using statement to your scripts
// using DigitalRuby.SoundManagerNamespace

namespace DigitalRuby.SoundManagerNamespace
{
    public class SoundManagerDemo : MonoBehaviour
    {
        public static SoundManagerDemo Instance;
        public Slider SoundSlider;
        public Slider MusicSlider;
        //public InputField SoundCountTextBox;
        //public bool PersistToggle;
        int currentMusic;
        public AudioSource[] SoundAudioSources;
        public AudioSource[] MusicAudioSources;
        
        private void PlaySound(int index)
        {
            SoundAudioSources[index].PlayOneShotSoundManaged(SoundAudioSources[index].clip);
        }

        private void PlayMusic(int index)
        {
            MusicAudioSources[index].PlayOneShotMusicManaged(MusicAudioSources[index].clip);
        }
        private void Awake()
        {
            
            DontDestroyOnLoad(this.gameObject);
            int objectsCount = FindObjectsOfType<SoundManagerDemo>().Length;

            if (objectsCount > 1)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            if (Instance == null)
            {
                Instance = this;
            }

            
        }
        public void FindSliders()
        {
            MusicSlider = Settings.Instance.musicSlider;
            SoundSlider = Settings.Instance.soundSlider;
            MusicSlider.value = SoundManager.MusicVolume;
            SoundSlider.value = SoundManager.SoundVolume;
        }
        private void Update()
        {
            bool musicIsPlaying = false;
            foreach (AudioSource a in MusicAudioSources)
            {
                if (a.isPlaying)
                    musicIsPlaying = true;
            }
            if (!musicIsPlaying)
            {
                if (currentMusic < MusicAudioSources.Length - 1)
                    currentMusic++;
                else
                    currentMusic = 0;
                PlayMusic(currentMusic);
            }
            if (Settings.Instance != null)
                MusicVolumeChanged();          
            if(Settings.Instance != null)
                SoundVolumeChanged();

        }

        public void ButtonPressSound()
        {
            PlaySound(0);
        }
        public void AkShot()
        {
            PlaySound(1);
        }
        public void NoAmmo()
        {
            PlaySound(2);

        }
        public void PickUpMeds()
        {
            PlaySound(3);
        }
        public void PickUpAmmo()
        {
            PlaySound(4);
        }
        public void PlayerHit()
        {
            PlaySound(5);
        }
        public void ZombieAttack(int Type)
        {
            PlaySound(6+Type);
        }
        public void ZombieDeath(int type)
        {
            PlaySound(10 + type);
        }
        public void ZombieGetHit(int type)
        {
            PlaySound(14 + type);
        }
        public void SoundVolumeChanged()
        {
            try
            {
                SoundManager.SoundVolume = SoundSlider.value;
            }
            catch
            {
                Debug.Log("Не присвоенный SoundVolume");
            }
        }

        public void MusicVolumeChanged()
        {
            try
            {
                SoundManager.MusicVolume = MusicSlider.value;
            }
            catch
            {
                Debug.Log("Не присвоенный MusicVolume");
            }
            
        }

        public void PersistToggleChanged(bool isOn)
        {
            SoundManager.StopSoundsOnLevelLoad = !isOn;
        }
    }
}
