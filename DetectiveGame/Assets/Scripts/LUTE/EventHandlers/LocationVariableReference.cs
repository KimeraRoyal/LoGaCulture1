using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class LocationVariableReference
{
    [SerializeField] private LocationVariable m_variable;

    public LocationVariable Variable => m_variable;
}