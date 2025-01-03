/// A simple struct wrapping a reference to a Variable. Allows for VariableReferenceDrawer. 
/// This is the a way to directly reference a variable in external c# scripts, it will 
/// give you an inspector field that gives a drop down of all the variables on the targeted
/// engine, in a similar way to what you would expect from selecting a variable on an order.
/// 
/// Also recommend implementing IVariableReference on any custom classes that use this so your
/// references can show up in searches for usage.

public struct VariableReference
{
    public Variable variable;

    public T Get<T>()
    {
        T retval = default(T);

        var asType = variable as BaseVariable<T>;

        if (asType != null)
            return asType.Value;

        return retval;
    }

    public void Set<T>(T val)
    {
        var asType = variable as BaseVariable<T>;

        if (asType != null)
            asType.Value = val;
    }
}