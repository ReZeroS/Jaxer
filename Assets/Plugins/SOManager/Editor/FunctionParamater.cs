using System;

public class FunctionParameter
{
    public string Name { get; }
    public Type Type { get; }

    public FunctionParameter(string name, Type type)
    {
        Name = name;
        Type = type;
    }
}