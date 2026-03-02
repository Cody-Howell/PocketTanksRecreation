namespace Logic; 

public static class MathHelpers {
    public static IEnumerable<int> GetSpacedOutValues(int count, int max) {
        yield return 0;

        float spacing = (float)max / (count - 1);
        float localSum = spacing;

        for (int i = 1; i < count; i++) {
            yield return (int)Math.Floor(localSum);
            localSum += spacing;
        }
    }
}
