using UnityEngine;

namespace Engine.GameSections
{
    public class AudioEngine : MonoBehaviour
    {
        public AudioSource gunAudio;
        public AudioSource zombiesAudioSource;
        public static AudioEngine audioEngine;
        public AudioClip upgradeWeaponGate;
        public AudioClip zombieHordeSound;
        public AudioClip bulletHitAudioClip;
        public bool isMusicPlay;
        public bool isHaptic;

        private void Awake()
        {
            audioEngine = this;
        }

        private void Start()
        {
            //FalconMediationCore.InitCore(settings, OnMediationInitialized);
        }
        public void SpecialFunc()
        {
            
        }
    }
}