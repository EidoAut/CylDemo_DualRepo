using CylinderDemo.Systems;

namespace CylinderDemo;

internal static class Program {
    private static void Main() {
        var fb = new FB_Cylinder();

        bool selectA; // we will force the user to pick this at startup

        // -------------------------------
        // Startup menu (like OB100/startup phase)
        // -------------------------------
        Console.WriteLine("CylinderDemo (PLC/TIA OOP analogy in C#)");
        Console.WriteLine();
        Console.WriteLine("Select which cylinder to control:");
        Console.WriteLine("  1 -> CylA (pneumatic)");
        Console.WriteLine("  2 -> CylB (electric)");
        Console.WriteLine();
        Console.WriteLine("Other controls during runtime:");
        Console.WriteLine("  E -> Extend selected cylinder");
        Console.WriteLine("  R -> Retract selected cylinder");
        Console.WriteLine("  P -> Toggle PressureOk (affects CylA)");
        Console.WriteLine("  Q -> Quit");
        Console.WriteLine();

        while (true) {
            var key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.Q) return;

            if (key == ConsoleKey.D1) {
                selectA = true;
                break;
            }

            if (key == ConsoleKey.D2) {
                selectA = false;
                break;
            }
        }

        bool dirty = true; // force first render

        // -------------------------------
        // Runtime scan loop (like OB1 cyclic)
        // -------------------------------
        while (true) {
            bool hadKey = false;

            // -------------------------------
            // Read "HMI" inputs (keyboard)
            // -------------------------------
            while (Console.KeyAvailable) {
                hadKey = true;
                dirty = true;

                var key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.Q) return;

                if (key == ConsoleKey.D1) selectA = true;
                if (key == ConsoleKey.D2) selectA = false;

                if (key == ConsoleKey.P) fb.PressureOk = !fb.PressureOk;

                if (key == ConsoleKey.E) {
                    if (selectA) fb.ExtendCylA = true;
                    else fb.ExtendCylB = true;
                }

                if (key == ConsoleKey.R) {
                    if (selectA) fb.RetractCylA = true;
                    else fb.RetractCylB = true;
                }
            }

            // -------------------------------
            // One "scan"
            // -------------------------------
            fb.Update();

            // -------------------------------
            // Console "watch table" (refresh only on input or first time)
            // -------------------------------
            if (dirty) {
                dirty = false;

                Console.Clear();
                Console.WriteLine("=== WATCH TABLE (C# mirroring the TIA DB view) ===");
                Console.WriteLine($"Selected: {(selectA ? "CylA (pneumatic)" : "CylB (electric)")}");
                Console.WriteLine();

                Console.WriteLine($"PressureOk (CylA permissive): {fb.PressureOk}");
                Console.WriteLine();

                Console.WriteLine(
                    $"CylA Status: Extended={fb.CylA_Status.Extended}, Retracted={fb.CylA_Status.Retracted} | " +
                    $"Valves: EXT={fb.CylA_DebugValves.ValveExtend}, RET={fb.CylA_DebugValves.ValveRetract}"
                );

                Console.WriteLine(
                    $"CylB Status: Extended={fb.CylB_Status.Extended}, Retracted={fb.CylB_Status.Retracted} | " +
                    $"Pos={fb.CylB_Debug.Position}, Target={fb.CylB_Debug.Target}, Step={fb.CylB_Debug.Step}"
                );

                Console.WriteLine();
                Console.WriteLine("Controls: 1/2 select, E extend, R retract, P toggle pressure, Q quit.");
            }

            // -------------------------------
            // Reset command bits (momentary pushbuttons)
            // -------------------------------
            fb.ExtendCylA = false;
            fb.RetractCylA = false;
            fb.ExtendCylB = false;
            fb.RetractCylB = false;

            Thread.Sleep(100);
        }
    }
}
