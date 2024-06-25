using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FpsCounter : MonoBehaviour
    {
        private TextMeshProUGUI fpsText;
        private float[] frameDeltaTimeArray;
        private int lastFrameIndex;

        private void Awake()
        {
            frameDeltaTimeArray = new float[50];
            fpsText = GetComponent<TextMeshProUGUI>();
        }

        private float CalculateFPS()
        {
            float total = 0f;
            for (var i = 0; i < frameDeltaTimeArray.Length; i++)
            {
                total += frameDeltaTimeArray[i];
            }

            return frameDeltaTimeArray.Length / total;
        }

        void Update()
        {
            frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
            lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
            fpsText.text = $"FPS: {Mathf.RoundToInt(CalculateFPS())}";
        }
    }
}