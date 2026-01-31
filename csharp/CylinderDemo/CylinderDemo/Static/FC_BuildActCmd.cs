using CylinderDemo.Models;

namespace CylinderDemo.Static;

/// <summary>
/// PLC analogy: FC_BuildActCmd (stateless helper).
///
/// In the TIA project, this FC "builds" the command UDT from simple inputs.
/// In C#, this is basically a small factory method.
/// </summary>
public static class FC_BuildActCmd
{
    public static ActCmd BuildActCmd(bool enable, bool extend, bool retract)
        => new()
        {
            Enable = enable,
            Extend = extend,
            Retract = retract
        };
}
