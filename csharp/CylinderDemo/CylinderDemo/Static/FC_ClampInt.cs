namespace CylinderDemo.Static;

/// <summary>
/// PLC analogy: FC_ClampInt (stateless helper).
///
/// Think of this as a "static utility method" shared by all objects.
/// No internal state -> safe to call from anywhere.
/// </summary>
public static class FC_ClampInt
{
    public static int ClampInt(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}
