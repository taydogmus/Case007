using System;
using Cinemachine;
using UnityEngine;

namespace Tuna
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        public static CameraShake Instance { get; private set; }
        private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
        private float shakeTimer;
        private float shakeTimerTotal;
        private float startingIntensity;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void ShakeCamera(float intensity, float time)
        {

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

            startingIntensity = time;
            shakeTimerTotal = time;
            shakeTimer = time;
        }

        private void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0)
                {
                    //Timer over!
                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
                }
            }
        }
    }
}