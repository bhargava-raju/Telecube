namespace Telecube.ArchitectureTests;

internal static class TestResultExtensions
{
    // Fully qualified: xUnit v3 also has a TestResult type in the global Xunit namespace.
    public static string FailingTypeNames(this NetArchTest.Rules.TestResult result) =>
        string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? []);
}
