using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Location", menuName = "LUTE/Location")]
public class Location : ScriptableObject
{
    [SerializeField] protected string m_label;
    [SerializeField] protected bool m_showName = true;

    [SerializeField] protected Sprite m_defaultIcon;
    [SerializeField] protected Color m_color = Color.white;

    [SerializeField] protected float m_radius = LogaConstants.DefaultRadius;

    public string Label => m_label;
    public bool ShowName => m_showName;

    public Sprite DefaultIcon => m_defaultIcon;
    public Color Color => m_color;

    public float Radius => m_radius;

    private void OnValidate()
    {
        m_label ??= name;
    }

#if UNITY_EDITOR
    public static Location CreateLocation(string _label, bool _showName, Sprite _icon, Color _color, float _radius)
    {
        var location = CreateInstance<Location>();

        location.m_label = _label;
        location.m_showName = _showName;
        
        location.m_defaultIcon = _icon;
        location.m_color = _color;
        
        location.m_radius = _radius;
        
        return location;
    }
#endif
}

[Serializable]
public class LocationReference
{
    [SerializeField] private Location m_location;

    public Location Location
    {
        get => m_location;
        set => m_location = value;
    }
}