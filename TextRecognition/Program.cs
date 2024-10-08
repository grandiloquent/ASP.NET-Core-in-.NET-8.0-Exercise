﻿using System.Drawing;
using static Screenshot;

POINT _point1 = new POINT();
POINT _point2 = new POINT();
KeyboardShare kbh = new KeyboardShare();
kbh.ConfigHook();
kbh.KeyDown += async (s, k) =>
{

    switch (k.Key)
    {
        case Key.F1:
            {
                Utils.ColorPicker();
                break;
            }
        case Key.F2:
            {

                break;
            }
        case Key.F3:
            {
                Utils.MakeFile();

                break;
            }
        case Key.F10:
            {
                if (_point1.X == 0)
                {
                    GetCursorPos(out _point1);
                }
                else
                {
                    GetCursorPos(out _point2);
                    var value = ImageUtils.ScreenShoot(new Point(_point1.X, _point1.Y), new Point(_point2.X, _point2.Y));
                    File.WriteAllBytes("".GetDesktopPath().GetUniqueFileName(".png"), value);
                    _point1 = new POINT();
                    _point2 = new POINT();
                }
                break;
            }
        case Key.F8:
            {
                ClipboardShare.SetText(Utils.Translate());
                break;
            }
        case Key.F7:
            {
                break;
            }
        case Key.F12:
            {
                if (_point1.X == 0)
                {
                    GetCursorPos(out _point1);
                }
                else
                {
                    GetCursorPos(out _point2);
                    var value = ImageUtils.Ocr(new Point(_point1.X, _point1.Y), new Point(_point2.X, _point2.Y));
                    ClipboardShare.SetText(value);
                    _point1 = new POINT();
                    _point2 = new POINT();
                }
                break;
            }
        case Key.F9:
            {
                if (_point1.X == 0)
                {
                    GetCursorPos(out _point1);
                }
                else
                {
                    GetCursorPos(out _point2);
                    var value = Utils.Ocr(new Point(_point1.X, _point1.Y), new Point(_point2.X, _point2.Y));
                    ClipboardShare.SetText(value);
                    _point1 = new POINT();
                    _point2 = new POINT();
                }
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
/*
 dotnet publish -r win-x64 --self-contained false -o C:\Users\Administrator\Desktop\.Folder\005 -p:PublishSingleFile=true,AssemblyName=TextRecognition
 */
