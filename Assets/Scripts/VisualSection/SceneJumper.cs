using System;
using System.Collections;
using DG.Tweening;
using Engine.GameSections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UISection
{
    public class SceneJumper : MonoBehaviour
    {
        [SerializeField] private Image loadingProgress;

        private AsyncOperation opeartion;
        private float progress;
        private float timer;
        public bool startTimer;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SpecialFunc()
        {
        }

        private void Start()
        {
            if (!AdManager.adManager.isInitialized)
            {
                AdManager.adManager.isInitialized = true;
                AdManager.adManager.Init();
                timer = 2;
                if (PlayerPrefs.GetInt("battleTutorial", 1) == 0)
                {
                    StartLoading(4);
                }
                else
                {
                    StartLoading(2);
                }
            }
            else
            {
                timer = 2;
                if (PlayerPrefs.GetInt("battleTutorial", 1) == 0)
                {
                    StartLoading(4);
                }
                else
                {
                    StartLoading(2);
                }
            }
        }

        private void Update()
        {
            if (startTimer)
            {
                loadingProgress.fillAmount =
                    Mathf.MoveTowards(loadingProgress.fillAmount, opeartion.progress, Time.deltaTime * 2);
                if (timer <= 0.1f)
                {
                    GameplayMaestro.Instance.isLoaded = true;
                    Destroy(gameObject, 1);
                    gameObject.SetActive(false);
                }

                timer -= Time.deltaTime;
            }
        }

        private void StartLoading(int sceneIndex)
        {
            GC.Collect();
            opeartion = SceneManager.LoadSceneAsync(sceneIndex);
            opeartion.allowSceneActivation = false;

            StartCoroutine(LoadingRoutine());
        }

        private IEnumerator LoadingRoutine()
        {
            progress = 0;
            while (progress < 89)
            {
                progress = (int)(opeartion.progress * 100);
                loadingProgress.fillAmount = opeartion.progress / 2;
                yield return null;
            }

            opeartion.allowSceneActivation = true;
            if (opeartion.progress >= 0.88f)
            {
                startTimer = true;
            }
        }
    }
}