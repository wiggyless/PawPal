using System;
using System.Linq;
using System.Reflection;

public static class ObjectValidationExtensions
{
    /// <summary>
    /// Returns false if any string property on the object is null, empty,
    /// or whitespace-only. Properties named in propertiesToSkip are ignored.
    /// </summary>
    public static bool AreStringPropertiesValid(this object obj, params string[] propertiesToSkip)
    {
        if (obj == null)
            return false;

        var skipSet = new HashSet<string>(propertiesToSkip ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

        var stringProperties = obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string)
                        && p.CanRead
                        && !skipSet.Contains(p.Name));

        foreach (var prop in stringProperties)
        {
            var value = (string)prop.GetValue(obj);
            if (string.IsNullOrWhiteSpace(value))
                return false;
        }

        return true;
    }
}