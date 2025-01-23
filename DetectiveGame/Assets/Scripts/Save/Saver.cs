using UnityEngine;

namespace Save
{
    public abstract class Saver : MonoBehaviour    
    {
        public abstract void Save();
        public abstract void Load();
    }
}