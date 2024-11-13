using System;
using DG.Tweening;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] protected float m_duration = 1.0f;
    [SerializeField] protected Vector3 m_strength = Vector3.one;
    [SerializeField] protected int m_vibrato = 10;
    [SerializeField] protected float m_randomness = 45.0f;

    private Vector3 m_startingPosition;

    private Tween m_shakeTween;

    public float Duration
    {
        get => m_duration;
        set => m_duration = value;
    }

    public Vector3 Strength
    {
        get => m_strength;
        set => m_strength = value;
    }

    public int Vibrato
    {
        get => m_vibrato;
        set => m_vibrato = value;
    }

    public float Randomness
    {
        get => m_randomness;
        set => m_randomness = value;
    }
    
    private void Start()
    {
        m_startingPosition = transform.localPosition;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) { Play(); }
    }

    public void Play()
        => Play(m_duration, m_strength, m_vibrato, m_randomness);

    public void Play(float _duration)
        => Play(_duration, m_strength, m_vibrato, m_randomness);

    public void Play(float _duration, Vector3 _strength)
        => Play(_duration, _strength, m_vibrato, m_randomness);

    public void Play(float _duration, Vector3 _strength, int _vibrato)
        => Play(_duration, _strength, _vibrato, m_randomness);

    public void Play(float _duration, Vector3 _strength, int _vibrato, float _randomness)
    {
        if (m_shakeTween is { active: true })
        {
            m_shakeTween.Kill();
            
            transform.localPosition = m_startingPosition;
        }
        m_shakeTween = transform.DOShakePosition(_duration, _strength, _vibrato, _randomness);
    }
}
