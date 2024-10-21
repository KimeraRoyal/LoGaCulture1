using TMPro;

namespace Mapbox.Examples
{
	using Mapbox.Unity.MeshGeneration.Interfaces;
	using System.Collections.Generic;
	using UnityEngine;

	public class LabelTextSetter : MonoBehaviour, IFeaturePropertySettable
	{
		[SerializeField] TMP_Text m_label; 

		public void Set(Dictionary<string, object> props)
		{
			var text = "";

			if (props.TryGetValue("name", out var value))
			{
				text = value.ToString();
			}
			else if (props.TryGetValue("house_num", out value))
			{
				text = value.ToString();
			}
			else if (props.TryGetValue("type", out value))
			{
				text = value.ToString();
			}

			m_label.text = text;
		}
	}
}