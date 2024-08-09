KeyboardShare kbh = new KeyboardShare();
kbh.ConfigHook();
kbh.KeyDown += async (s, k) =>
{

    switch (k.Key)
    {
        case Key.F1:
            {

                break;
            }
        case Key.F2:
            {
                break;
            }
        case Key.F3:
            {
                break;
            }
        case Key.F4:
            {
                break;
            }
        case Key.F8:
            {

                break;
            }
        case Key.F7:
            {
                break;
            }
        case Key.F10:
            {
                break;
            }
        case Key.F9:
            {
                break;
            }
    }


};
MSG message;
while (KeyboardShare.GetMessage(out message, IntPtr.Zero, 0, 0) != 0)
{
    KeyboardShare.TranslateMessage(ref message);
    KeyboardShare.DispatchMessage(ref message);
}