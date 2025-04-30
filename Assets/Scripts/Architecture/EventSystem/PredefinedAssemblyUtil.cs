using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Finds all non-abstract, non-interface types in any loaded assembly
/// that implement the given interface (e.g. IEvent).
/// </summary>
public static class PredefinedAssemblyUtil
{
    public static List<Type> GetTypes(Type interfaceType)
    {
        var results = new List<Type>();

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types;
            try
            {
                types = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }

            if (types == null)
                continue;

            foreach (var t in types)
            {
                if (t == null)
                    continue;

                if (interfaceType.IsAssignableFrom(t)
                    && !t.IsInterface
                    && !t.IsAbstract)
                {
                    results.Add(t);
                }
            }
        }

        return results;
    }
}
