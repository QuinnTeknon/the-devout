namespace Devout.DevoutCode.Extensions;

public static class ListExtensions {
    public static void Shuffle<T>(this IList<T> list) {
        Random RNG = new();
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = RNG.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}