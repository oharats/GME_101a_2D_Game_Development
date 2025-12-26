using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float _duration = 0.35f;
    public AnimationCurve _curve;


    public IEnumerator Shake()
    {
        Vector3 _startPosition = transform.position;
        float _elapsedTime = 0f;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float _strength = _curve.Evaluate(_elapsedTime / _duration);
            transform.position = _startPosition + Random.insideUnitSphere * _strength;
            yield return null;
        }

        transform.position = _startPosition;
    }
}
