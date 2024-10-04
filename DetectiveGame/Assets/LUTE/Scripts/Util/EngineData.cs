using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class IntVar
{
    [SerializeField] protected string key;
    [SerializeField] protected int value;

    public string Key { get { return key; } set { key = value; } }
    public int Value { get { return value; } set { this.value = value; } }
}

[System.Serializable]
public class BoolVar
{
    [SerializeField] protected string key;
    [SerializeField] protected bool value;

    public string Key { get { return key; } set { key = value; } }
    public bool Value { get { return value; } set { this.value = value; } }
}

[System.Serializable]
public class FloatVar
{
    [SerializeField] protected string key;
    [SerializeField] protected float value;

    public string Key { get { return key; } set { key = value; } }
    public float Value { get { return value; } set { this.value = value; } }
}

[System.Serializable]
public class StringVar
{
    [SerializeField] protected string key;
    [SerializeField] protected string value;

    public string Key { get { return key; } set { key = value; } }
    public string Value { get { return value; } set { this.value = value; } }
}

[System.Serializable]
public class PostcardVar
{
    [SerializeField] protected Postcard postcard;
    [SerializeField] protected string name;
    [SerializeField] protected string desc;
    [SerializeField] protected string creator;
    [SerializeField] protected int total;
    [SerializeField] protected List<Sticker> stickers = new List<Sticker>();
    [SerializeField] protected List<StickerVar> stickerVars = new List<StickerVar>();


    public Postcard Postcard { get { return postcard; } set { postcard = value; } }
    public string Name { get { return name; } set {  name = value; } }
    public string Desc { get { return desc; } set { desc = value; } }
    public string Creator { get { return creator; } set { creator = value; } }
    public int Total { get { return total; } set { total = value; } }
    public List<Sticker> Stickers { get { return stickers; } set { stickers = value; } }
    public List<StickerVar> StickerVars { get { return stickerVars; } set { stickerVars = value; } }

    [System.Serializable]
    public class StickerVar
    {
        [SerializeField] protected string name;
        [SerializeField] protected string desc;
        [SerializeField] protected StickerManager.StickerType type;
        [SerializeField] protected Sprite image;
        [SerializeField] protected Vector3 position;
        [SerializeField] protected Vector3 scale;
        [SerializeField] protected Quaternion rot;

        public string Name { get { return name; } set { name = value; } }
        public string Desc { get { return desc; } set { desc = value; } }
        public StickerManager.StickerType Type { get { return type; } set { type = value; } }
        public Sprite Image { get { return image; } set { image = value; } }
        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 StickerScale { get { return scale; } set { scale = value; } }
        public Quaternion StickerRot { get { return rot; } set { rot = value; } }
    }

}

[System.Serializable]
public class NodeVar
{
    [SerializeField] protected ExecutionState state;
    [SerializeField] protected bool completed;
    [SerializeField] protected string name;

    public ExecutionState State { get { return state; } set { state = value; } }
    public bool Completed { get { return completed; } set { completed = value; } }
    public string Name { get { return name; } set { name = value; } }
}

/// Serializable container for encoding the state of variables.
[System.Serializable]
public class EngineData 
{
    [SerializeField] protected string engineName;
    [SerializeField] protected List<IntVar> intVars = new List<IntVar>();
    [SerializeField] protected List<BoolVar> boolVars = new List<BoolVar>();
    [SerializeField] protected List<FloatVar> floatVars = new List<FloatVar>();
    [SerializeField] protected List<StringVar> stringVars = new List<StringVar>();
    [SerializeField] protected List<PostcardVar> postcardVars = new List<PostcardVar>();
    [SerializeField] protected List<NodeVar> nodeVars = new List<NodeVar>();

    public string EngineName { get { return engineName; } set { engineName = value; } }

    public List<IntVar> IntVars { get { return intVars; } set { intVars = value; } }
    public List<BoolVar> BoolVars { get { return boolVars; } set { boolVars = value; } }
    public List<FloatVar> FloatVars { get { return floatVars; } set { floatVars = value; } }
    public List<StringVar> StringVars { get { return stringVars; } set { stringVars = value; } }
    public List<PostcardVar> PostcardVars { get { return postcardVars; } set {  postcardVars = value; } }
    public List<NodeVar> NodeVars { get { return nodeVars; } set { nodeVars = value; } }

    public static EngineData Encode(BasicFlowEngine engine)
    {
        var engineData = new EngineData();
        engineData.EngineName = engine.name;

        // save all the completition states on the nodes
        var nodes = engine.GetComponents<Node>();
        foreach (Node node in nodes)
        {
            if (node != null && node.Saveable)
            {
                var d = new NodeVar();
                d.State = node.CheckExecutionState();
                d.Completed = node.NodeComplete;
                d.Name = node._NodeName;
                engineData.NodeVars.Add(d);
            }
        }

        for (int i = 0; i < engine.Variables.Count; i++)
        {
            var v = engine.Variables[i];

            var intVariable = v as IntegerVariable;
            if (intVariable != null)
            {
                var d = new IntVar();
                d.Key = intVariable.Key;
                d.Value = intVariable.Value;
                engineData.IntVars.Add(d);
            }

            var boolVariable = v as BooleanVariable;
            if (boolVariable != null)
            {
                var d = new BoolVar();
                d.Key = boolVariable.Key;
                d.Value = boolVariable.Value;
                engineData.BoolVars.Add(d);
            }

            var floatVariable = v as FloatVariable;
            if (floatVariable != null)
            {
                var d = new FloatVar();
                d.Key = floatVariable.Key;
                d.Value = floatVariable.Value;
                engineData.FloatVars.Add(d);
            }

            var stringVariable = v as StringVariable;
            if (stringVariable != null)
            {
                var d = new StringVar();
                d.Key = stringVariable.Key;
                d.Value = stringVariable.Value;
                engineData.StringVars.Add(d);
            }
        }

        var postcards = engine.GetComponents<Postcard>();
        foreach (Postcard postcard in postcards)
        {
            if (postcard != null)
            {
                var d = new PostcardVar();
                d.Postcard = postcard;
                d.Name = postcard.PostcardName;
                d.Desc = postcard.PostcardDesc;
                d.Creator = postcard.PostcardCreator;
                d.Total = postcard.TotalStickers;
                d.StickerVars = new List<PostcardVar.StickerVar>(postcard.StickerVars);

                var originalStickers = postcard.stickers;

                foreach (var original in originalStickers)
                {
                    if (original != null)
                    {
                        var newStickerVar = new PostcardVar.StickerVar();
                        newStickerVar.Name = original.StickerName;
                        newStickerVar.Desc = original.StickerDescription;
                        newStickerVar.Type = original.StickerType;
                        newStickerVar.Image = original.StickerImage;
                        newStickerVar.Position = original.StickerPosition;
                        newStickerVar.StickerScale = original.StickerScale;
                        newStickerVar.StickerRot = original.StickerRotation;

                        d.StickerVars.Add(newStickerVar);
                    }
                }

                engineData.PostcardVars.Add(d);
            }
        }

        return engineData;
    }

    public static void Decode(EngineData engineData)
    {
        var go = GameObject.Find(engineData.EngineName);
        if (go == null)
        {
            Debug.Log("Failed to find engine with name: " + engineData.EngineName);
            return;
        }

        var engine = go.GetComponent<BasicFlowEngine>();
        if (engine == null)
        {
            Debug.Log("Failed to find engine component in game object: " + engineData.EngineName);
            return;
        }
        for (int i = 0; i < engineData.IntVars.Count; i++)
        {
            var intVar = engineData.IntVars[i];
            engine.SetIntegerVariable(intVar.Key, intVar.Value);
        }
        for (int i = 0; i < engineData.BoolVars.Count; i++)
        {
            var boolVar = engineData.BoolVars[i];
            engine.SetBooleanVariable(boolVar.Key, boolVar.Value);
        }
        for (int i = 0; i < engineData.FloatVars.Count; i++)
        {
            var floatVar = engineData.FloatVars[i];
            engine.SetFloatVariable(floatVar.Key, floatVar.Value);
        }
        for (int i = 0; i < engineData.StringVars.Count; i++)
        {
            var stringVar = engineData.StringVars[i];
            engine.SetStringVariable(stringVar.Key, stringVar.Value);
        }
        for(int i = 0; i < engineData.PostcardVars.Count; i++)
        {
            var postcardVar = engineData.PostcardVars[i];
            engine.SetPostcard(postcardVar);
        }
        Postcard.ActivePostcard = null;
        Postcard.activePostcards.Clear();
        
        for(int i = 0; i < engineData.NodeVars.Count; i++)
        {
            var nodeVar = engineData.NodeVars[i];
            engine.SetNodeState(nodeVar.Name, nodeVar.State, nodeVar.Completed);
        }
    }
}