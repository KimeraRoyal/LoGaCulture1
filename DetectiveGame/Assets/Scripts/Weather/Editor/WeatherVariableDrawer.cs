using UnityEditor;

namespace Weather.Editor
{
    [CustomPropertyDrawer(typeof(WeatherData))]
    public class WeatherDataDrawer : VariableDataDrawer<WeatherVariable>
    { }
}

