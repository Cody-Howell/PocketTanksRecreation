using Logic;
using TUnit.Assertions.Enums;

namespace LogicTests;

public class MathHelperTests {
    [Test]
    [MethodDataSource(typeof(MathHelperTests), nameof(SpacedOutTestCases))]
    public async Task CanGetSpacedOutCounts(int count, int maxWidth, int[] results) {
        IEnumerable<int> result = MathHelpers.GetSpacedOutValues(count, maxWidth);
        await Assert.That(result).IsEquivalentTo(results, CollectionOrdering.Matching);
    }

    public static IEnumerable<Func<(int count, int maxWidth, int[] results)>> SpacedOutTestCases() {
        yield return () => (3, 10, [0, 5, 10]);
        yield return () => (4, 10, [0, 3, 6, 10]);
        yield return () => (5, 10, [0, 2, 5, 7, 10]);
    }
}
