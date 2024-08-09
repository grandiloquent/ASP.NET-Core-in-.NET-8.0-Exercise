using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 20)]
public struct KeyboardHookStruct
{
    [FieldOffset(16)] public IntPtr dwExtraInfo;
    [FieldOffset(8)] public int Flags;
    [FieldOffset(0)] public Key Key;
    [FieldOffset(4)] public int ScanCode;
    [FieldOffset(12)] public int Time;
}