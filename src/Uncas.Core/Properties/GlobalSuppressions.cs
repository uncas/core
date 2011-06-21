using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage(
    "Microsoft.Design",
    "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace",
    Target = "Uncas.Core",
    Justification = "The Uncas.Core namespace holds infrastructure code that is not application specific; with time it is expected to contain a range of types.")]