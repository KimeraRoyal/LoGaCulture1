using System;
using UnityEngine;

namespace Weather
{
    [VariableInfo("Location", "Weather")]
    [AddComponentMenu("")]
    [Serializable]
    public class WeatherVariable : BaseVariable<Weather>
    {
        public override bool SupportsArithmetic(SetOperator setOperator)
            => false;

        public override bool SupportsComparison()
            => true;
    }
    
    [Serializable]
    public struct WeatherData
    {
        [VariableProperty("<Value>", typeof(WeatherVariable))]
        [SerializeField] public WeatherVariable weatherRef;
        
        [SerializeField] public Weather weatherVal;

        public WeatherData(Weather v)
        {
            weatherVal = v;
            weatherRef = null;
        }

        public static implicit operator Weather(WeatherData weatherData)
        {
            return weatherData.Value;
        }

        public Weather Value
        {
            get { return (weatherRef == null) ? weatherVal : weatherRef.Value; }
            set { if (weatherRef == null) { weatherVal = value; } else { weatherRef.Value = value; } }
        }

        public string GetDescription()
        {
            if (weatherRef == null)
            {
                return weatherVal.ToString();
            }
            else
            {
                return weatherRef.Key;
            }
        }
    }
}
