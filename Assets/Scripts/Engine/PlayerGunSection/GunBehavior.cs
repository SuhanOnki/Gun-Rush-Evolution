using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Engine.Bullet;
using Engine.GameSections;
using Engine.MergeGamePlaySection;
using Engine.ObjectCreatorDatas;
using Engine.ObstacleDataSection.FinishObjects;
using MoreMountains.NiceVibrations;
using UISection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Engine.PlayerGunSection
{
    public class GunBehavior : MonoBehaviour
    {
        public static UnityEvent AttackChange = new UnityEvent();

        [SerializeField] private GameObject arrowShootPrefab;
        [SerializeField] private Transform arrowsTransform;
        public Vector3 zombiePosition;
        [SerializeField] private int countArrows;
        public GameObject[] arrowsPrefab;
        public ParticleSystem upgrade;

        [SerializeField] private GameObject[] guns;
        public static GunBehavior gunBehavior;
        [SerializeField] private Pistol[] pistols;
        [SerializeField] private GameObject finishZonePanel;


        public ParticleSystem particle;
        public ParticleSystem upgradeParticle;
        public float zOffset;
        private Transform arrowStartPositions;
        public Vector3 offest;
        public Vector3 recordCameraOffset;
        public GameObject finishZombieCamera;
        public float animationSpeed;
        public GameObject finishStoneCamera;
        [FormerlySerializedAs("winFinish")] public FinishDataUI winFinishData;
        public int partIndex;
        private int arrowsIndex;
        private GameplayMaestro gameplayMaestro;
        public AudioClip[] gunShotsAudios;
        public ObjectPoolerCreator moneyPooler;
        [FormerlySerializedAs("moneyPrefab")] public StoneMoney stoneMoneyPrefab;
        public GameObject fogs;
        public float widthOffset;
        private float _timer;
        private bool attack;
        public GameObject aim;
        public Animator animator;
        private int shootCount;
        public List<GunPartUpgradeData> partDatas;
        [FormerlySerializedAs("currentPart")] public GunPartUpgradeData currentGunPartUpgrade;
        public bool reBoot;
        [FormerlySerializedAs("currentGunData")] public GunBehaviorData currentGunBehaviorData;
        public int[] gunPartsCountEveryGun;

        public ObjectPoolerCreator arrowPooler;

        //private SoundManager soundManager;
        private int gunLevel;

        public void SpecialFunc()
        {
            
        }
        private void Awake()
        {
            gunBehavior = this;
            gunLevel = PlayerPrefs.GetInt("GunLevel", 1);
            var index = guns[gunLevel - 1].gameObject.GetComponent<GunBehaviorData>().index;
            arrowShootPrefab = arrowsPrefab[index];
            countArrows *= 2;
        }

        private IEnumerator Start()
        {
            AttackChange.AddListener(AttackChanger);
            gameplayMaestro = GameplayMaestro.Instance;
            //soundManager = SoundManager.soundManager;
            GunPartInit();
            UploadGun();
            MMVibrationManager.SetHapticsActive(true);
            //CreateArrows();
            //SwipeDetection.SwipeEvent += OnSwipe;
            //StartCoroutine(ShootArrow());
            yield return null;

            arrowPooler = ObjectPoolerCreator.CreateInstance(arrowShootPrefab.gameObject.GetComponent<Bullet.Bullet>(), countArrows,
                arrowsTransform,
                null, false);

            yield return null;
            moneyPooler = ObjectPoolerCreator.CreateInstance(stoneMoneyPrefab, Random.Range(15, 20),
                arrowsTransform,
                null, false);
        }

        public void GunPartInit()
        {
            int gunLevel = PlayerPrefs.GetInt("GunLevel", 1);

            partDatas.Clear();
            for (var i = 0; i < guns.Length; i++)
            {
                var partData = new GunPartUpgradeData();

                partDatas.Add(partData);
                int defaultValue = 0;
                partDatas[i].partsUpgrade = new List<int>();

                for (int j = 0; j < gunPartsCountEveryGun[i]; j++)
                {
                    partDatas[i].partsUpgrade.Add(defaultValue);
                }
            }

            int indexGunParts = 0;
            for (int i = 0; i < gunLevel - 1; i++)
            {
                indexGunParts += gunPartsCountEveryGun[i];
            }

            for (var i = 0; i < gunLevel; i++)
            {
                var partData = partDatas[i];
                for (var i1 = 0; i1 < partData.partsUpgrade.Count; i1++)
                {
                    partData.partsUpgrade[i1] = PlayerPrefs.GetInt($"{i} Weapon part {i1}", 0);
                }
            }

            currentGunPartUpgrade = partDatas[gunLevel - 1];
            int number = 0;
            int particleIndex = 0;
            for (var i = 0; i < currentGunPartUpgrade.partsUpgrade.Count; i++)
            {
                guns[gunLevel - 1].GetComponent<GunBehaviorData>().gameObject.SetActive(true);
                if (currentGunPartUpgrade.partsUpgrade[i] == 1)
                {
                    /*
                                var transformLocalPosition =
                                    guns[gunLevel - 1].GetComponent<GunsData>().particleStm.transform.localPosition;
                                guns[gunLevel - 1].GetComponent<GunsData>().particleStm.transform.localPosition =
                                    new Vector3(transformLocalPosition.x, transformLocalPosition.y, 0.4f);*/
                    var gunData = guns[gunLevel - 1].gameObject.GetComponent<GunBehaviorData>();
                    if (gunData.parts1[i].gameObject.GetComponent<GunPartCharacter>())
                    {
                        particleIndex = currentGunPartUpgrade.partsUpgrade[i];
                    }

                    gunData.parts1[i].transform.gameObject.SetActive(true);
                }
                else if (currentGunPartUpgrade.partsUpgrade[i] == 2)
                {
                    var gunData = guns[gunLevel - 1].gameObject.GetComponent<GunBehaviorData>();
                    gunData.parts2[i].transform.gameObject.SetActive(true);
                    if (gunData.parts2[i].gameObject.GetComponent<GunPartCharacter>())
                    {
                        particleIndex = currentGunPartUpgrade.partsUpgrade[i];
                    }
                }
                else if (currentGunPartUpgrade.partsUpgrade[i] >= 3)
                {
                    var gunData = guns[gunLevel - 1].gameObject.GetComponent<GunBehaviorData>();
                    gunData.parts3[i].transform.gameObject.SetActive(true);
                    if (gunData.parts3[i].gameObject.GetComponent<GunPartCharacter>())
                    {
                        particleIndex = currentGunPartUpgrade.partsUpgrade[i];
                    }
                    //particleIndex = guns[gunLevel - 1].gameObject.GetComponent<GunsData>().offsets.Length - 1;
                }
                /*
            else if (currentPart.partsUpgrade[i] >=
                     guns[gunLevel - 1].gameObject.GetComponent<GunsData>().offsets.Length)
            {
                var gunData = guns[gunLevel - 1].gameObject.GetComponent<GunsData>();
                gunData.parts3[i].transform.gameObject.SetActive(true);
                particleIndex = guns[gunLevel - 1].gameObject.GetComponent<GunsData>().offsets.Length - 1;
            }*/
                else if (currentGunPartUpgrade.partsUpgrade[i] == 0)
                {
                    number++;
                }

                if (currentGunPartUpgrade.partsUpgrade.Count - 1 == i)
                {
                    if (number == currentGunPartUpgrade.partsUpgrade.Count)
                    {
                        var gunData = guns[gunLevel - 1].gameObject.GetComponent<GunBehaviorData>();
                        gunData.setDefaultOffset = true;
                    }
                }
            }

            var gun = guns[gunLevel - 1].gameObject.GetComponent<GunBehaviorData>();
            currentGunBehaviorData = gun;
            BoxCollider component = GunMovement.gunMovement.gameObject.GetComponent<BoxCollider>();
            if (particleIndex == 0)
            {
                if (currentGunBehaviorData.sizeWithoutMuffer == Vector3.zero && currentGunBehaviorData.centerWithoutMuffer == Vector3.zero)
                {
                    component.size = currentGunBehaviorData.size;
                    component.center = currentGunBehaviorData.center;
                }
                else
                {
                    component.size = currentGunBehaviorData.sizeWithoutMuffer;
                    component.center = currentGunBehaviorData.centerWithoutMuffer;
                }
            }
            else
            {
                component.size = currentGunBehaviorData.size;
                component.center = currentGunBehaviorData.center;
            }
            /*
        var originScale = currentGunData.transform.localScale;
        currentGunData.transform.localScale = Vector3.one;*/
            //currentGunData.transform.localScale = originScale;

            gun.gunBehavior = this;
            gun.InitGunData(particleIndex);
        }

        private void Update()
        {
            finishStoneCamera.transform.position = transform.position + recordCameraOffset;
            var targetPosition = transform.position + offest;
            targetPosition.x = Mathf.Clamp(targetPosition.x, 0f, 0f);
            finishZombieCamera.transform.position = targetPosition;
            if (gameplayMaestro.playerState == GameplayMaestro.State.Win || gameplayMaestro.playerState == GameplayMaestro.State.Lose ||
                reBoot)
            {
                if (zombiePosition != Vector3.zero)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, zombiePosition.z);
                }

                animator.SetBool("Stay", true);
                animator.SetBool("Fire", false);
                return;
            }

            if ((gameplayMaestro.playerState == GameplayMaestro.State.Finish) &&
                gameplayMaestro.levelType == GameplayMaestro.LevelType.KillZombie)
            {
                if ( /*_timer >= 45f / gameManager.rateGateArrow &&*/ gameplayMaestro.playerState != GameplayMaestro.State.Lose &&
                                                                      gameplayMaestro.playerState != GameplayMaestro.State.Win)
                {
                    //_timer = 0;
                    //ShootBullet();
                    if (gameplayMaestro.amountZombies < 3)
                    {
                        AudioEngine.audioEngine.gunAudio.Stop();
                        gameplayMaestro.canZombieSoundEffect = true;
                    }

                    animator.SetBool("Stay", false);
                    animator.SetBool("Fire", true);
                }
                else
                {
                    animator.SetBool("Stay", true);
                    animator.SetBool("Fire", false);
                }

                /*_timer += Time.deltaTime;*/
            }
        }

        private void Shoot()
        {
            if (_timer >= 45f / gameplayMaestro.rateGateArrow)
            {
                shootCount++;
                _timer = 0;
                particle.Play();
                animator.SetTrigger("Attack");
                var arrows = arrowPooler.GetObject();
                arrows.transform.position = arrowStartPositions.position;
                arrows.transform.rotation = transform.localRotation;

                Ray ray = Camera.main.ScreenPointToRay(aim.transform.position);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.collider != null)
                    {
                        var position = raycastHit.point;
                        position.y = transform.position.y;
                        if (position.z < 7)
                        {
                            position.z = 15f;
                            position.x = raycastHit.point.x * zOffset;
                        }

                        var direction = (position - transform.position).normalized;

                        arrows.transform.localRotation = Quaternion.LookRotation(direction);
                        transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.2f)
                            .SetEase(Ease.Linear);
                        arrows.gameObject.SetActive(true);
                        arrowsIndex++;
                        if (arrowsIndex >= countArrows) arrowsIndex = 0;
                    }
                }
            }

            _timer += Time.deltaTime;
        }

        private void UploadGun()
        {
            gameplayMaestro.powerArrow =
                pistols[gunLevel - 1].powerArrow * (int)(float.Parse(PlayerPrefs.GetString("power", "1")) / 2);
            gameplayMaestro.rateGateArrow = pistols[gunLevel - 1].rateGateArrow *
                                        (int)(float.Parse(PlayerPrefs.GetString("FireRate", "1")) / 2);
            if (gameplayMaestro.rateGateArrow > 100)
            {
                gameplayMaestro.rateGateArrow = 100;
            }

            gameplayMaestro.rangeGateArrow = pistols[gunLevel - 1].range *
                                         (int)(float.Parse(PlayerPrefs.GetString("FireRange", "1")) / 2);
            if (gameplayMaestro.rateGateArrow < 10)
            {
                gameplayMaestro.rateGateArrow = pistols[gunLevel - 1].rateGateArrow;
            }

            if (gameplayMaestro.rangeGateArrow > 4)
            {
                gameplayMaestro.rangeGateArrow = 5;
            }

            if (gameplayMaestro.powerArrow < 5)
            {
                gameplayMaestro.powerArrow = pistols[gunLevel - 1].powerArrow;
            }

            if (gameplayMaestro.powerArrow < 0.5f)
            {
                gameplayMaestro.rangeGateArrow = pistols[gunLevel - 1].range;
            }

            animationSpeed = gameplayMaestro.rateGateArrow;
            //gameManager = pistols[gunLevel - 1].arrowCount + PlayerPrefs.GetInt("FireRange", 0);
            guns[gunLevel - 1].SetActive(true);
            animator = guns[gunLevel - 1].GetComponent<Animator>();
            particle = guns[gunLevel - 1].GetComponent<GunBehaviorData>().particleStm;
            //soundManager.gunAudio.clip = gunShotsAudios[gunLevel - 1];
            arrowStartPositions = guns[gunLevel - 1].GetComponent<GunBehaviorData>().defaultShootPosition;
            animator.SetFloat("Speed",
                Mathf.Abs((animationSpeed / 100)));
        }

        public void Rotate()
        {
            animator.SetTrigger("Rotate");
        }

        private void AttackChanger()
        {
            attack = !attack;
            animator.SetBool("Fire", attack);
            animator.SetBool("Stay", !attack);
        }


        public void ShootBullet()
        {
            if (AudioEngine.audioEngine.isHaptic)
            {
                MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
            }

            particle.Play();
            var arrows = arrowPooler.GetObject();
            var arrow = arrows.gameObject.GetComponent<Bullet.Bullet>();
            arrow.gunBehavior = this;
            arrows.gameObject.SetActive(false);
            arrows.transform.position = arrowStartPositions.position;
            arrows.transform.rotation = transform.localRotation;
            arrows.gameObject.SetActive(true);
            arrowsIndex++;
            if (arrowsIndex >= countArrows) arrowsIndex = 0;
        }

        public void AddPartGun(int index)
        {
            int gunLevel = PlayerPrefs.GetInt("GunLevel", 1);
            if (index > 0)
            {
                BoxCollider component = GunMovement.gunMovement.gameObject.GetComponent<BoxCollider>();
                component.size = currentGunBehaviorData.size;
                component.center = currentGunBehaviorData.center;
            }

            guns[gunLevel - 1].GetComponent<GunBehaviorData>().AddPartGun(index);
        }
    }

    [Serializable]
    class Pistol
    {
        public int powerArrow;
        public float rateGateArrow;
        public float range;
    }
}