using System.Collections.Generic;
using KR.Map.Marker;
using UnityEngine;

namespace KR
{
    public class Markers : MonoBehaviour
    {
        private List<Marker> m_markerList;
        private Dictionary<string, Marker> m_markerDictionary;

        [SerializeField] private Marker m_markerPrefab;

        public IReadOnlyList<Marker> List => m_markerList;
        
        private void Awake()
        {
            m_markerList = new List<Marker>();
            m_markerDictionary = new Dictionary<string, Marker>();
        }

        public Marker GetMarker(string _name)
            => m_markerDictionary.GetValueOrDefault(_name);

        public Marker AddMarker(string _name)
        {
            if (m_markerDictionary.TryGetValue(_name, out var marker)) { return marker; }

            marker = Instantiate(m_markerPrefab, transform);
            
            m_markerList.Add(marker);
            m_markerDictionary.Add(_name, marker);
            
            return marker;
        }
    }
}
