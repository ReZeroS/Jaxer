using System;
using System.Collections.Generic;

public class FunctionWrapper
{
    public string Name { get; }
    public Action<object[]> Execute { get; }
    public List<FunctionParameter> Parameters { get; }

    public FunctionWrapper(string name, Action<object[]> execute, List<FunctionParameter> parameters)
    {
        Name = name;
        Execute = execute;
        Parameters = parameters;
    }

    public void ExecuteFunction(object[] parameters)
    {
        Execute(parameters);
    }
}