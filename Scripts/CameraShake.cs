using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _camera;

    float maxShakeTime = 0;
    float currentShakeTime = 0;
    float startIntensity = 0;

    CinemachineBasicMultiChannelPerlin channel;
    void Start()
    {
        channel = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentShakeTime > 0)
        {
            currentShakeTime -= Time.deltaTime;
            if(currentShakeTime <= 0f)
            {
                channel.m_AmplitudeGain = 0;//Mathf.Lerp(startIntensity, 0f, 1 - (currentShakeTime / maxShakeTime));
            }
        }
    }

    public void Shake(float intensity, float time)
    {
        startIntensity = intensity;
        channel.m_AmplitudeGain = intensity;
        maxShakeTime = time;
        currentShakeTime = time;
    }
}
