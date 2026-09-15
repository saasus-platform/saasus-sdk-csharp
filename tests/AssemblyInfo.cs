// The live suites mutate shared SaaSus state: the Auth stories register the Billing Stripe
// integration when the environment has none, and the Billing stories delete it again. Separate
// xUnit collections would be allowed to run in parallel, so one suite could remove the
// registration another one is still using. Running the collections one after another keeps the
// shared configuration owned by a single suite at a time.
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
