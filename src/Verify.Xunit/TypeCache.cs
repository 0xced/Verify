﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

static class TypeCache
{
    static Assembly assembly = null!;
    static List<Type> types = null!;

    public static void SetTestAssembly(Assembly assembly)
    {
        TypeCache.assembly = assembly;
        types = assembly.GetExportedTypes()
            .Where(x => !x.IsAbstract)
            .ToList();
    }

    const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

    public static (Type, MethodInfo) GetInfo(string file, string method)
    {
        if (assembly == null)
        {
            throw new Exception("Call `Verifier.SetTestAssembly(Assembly.GetExecutingAssembly());` at assembly startup.");
        }

        var type = FindType(file);

        var methodInfo = type.GetMethod(method, flags);
        if (methodInfo == null)
        {
            throw new Exception($"Method `{methodInfo}` not found on type `{type.Name}`.");
        }

        return (type, methodInfo);
    }

    static Type FindType(string file)
    {
        var withoutExtension = file.Substring(0, file.LastIndexOf('.'));
        var withDots = withoutExtension
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');
        foreach (var type in types)
        {
            if (withDots.EndsWith(type.FullName))
            {
                return type;
            }
        }

        throw new Exception($"Unable to find type for file `{file}`.");
    }
}