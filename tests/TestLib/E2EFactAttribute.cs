namespace SaasusSdk.Tests.TestLib;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class E2EFactAttribute : FactAttribute
{
    public E2EFactAttribute()
    {
        EnvFileLoader.LoadAndApply();
        if (!bool.TryParse(Environment.GetEnvironmentVariable("SAASUS_E2E"), out var enabled) ||
            !enabled)
        {
            Skip = "Set SAASUS_E2E=true to run tests that change live API data.";
        }
    }
}
