#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <Windows.h>
#include <stdbool.h>

void Drag(int x, int y) {
    int maxX = GetSystemMetrics(SM_CXSCREEN), maxY = GetSystemMetrics(SM_CYSCREEN);
    //int x = 1191, y =202;
    double factorX = 65536.0 / maxX, factorY = 65536.0 / maxY;

    INPUT ip;

    ZeroMemory(&ip, sizeof(ip));

    ip.type = INPUT_MOUSE;

    ip.mi.mouseData = 0;
    ip.mi.dx = x * factorX;
    ip.mi.dy = y * factorY;


    ip.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN;

    SendInput(1, &ip, sizeof(ip));


    ip.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;

    SendInput(1, &ip, sizeof(ip));


    ip.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP;

    SendInput(1, &ip, sizeof(ip));
}

bool SetText(const char *str) {
    //GetForegroundWindow()
    if (!OpenClipboard(0)) {
        printf("ERROR");
        return FALSE;
    }
    EmptyClipboard();
    const size_t len = strlen(str) + 1;
    HGLOBAL hglb = GlobalAlloc(GMEM_MOVEABLE, len);
    memcpy(GlobalLock(hglb), str, len);
    GlobalUnlock(hglb);
    //EmptyClipboard();
    HANDLE res = SetClipboardData(CF_TEXT, hglb);
    bool isSet = res ? TRUE : FALSE;
    if (!isSet) {
        GlobalFree(hglb);
    }
    CloseClipboard();
    return isSet;
}

POINT p;
typedef struct Thread {
    HANDLE handle;
    DWORD dwThreadId;
    BOOL status;
} thread;

#define RegisterHotKeyWithoutModifier(X, Y)                                                \
    if (RegisterHotKey(NULL, X, 0, X)) {                                \
        printf("成功快捷键：%c = %d = 0x%0X, %s\n", MapVirtualKeyA(X, 2), X,X,Y); \
    }
#define HandleHotKeyWithThread(X, Y, Z)                                         \
    if (msg.wParam == X) {                                              \
        GetCursorPos(&p);                                                 \
        if (!hWnd) {                                                      \
            hWnd = WindowFromPoint(p);                                      \
        }                                                                 \
        if (!threads[Y].handle) {                                         \
            threads[Y].handle =                                             \
            CreateThread(NULL, 0, Z, &hWnd, 0, &threads[Y].dwThreadId); \
            threads[Y].status = TRUE;                                       \
            printf("创建线程 %d = %d\n", Y, threads[Y].dwThreadId);         \
        } else {                                                            \
            if (threads[Y].status) {                                        \
                SuspendThread(threads[Y].handle);                             \
                threads[Y].status = FALSE;                                    \
                printf("暂停线程 %d = %d\n", Y, threads[Y].dwThreadId);       \
            } else {                                                        \
                ResumeThread(threads[Y].handle);                              \
                threads[Y].status = TRUE;                                     \
                printf("重启线程 %d = %d\n", Y, threads[Y].dwThreadId);       \
            }                                                               \
        }                                                                 \
    }

void Click(int x, int y) {
    SetCursorPos(x, y);
    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
    Sleep(150);
    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
}

void RightClick(int x, int y) {
    SetCursorPos(x, y);
    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
    Sleep(150);
    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
}

/*void DoubleClick(int x, int y) {
	SetCursorPos(x, y);
	mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

	Sleep(150);

	mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
}*/

INPUT createScanCodeEvent(int vkCode, bool isDown) {
    INPUT input = {0};
    input.type = INPUT_KEYBOARD;
    input.ki.wVk = vkCode;
    input.ki.wScan = MapVirtualKeyEx(vkCode, 0, GetKeyboardLayout(0));
    input.ki.dwFlags = (isDown ? 0 : KEYEVENTF_KEYUP) | KEYEVENTF_SCANCODE;
    input.ki.time = 0;
    input.ki.dwExtraInfo = 0;
    return input;
}

void SendKeyBackground(HWND hWnd, WORD wVk) {
    PostMessage(hWnd, WM_KEYDOWN, wVk, 0);

    Sleep(150);
    // PostMessage(hWnd, WM_CHAR, wVk,0);

    PostMessage(hWnd, WM_KEYUP, wVk, 0);
}

void BackgroundMouseLeftClick(HWND hWnd, int x, int y) {
    PostMessage(hWnd, WM_LBUTTONDOWN, 0, MAKELPARAM(x, y));
    Sleep(100);
    PostMessage(hWnd, WM_LBUTTONUP, 0, MAKELPARAM(x, y));
}

void SendKeyWithAlt(HWND hWnd, UINT vk) {
    PostMessage(hWnd, WM_SYSKEYDOWN, 0x12, 0x60380001);
    Sleep(100);
    PostMessage(hWnd, WM_SYSKEYUP, 0x12, 0xC0380001);
    Sleep(100);
    // PostMessage(hWnd, WM_SYSKEYDOWN, 0x12, 0x60380001);
    // Sleep(10);
    PostMessage(hWnd, WM_SYSKEYDOWN, vk, 0);
    Sleep(100);
    PostMessage(hWnd, WM_SYSKEYUP, vk, 0);
    Sleep(100);
    // PostMessage(hWnd, WM_KEYUP, 0x12, 0);
}

void Press(int vkey) {
    /*INPUT ip;
    ip.type = INPUT_KEYBOARD;
    ip.ki.time = 0;
    ip.ki.dwFlags = KEYEVENTF_UNICODE;
    ip.ki.wScan = VK_RETURN; //VK_RETURN is the code of Return key
    ip.ki.wVk = 0;

    ip.ki.dwExtraInfo = 0;
    SendInput(1, &ip, sizeof(INPUT));*/
    INPUT input;
    //WORD vkey = VK_RETURN; // see link below
    input.type = INPUT_KEYBOARD;
    //input.ki.wScan = MapVirtualKey(vkey, MAPVK_VK_TO_VSC);

    input.ki.wVk = vkey;
    //input.ki.dwFlags = 0; // there is no KEYEVENTF_KEYDOWN
    SendInput(1, &input, sizeof(INPUT));

    input.ki.dwFlags = KEYEVENTF_KEYUP;
    SendInput(1, &input, sizeof(INPUT));

}

// 
void SendText(const char *str) {
    POINT point;
    GetCursorPos(&point);
    HWND hwnd = WindowFromPoint(point);
    size_t len = strlen(str);
    for (int i = 0; i < len; ++i) {
        SHORT c = VkKeyScan(str[i]);//tolower(VkKeyScan(str[i]));
        SendMessage(hwnd, WM_KEYDOWN, c, 0);
        SendMessage(hwnd, WM_CHAR, c, 0);
        SendMessage(hwnd, WM_KEYUP, c, 0);
        Sleep(150);
    }
}

void CtrlShiftClick(int x, int y) {
    INPUT ctrl;
    ctrl.type = INPUT_KEYBOARD;
    ctrl.ki.wVk = VK_CONTROL;
    SendInput(1, &ctrl, sizeof(INPUT));

    INPUT shift;
    shift.type = INPUT_KEYBOARD;
    shift.ki.wVk = VK_SHIFT;
    SendInput(1, &shift, sizeof(INPUT));
    Sleep(150);
    Click(x, y);
    Sleep(150);
    shift.ki.dwFlags = KEYEVENTF_KEYUP;
    SendInput(1, &shift, sizeof(INPUT));
    ctrl.ki.dwFlags = KEYEVENTF_KEYUP;
    SendInput(1, &ctrl, sizeof(INPUT));

}

void CtrlClick(int x, int y) {
    INPUT ctrl;
    ctrl.type = INPUT_KEYBOARD;
    ctrl.ki.wVk = VK_CONTROL;
    SendInput(1, &ctrl, sizeof(INPUT));


    Sleep(150);
    Click(x, y);
    Sleep(150);
    ctrl.ki.dwFlags = KEYEVENTF_KEYUP;
    SendInput(1, &ctrl, sizeof(INPUT));

}

void CtrlPress(int c) {
    INPUT input[4];
    memset(input, 0, 4 * sizeof(input[0]));
    input[0].type = INPUT_KEYBOARD;
    input[1].type = INPUT_KEYBOARD;
    input[0].ki.wVk = VK_CONTROL;
    // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    //HKL currentKBL = GetKeyboardLayout(0);
//printf("%d",VkKeyScanEx(']', currentKBL ));


    input[1].ki.wVk = c;//towupper(c);
    input[2] = input[1];
    input[3] = input[0];
    input[2].ki.dwFlags = KEYEVENTF_KEYUP;
    input[3].ki.dwFlags = KEYEVENTF_KEYUP;
    SendInput(4, input, sizeof(input[0]));
}

void ShiftPress(int c) {
    INPUT input[4];
    memset(input, 0, 4 * sizeof(input[0]));
    input[0].type = INPUT_KEYBOARD;
    input[1].type = INPUT_KEYBOARD;
    input[0].ki.wVk = VK_SHIFT;
    // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    //HKL currentKBL = GetKeyboardLayout(0);
//printf("%d",VkKeyScanEx(']', currentKBL ));


    input[1].ki.wVk = c;//towupper(c);
    input[2] = input[1];
    input[3] = input[0];
    input[2].ki.dwFlags = KEYEVENTF_KEYUP;
    input[3].ki.dwFlags = KEYEVENTF_KEYUP;
    SendInput(4, input, sizeof(input[0]));
}

void DoubleClick(int x, int y) {
    const double XSCALEFACTOR = 65535 / (GetSystemMetrics(SM_CXSCREEN) - 1);
    const double YSCALEFACTOR = 65535 / (GetSystemMetrics(SM_CYSCREEN) - 1);

    POINT cursorPos;
    GetCursorPos(&cursorPos);

    double cx = cursorPos.x * XSCALEFACTOR;
    double cy = cursorPos.y * YSCALEFACTOR;

    double nx = x * XSCALEFACTOR;
    double ny = y * YSCALEFACTOR;

    INPUT Input = {0};
    Input.type = INPUT_MOUSE;

    Input.mi.dx = (LONG) nx;
    Input.mi.dy = (LONG) ny;

    Input.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;

    SendInput(1, &Input, sizeof(INPUT));
    SendInput(1, &Input, sizeof(INPUT));

    Input.mi.dx = (LONG) cx;
    Input.mi.dy = (LONG) cy;

    Input.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;

    SendInput(1, &Input, sizeof(INPUT));
}



DWORD WINAPI Strategy(LPVOID lpParam) {
    HWND hWnd = *(HWND *) lpParam;
    return 0;
}

int CaptureAnImage(HWND hWnd) {
    HDC hdcScreen;
    HDC hdcWindow;
    HDC hdcMemDC = NULL;
    HBITMAP hbmScreen = NULL;
    BITMAP bmpScreen;
    DWORD dwBytesWritten = 0;
    DWORD dwSizeofDIB = 0;
    HANDLE hFile = NULL;
    char *lpbitmap = NULL;
    HANDLE hDIB = NULL;
    DWORD dwBmpSize = 0;

// Retrieve the handle to a display device context for the client
// area of the window.
    hdcScreen = GetDC(NULL);
    hdcWindow = GetDC(hWnd);

// Create a compatible DC, which is used in a BitBlt from the window DC.
    hdcMemDC = CreateCompatibleDC(hdcWindow);

    if (!hdcMemDC) {
        MessageBox(hWnd, L"CreateCompatibleDC has failed", L"Failed", MB_OK);
        goto done;
    }

// Get the client area for size calculation.
    RECT rcClient;
    GetClientRect(hWnd, &rcClient);

// This is the best stretch mode.
    SetStretchBltMode(hdcWindow, HALFTONE);

// The source DC is the entire screen, and the destination DC is the current window (HWND).
    if (!StretchBlt(hdcWindow,
                    0, 0,
                    rcClient.right, rcClient.bottom,
                    hdcScreen,
                    0, 0,
                    GetSystemMetrics(SM_CXSCREEN),
                    GetSystemMetrics(SM_CYSCREEN),
                    SRCCOPY)) {
        MessageBox(hWnd, L"StretchBlt has failed", L"Failed", MB_OK);
        goto done;
    }

// Create a compatible bitmap from the Window DC.
    hbmScreen = CreateCompatibleBitmap(hdcWindow, rcClient.right - rcClient.left, rcClient.bottom - rcClient.top);

    if (!hbmScreen) {
        MessageBox(hWnd, L"CreateCompatibleBitmap Failed", L"Failed", MB_OK);
        goto done;
    }

// Select the compatible bitmap into the compatible memory DC.
    SelectObject(hdcMemDC, hbmScreen);

// Bit block transfer into our compatible memory DC.
    if (!BitBlt(hdcMemDC,
                0, 0,
                rcClient.right - rcClient.left, rcClient.bottom - rcClient.top,
                hdcWindow,
                0, 0,
                SRCCOPY)) {
        MessageBox(hWnd, L"BitBlt has failed", L"Failed", MB_OK);
        goto done;
    }

// Get the BITMAP from the HBITMAP.
    GetObject(hbmScreen, sizeof(BITMAP), &bmpScreen);

    BITMAPFILEHEADER bmfHeader;
    BITMAPINFOHEADER bi;

    bi.biSize = sizeof(BITMAPINFOHEADER);
    bi.biWidth = bmpScreen.bmWidth;
    bi.biHeight = bmpScreen.bmHeight;
    bi.biPlanes = 1;
    bi.biBitCount = 32;
    bi.biCompression = BI_RGB;
    bi.biSizeImage = 0;
    bi.biXPelsPerMeter = 0;
    bi.biYPelsPerMeter = 0;
    bi.biClrUsed = 0;
    bi.biClrImportant = 0;

    dwBmpSize = ((bmpScreen.bmWidth * bi.biBitCount + 31) / 32) * 4 * bmpScreen.bmHeight;

// Starting with 32-bit Windows, GlobalAlloc and LocalAlloc are implemented as wrapper functions that
// call HeapAlloc using a handle to the process's default heap. Therefore, GlobalAlloc and LocalAlloc
// have greater overhead than HeapAlloc.
    hDIB = GlobalAlloc(GHND, dwBmpSize);
    lpbitmap = (char *) GlobalLock(hDIB);

// Gets the "bits" from the bitmap, and copies them into a buffer
// that's pointed to by lpbitmap.
    GetDIBits(hdcWindow, hbmScreen, 0,
              (UINT) bmpScreen.bmHeight,
              lpbitmap,
              (BITMAPINFO *) &bi, DIB_RGB_COLORS);

// A file is created, this is where we will save the screen capture.
    hFile = CreateFile("C:\\Users\\Administrator\\Desktop\\v2rayN\\captureqwsx.bmp",
                       GENERIC_WRITE,
                       0,
                       NULL,
                       CREATE_ALWAYS,
                       FILE_ATTRIBUTE_NORMAL, NULL);
    printf("%d", dwBmpSize);
// Add the size of the headers to the size of the bitmap to get the total file size.
    dwSizeofDIB = dwBmpSize + sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);

// Offset to where the actual bitmap bits start.
    bmfHeader.bfOffBits = (DWORD) sizeof(BITMAPFILEHEADER) + (DWORD) sizeof(BITMAPINFOHEADER);

// Size of the file.
    bmfHeader.bfSize = dwSizeofDIB;

// bfType must always be BM for Bitmaps.
    bmfHeader.bfType = 0x4D42; // BM.

    WriteFile(hFile, (LPSTR) &bmfHeader, sizeof(BITMAPFILEHEADER), &dwBytesWritten, NULL);
    WriteFile(hFile, (LPSTR) &bi, sizeof(BITMAPINFOHEADER), &dwBytesWritten, NULL);
    WriteFile(hFile, (LPSTR) lpbitmap, dwBmpSize, &dwBytesWritten, NULL);

// Unlock and Free the DIB from the heap.
    GlobalUnlock(hDIB);
    GlobalFree(hDIB);

// Close the handle for the file that was created.
    CloseHandle(hFile);

// Clean up.
    done:
    DeleteObject(hbmScreen);
    DeleteObject(hdcMemDC);
    ReleaseDC(NULL, hdcScreen);
    ReleaseDC(hWnd, hdcWindow);

    return 0;
}

// gcc main.c -lgdi32 -o a.exe
int main(void) {

// https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes

    //RegisterHotKeyWithoutModifier(VK_F11, ""); 

    thread threads[4] = {{0},
                         {0},
                         {0},
                         {0}};
    HWND hWnd = 0;
    MSG msg = {0};

    while (GetMessage(&msg, NULL, 0, 0) != 0) {
        //if (msg.message != WM_HOTKEY)
           // continue;
        //HandleHotKeyWithThread(VK_F7, 2, Strategy5);
        printf("%d",msg.wParam);
        // T
        if (msg.wParam == 0x54) {
            printf("%s","t");
            Press(0x45);
            Sleep(100);
            Press(0x59);
            
        }
        
    }
    return 0;
}
