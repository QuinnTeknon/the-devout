using System.Drawing;
using Godot;

namespace Devout.DevoutCode.Extensions;

public static class IntegerExtensions {
    public static bool Factorises(this int number, int[] factors) {
        foreach (int factor in factors) {
            if (number % factor != 0) {
                return false;
            }
        }
        return true;
    }
    
    public static bool In<T>(this T item, params T[] items) {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        return items.Contains(item);
    }
}