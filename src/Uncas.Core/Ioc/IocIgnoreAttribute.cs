namespace Uncas.Core.Ioc
{
    using System;

    /// <summary>
    /// Instructs the automagic registering of IOC containers to ignore a given class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class IocIgnoreAttribute : Attribute
    {
    }
}