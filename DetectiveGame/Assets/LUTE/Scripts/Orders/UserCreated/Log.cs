using UnityEngine;

[OrderInfo("Debug",
              "Log",
              "Logs a message to the debug output.")]
[AddComponentMenu("")]
public class Log : Order
{
    [SerializeField] protected string message;
    
    public override void OnEnter()
    {
        Debug.Log(message);
        Continue();
    }

    public override string GetSummary()
        => $"Log: {message}";
}