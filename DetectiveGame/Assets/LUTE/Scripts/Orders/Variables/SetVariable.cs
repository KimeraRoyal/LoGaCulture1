using Unity.XR.CoreUtils.Datums;
using UnityEngine;
using static BooleanVariable;
using static FloatVariable;

[OrderInfo("Variable",
            "Set Variable",
            "Sets a Integer (and more) variable to a new value using a simple arithmetic operation. The value can be a constant or reference another variable of the same type.")]
public class SetVariable : Order, ISerializationCallbackReceiver
{
    [SerializeField] protected AnyVariableAndDataPair variable = new AnyVariableAndDataPair();

    [Tooltip("The type of operation to perform on the variable")]
    [SerializeField] protected SetOperator setOperator;

    protected virtual void SetOperation()
    {
        if (variable.variable == null)
        {
            Debug.LogError("No variable selected");
            return;
        }
        variable.SetOp(setOperator);
    }

    public virtual SetOperator _SetOperator
    {
        get
        {
            return setOperator;
        }
    }

    public override void OnEnter()
    {
        SetOperation();

        Continue();
    }

    public override string GetSummary()
    {
        if (variable.variable == null)
        {
            return "Error: No variable selected";
        }

        string desc = variable.variable.Key;
        desc += " " + VariableUtil.GetSetOperatorDescription(setOperator) + " ";
        desc += variable.GetDataDescription();

        return desc;
    }

    public override bool HasReference(Variable variable)
    {
        return false;
    }

    [Tooltip("Variable to use in expression.")]
    [VariableProperty(AllVariableTypes.VariableAny.Any)]
    [SerializeField] protected Variable var;

    [Tooltip("Integer value to compare against")]
    [SerializeField] protected IntegerData integerData;

    [Tooltip("Boolean value to compare against")]
    [SerializeField] protected BooleanData booleanData;

    [Tooltip("Float value to compare against")]
    [SerializeField] protected FloatData floatData;

    [Tooltip("String value to compare against")]
    [SerializeField] protected StringData stringData;

    public void OnBeforeSerialize()
    {
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (var == null)
            return;
        else
            variable.variable = var;

        if (variable.GetType() == typeof(IntegerVariable) && !integerData.Equals(new IntegerData()))
        {
            variable.data.integerData = integerData;
            integerData = new IntegerData();
        }
        else if (variable.GetType() == typeof(BooleanVariable) && !booleanData.Equals(new BooleanData()))
        {
            variable.data.booleanData = booleanData;
            booleanData = new BooleanData();
        }
        else if (variable.GetType() == typeof(FloatVariable) && !floatData.Equals(new FloatData()))
        {
            variable.data.floatData = floatData;
            floatData = new FloatData();
        }
        else if (variable.GetType() == typeof(StringVariable) && !stringData.Equals(new StringData()))
        {
            variable.data.stringData.stringRef = stringData.stringRef;
            variable.data.stringData.stringVal = stringData.stringVal;
            stringData = new StringData();
        }

        variable = null;
    }
}
