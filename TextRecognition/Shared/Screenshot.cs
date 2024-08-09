using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

public static class Screenshot
{
    public struct POINT
    {
        public int X;
        public int Y;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public int X
        {
            get
            {
                return Left;
            }
            set
            {
                Right -= Left - value;
                Left = value;
            }
        }
        public int Y
        {
            get
            {
                return Top;
            }
            set
            {
                Bottom -= Top - value;
                Top = value;
            }
        }
        public int Width
        {
            get
            {
                return Right - Left;
            }
            set
            {
                Right = value + Left;
            }
        }
        public int Height
        {
            get
            {
                return Bottom - Top;
            }
            set
            {
                Bottom = value + Top;
            }
        }
        public Point Location
        {
            get
            {
                return new Point(Left, Top);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public RECT(Rectangle r)
            : this(r.Left, r.Top, r.Right, r.Bottom)
        {
        }
        public static implicit operator Rectangle(RECT r)
        {
            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }
        public static implicit operator RECT(Rectangle r)
        {
            return new RECT(r);
        }
        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }
        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }
        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }
        public override bool Equals(object obj)
        {
            //            if (obj is RECT rect)
            //            {
            //                return Equals(rect);
            //            }
            //
            //            if (obj is Rectangle rectangle)
            //            {
            //                return Equals(new RECT(rectangle));
            //            }
            return false;
        }
        public override int GetHashCode()
        {
            return ((Rectangle)this).GetHashCode();
        }
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }
    [DllImport("gdi32.dll")]
    public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);
    [DllImport("user32.dll")]
    public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nReghtRect, int nBottomRect);
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nReghtRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hDC);
    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);
    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
    [DllImport("user32.dll")]
    public static extern IntPtr GetDesktopWindow();
    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowDC(IntPtr hWnd);
    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out POINT lpPoint);
    [DllImport("user32.dll")]
    static extern IntPtr WindowFromPoint(POINT p);
    private static Bitmap CaptureRectangleNative(Rectangle rect, bool captureCursor = false)
    {
        IntPtr handle = GetDesktopWindow();
        return CaptureRectangleNative(handle, rect, captureCursor);
    }
    private static Bitmap CaptureRectangleNative(IntPtr handle, Rectangle rect, bool captureCursor = false)
    {
        if (rect.Width == 0 || rect.Height == 0)
        {
            return null;
        }
        IntPtr hdcSrc = GetWindowDC(handle);
        IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
        IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, rect.Width, rect.Height);
        IntPtr hOld = SelectObject(hdcDest, hBitmap);
        BitBlt(hdcDest, 0, 0, rect.Width, rect.Height, hdcSrc, rect.X, rect.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
        if (captureCursor)
        {
            //				try {
            //					CursorData cursorData = new CursorData();
            //					cursorData.DrawCursor(hdcDest, rect.Location);
            //				} catch (Exception e) {
            //					DebugHelper.WriteException(e, "Cursor capture failed.");
            //				}
        }
        SelectObject(hdcDest, hOld);
        DeleteDC(hdcDest);
        ReleaseDC(handle, hdcSrc);
        Bitmap bmp = Image.FromHbitmap(hBitmap);
        DeleteObject(hBitmap);
        return bmp;
    }
    public static Bitmap CaptureRectangle(Rectangle rect)
    {
        //            if (RemoveOutsideScreenArea)
        //            {
        //                Rectangle bounds = CaptureHelpers.GetScreenBounds();
        //                rect = Rectangle.Intersect(bounds, rect);
        //            }
        return CaptureRectangleNative(rect);
    }
    public static Bitmap CaptureWindow(IntPtr handle)
    {
        if (handle.ToInt32() > 0)
        {
            Rectangle rect;
            //if (CaptureClientArea) {
            rect = GetClientRect(handle);
            //				} else {
            //					rect = CaptureHelpers.GetWindowRectangle(handle);
            //				}
            //bool isTaskbarHide = false;
            //                try
            //                {
            //                    if (AutoHideTaskbar)
            //                    {
            //                        isTaskbarHide = NativeMethods.SetTaskbarVisibilityIfIntersect(false, rect);
            //                    }
            //
            return CaptureRectangle(rect);
            //                }
            //                finally
            //                {
            //                    if (isTaskbarHide)
            //                    {
            //                        NativeMethods.SetTaskbarVisibility(true);
            //                    }
            //                }
        }
        return null;
    }
    public static string GenerateRandomString(int length)
    {
        var coupon = new StringBuilder();
        var rng = RandomNumberGenerator.Create() ;
        var rnd = new byte[1];
        var n = 0;
        while (n < length)
        {
            rng.GetBytes(rnd);
            var c = (char)rnd[0];
            if (c <= 122 && c >= 97)
            {
                ++n;
                coupon.Append(c);
            }
        }
        return coupon.ToString();
    }
    public static Rectangle GetClientRect(IntPtr handle)
    {
        RECT rect;
        GetClientRect(handle, out rect);
        Point position = rect.Location;
        ClientToScreen(handle, ref position);
        return new Rectangle(position, rect.Size);
    }
    public static string GetDesktopPath(string f)
    {
        var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dir = Path.Combine(dir, "图片");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return Path.Combine(dir, f);
    }
    public static void SaveScreenshot()
    {
        POINT p;
        GetCursorPos(out p);
        IntPtr hwnd = WindowFromPoint(p);//handle here
        var bitmap = CaptureWindow(hwnd);
        //string.Format("{0}-{1}.jpg", DateTime.Now.ToString("yyyy-MM-dd"), GenerateRandomString(6))
        var src = GetDesktopPath(string.Format("{0}.jpg", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

        WriteBitmapToFile(src, bitmap);
    }
    public static Bitmap GetScreenshot()
    {
        POINT p;
        GetCursorPos(out p);
        IntPtr hwnd = WindowFromPoint(p);//handle here
        return CaptureWindow(hwnd);

    }
    public static void WriteBitmapToFile(string filename, Bitmap bitmap)
    {
        bitmap.Save(filename, ImageFormat.Jpeg);
    }
}

