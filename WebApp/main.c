/* Linux-like double-linked list implementation */

#ifndef SYSPROG21_LIST_H
#define SYSPROG21_LIST_H

#ifdef __cplusplus
extern "C" {
#endif

#include <stddef.h>

/* "typeof" is a GNU extension.
 * Reference: https://gcc.gnu.org/onlinedocs/gcc/Typeof.html
 */
#if defined(__GNUC__)
#define __LIST_HAVE_TYPEOF 1
#endif

/**
 * container_of() - Calculate address of object that contains address ptr
 * @ptr: pointer to member variable
 * @type: type of the structure containing ptr
 * @member: name of the member variable in struct @type
 *
 * Return: @type pointer of object containing ptr
 */
#ifndef container_of
#ifdef __LIST_HAVE_TYPEOF
#define container_of(ptr, type, member)                            \
    __extension__({                                                \
        const __typeof__(((type *) 0)->member) *__pmember = (ptr); \
        (type *) ((char *) __pmember - offsetof(type, member));    \
    })
#else
#define container_of(ptr, type, member) \
    ((type *) ((char *) (ptr) -offsetof(type, member)))
#endif
#endif

/**
 * struct list_head - Head and node of a double-linked list
 * @prev: pointer to the previous node in the list
 * @next: pointer to the next node in the list
 *
 * The simple double-linked list consists of a head and nodes attached to
 * this head. Both node and head share the same struct type. The list_*
 * functions and macros can be used to access and modify this data structure.
 *
 * The @prev pointer of the list head points to the last list node of the
 * list and @next points to the first list node of the list. For an empty list,
 * both member variables point to the head.
 *
 * The list nodes are usually embedded in a container structure which holds the
 * actual data. Such an container object is called entry. The helper list_entry
 * can be used to calculate the object address from the address of the node.
 */
struct list_head {
    struct list_head *prev;
    struct list_head *next;
};

/**
 * LIST_HEAD - Declare list head and initialize it
 * @head: name of the new object
 */
#define LIST_HEAD(head) struct list_head head = {&(head), &(head)}

/**
 * INIT_LIST_HEAD() - Initialize empty list head
 * @head: pointer to list head
 *
 * This can also be used to initialize a unlinked list node.
 *
 * A node is usually linked inside a list, will be added to a list in
 * the near future or the entry containing the node will be free'd soon.
 *
 * But an unlinked node may be given to a function which uses list_del(_init)
 * before it ends up in a previously mentioned state. The list_del(_init) on an
 * initialized node is well defined and safe. But the result of a
 * list_del(_init) on an uninitialized node is undefined (unrelated memory is
 * modified, crashes, ...).
 */
static inline void INIT_LIST_HEAD(struct list_head *head) {
    head->next = head;
    head->prev = head;
}

/**
 * list_add() - Add a list node to the beginning of the list
 * @node: pointer to the new node
 * @head: pointer to the head of the list
 */
static inline void list_add(struct list_head *node, struct list_head *head) {
    struct list_head *next = head->next;

    next->prev = node;
    node->next = next;
    node->prev = head;
    head->next = node;
}

/**
 * list_add_tail() - Add a list node to the end of the list
 * @node: pointer to the new node
 * @head: pointer to the head of the list
 */
static inline void list_add_tail(struct list_head *node, struct list_head *head) {
    struct list_head *prev = head->prev;

    prev->next = node;
    node->next = head;
    node->prev = prev;
    head->prev = node;
}

/**
 * list_del() - Remove a list node from the list
 * @node: pointer to the node
 *
 * The node is only removed from the list. Neither the memory of the removed
 * node nor the memory of the entry containing the node is free'd. The node
 * has to be handled like an uninitialized node. Accessing the next or prev
 * pointer of the node is not safe.
 *
 * Unlinked, initialized nodes are also uninitialized after list_del.
 *
 * LIST_POISONING can be enabled during build-time to provoke an invalid memory
 * access when the memory behind the next/prev pointer is used after a list_del.
 * This only works on systems which prohibit access to the predefined memory
 * addresses.
 */
static inline void list_del(struct list_head *node) {
    struct list_head *next = node->next;
    struct list_head *prev = node->prev;

    next->prev = prev;
    prev->next = next;

#ifdef LIST_POISONING
    node->prev = (struct list_head *) (0x00100100);
    node->next = (struct list_head *) (0x00200200);
#endif
}

/**
 * list_del_init() - Remove a list node from the list and reinitialize it
 * @node: pointer to the node
 *
 * The removed node will not end up in an uninitialized state like when using
 * list_del. Instead the node is initialized again to the unlinked state.
 */
static inline void list_del_init(struct list_head *node) {
    list_del(node);
    INIT_LIST_HEAD(node);
}

/**
 * list_empty() - Check if list head has no nodes attached
 * @head: pointer to the head of the list
 *
 * Return: 0 - list is not empty !0 - list is empty
 */
static inline int list_empty(const struct list_head *head) {
    return (head->next == head);
}

/**
 * list_is_singular() - Check if list head has exactly one node attached
 * @head: pointer to the head of the list
 *
 * Return: 0 - list is not singular !0 -list has exactly one entry
 */
static inline int list_is_singular(const struct list_head *head) {
    return (!list_empty(head) && head->prev == head->next);
}

/**
 * list_splice() - Add list nodes from a list to beginning of another list
 * @list: pointer to the head of the list with the node entries
 * @head: pointer to the head of the list
 *
 * All nodes from @list are added to to the beginning of the list of @head.
 * It is similar to list_add but for multiple nodes. The @list head is not
 * modified and has to be initialized to be used as a valid list head/node
 * again.
 */
static inline void list_splice(struct list_head *list, struct list_head *head) {
    struct list_head *head_first = head->next;
    struct list_head *list_first = list->next;
    struct list_head *list_last = list->prev;

    if (list_empty(list))
        return;

    head->next = list_first;
    list_first->prev = head;

    list_last->next = head_first;
    head_first->prev = list_last;
}

/**
 * list_splice_tail() - Add list nodes from a list to end of another list
 * @list: pointer to the head of the list with the node entries
 * @head: pointer to the head of the list
 *
 * All nodes from @list are added to to the end of the list of @head.
 * It is similar to list_add_tail but for multiple nodes. The @list head is not
 * modified and has to be initialized to be used as a valid list head/node
 * again.
 */
static inline void list_splice_tail(struct list_head *list,
                                    struct list_head *head) {
    struct list_head *head_last = head->prev;
    struct list_head *list_first = list->next;
    struct list_head *list_last = list->prev;

    if (list_empty(list))
        return;

    head->prev = list_last;
    list_last->next = head;

    list_first->prev = head_last;
    head_last->next = list_first;
}

/**
 * list_splice_init() - Move list nodes from a list to beginning of another list
 * @list: pointer to the head of the list with the node entries
 * @head: pointer to the head of the list
 *
 * All nodes from @list are added to to the beginning of the list of @head.
 * It is similar to list_add but for multiple nodes.
 *
 * The @list head will not end up in an uninitialized state like when using
 * list_splice. Instead the @list is initialized again to the an empty
 * list/unlinked state.
 */
static inline void list_splice_init(struct list_head *list,
                                    struct list_head *head) {
    list_splice(list, head);
    INIT_LIST_HEAD(list);
}

/**
 * list_splice_tail_init() - Move list nodes from a list to end of another list
 * @list: pointer to the head of the list with the node entries
 * @head: pointer to the head of the list
 *
 * All nodes from @list are added to to the end of the list of @head.
 * It is similar to list_add_tail but for multiple nodes.
 *
 * The @list head will not end up in an uninitialized state like when using
 * list_splice. Instead the @list is initialized again to the an empty
 * list/unlinked state.
 */
static inline void list_splice_tail_init(struct list_head *list,
                                         struct list_head *head) {
    list_splice_tail(list, head);
    INIT_LIST_HEAD(list);
}

/**
 * list_cut_position() - Move beginning of a list to another list
 * @head_to: pointer to the head of the list which receives nodes
 * @head_from: pointer to the head of the list
 * @node: pointer to the node in which defines the cutting point
 *
 * All entries from the beginning of the list @head_from to (including) the
 * @node is moved to @head_to.
 *
 * @head_to is replaced when @head_from is not empty. @node must be a real
 * list node from @head_from or the behavior is undefined.
 */
static inline void list_cut_position(struct list_head *head_to,
                                     struct list_head *head_from,
                                     struct list_head *node) {
    struct list_head *head_from_first = head_from->next;

    if (list_empty(head_from))
        return;

    if (head_from == node) {
        INIT_LIST_HEAD(head_to);
        return;
    }

    head_from->next = node->next;
    head_from->next->prev = head_from;

    head_to->prev = node;
    node->next = head_to;
    head_to->next = head_from_first;
    head_to->next->prev = head_to;
}

/**
 * list_move() - Move a list node to the beginning of the list
 * @node: pointer to the node
 * @head: pointer to the head of the list
 *
 * The @node is removed from its old position/node and add to the beginning of
 * @head
 */
static inline void list_move(struct list_head *node, struct list_head *head) {
    list_del(node);
    list_add(node, head);
}

/**
 * list_move_tail() - Move a list node to the end of the list
 * @node: pointer to the node
 * @head: pointer to the head of the list
 *
 * The @node is removed from its old position/node and add to the end of @head
 */
static inline void list_move_tail(struct list_head *node,
                                  struct list_head *head) {
    list_del(node);
    list_add_tail(node, head);
}

/**
 * list_entry() - Calculate address of entry that contains list node
 * @node: pointer to list node
 * @type: type of the entry containing the list node
 * @member: name of the list_head member variable in struct @type
 *
 * Return: @type pointer of entry containing node
 */
#define list_entry(node, type, member) container_of(node, type, member)

/**
 * list_first_entry() - get first entry of the list
 * @head: pointer to the head of the list
 * @type: type of the entry containing the list node
 * @member: name of the list_head member variable in struct @type
 *
 * Return: @type pointer of first entry in list
 */
#define list_first_entry(head, type, member) \
    list_entry((head)->next, type, member)

/**
 * list_last_entry() - get last entry of the list
 * @head: pointer to the head of the list
 * @type: type of the entry containing the list node
 * @member: name of the list_head member variable in struct @type
 *
 * Return: @type pointer of last entry in list
 */
#define list_last_entry(head, type, member) \
    list_entry((head)->prev, type, member)

/**
 * list_for_each - iterate over list nodes
 * @node: list_head pointer used as iterator
 * @head: pointer to the head of the list
 *
 * The nodes and the head of the list must must be kept unmodified while
 * iterating through it. Any modifications to the the list will cause undefined
 * behavior.
 */
#define list_for_each(node, head) \
    for (node = (head)->next; node != (head); node = node->next)

/**
 * list_for_each_entry - iterate over list entries
 * @entry: pointer used as iterator
 * @head: pointer to the head of the list
 * @member: name of the list_head member variable in struct type of @entry
 *
 * The nodes and the head of the list must must be kept unmodified while
 * iterating through it. Any modifications to the the list will cause undefined
 * behavior.
 *
 * FIXME: remove dependency of __typeof__ extension
 */
#ifdef __LIST_HAVE_TYPEOF
#define list_for_each_entry(entry, head, member)                       \
    for (entry = list_entry((head)->next, __typeof__(*entry), member); \
         &entry->member != (head);                                     \
         entry = list_entry(entry->member.next, __typeof__(*entry), member))
#endif

/**
 * list_for_each_safe - iterate over list nodes and allow deletes
 * @node: list_head pointer used as iterator
 * @safe: list_head pointer used to store info for next entry in list
 * @head: pointer to the head of the list
 *
 * The current node (iterator) is allowed to be removed from the list. Any
 * other modifications to the the list will cause undefined behavior.
 */
#define list_for_each_safe(node, safe, head)                     \
    for (node = (head)->next, safe = node->next; node != (head); \
         node = safe, safe = node->next)

/**
 * list_for_each_entry_safe - iterate over list entries and allow deletes
 * @entry: pointer used as iterator
 * @safe: @type pointer used to store info for next entry in list
 * @head: pointer to the head of the list
 * @member: name of the list_head member variable in struct type of @entry
 *
 * The current node (iterator) is allowed to be removed from the list. Any
 * other modifications to the the list will cause undefined behavior.
 *
 * FIXME: remove dependency of __typeof__ extension
 */
#define list_for_each_entry_safe(entry, safe, head, member)                \
    for (entry = list_entry((head)->next, __typeof__(*entry), member),     \
        safe = list_entry(entry->member.next, __typeof__(*entry), member); \
         &entry->member != (head); entry = safe,                           \
        safe = list_entry(safe->member.next, __typeof__(*entry), member))

#undef __LIST_HAVE_TYPEOF

#ifdef __cplusplus
}
#endif

#endif /* SYSPROG21_LIST_H */

// #include "input.h"

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

struct listitem {
    POINT p;
    struct list_head list;
};

DWORD WINAPI Strategy(LPVOID lpParam) {


    HWND hWnd = *(HWND *) lpParam;
    while (1) {
        Press(0x52);
        Sleep(1000);
        Press(0x52);
        Sleep(4000);
        Press(0x52);
        Sleep(1000);
        Press(0x52);
        Sleep(4000);
        Press(0x36);
        Sleep(1000);
        Press(0x36);
        Sleep(3000);
    }
    return 0;
}

struct list_head testlist;

DWORD WINAPI Strategy1() {
    struct listitem *item = NULL;
    item = (struct listitem *) malloc(sizeof(*item));
    POINT pp;
    GetCursorPos(&pp);
    item->p = pp;
    list_add_tail(&item->list, &testlist);

//    HDC hdc = GetDC(NULL);
//
//    for (int i =393; i < 979; ++i) {
//        for (int j = 197; j < 512; ++j) {
//            printf("%dx%d\n",i,j);
//            if (GetPixel(hdc, i, j) == 0x9ECCDF
//              ) {
//                Click(i, j);
//                Sleep(6000);
//
//            }
//        }
//    }
//    ReleaseDC(NULL, hdc);

    return 0;
}

DWORD WINAPI Strategy2(LPVOID lpParam) {
    struct listitem *item = NULL, *is = NULL;
    list_for_each_entry_safe (item, is, &testlist, list) {
        Click(item->p.x, item->p.y);
        Sleep(6000);
    };
    list_for_each_entry_safe (item, is, &testlist, list) {
        list_del(&item->list);
        free(item);
    }
    return 0;
}


void method1() {
    while (true) {
        HDC hdc = GetDC(NULL);
        if (hdc) {
            if (GetPixel(hdc, 675, 497) == 0xE1DAC8
                && GetPixel(hdc, 697, 491) == 0xE1DAC8) {
                Click(675, 497);
                Sleep(1000);
            }


            if (GetPixel(hdc, 1187, 36) == 0xADC8DD && GetPixel(hdc, 1188, 35) == 0xADC8DD) {
                Click(939, 93);
                Sleep(1000);
                Click(526, 109);
                Sleep(1000);
                Click(492, 574);
                Sleep(1000);
                Click(225, 326);
                Sleep(1000);
                Click(372, 550);
                Sleep(1000);
                Click(1072, 72);
                Sleep(1000);
            }
            ReleaseDC(NULL, hdc);
        }
        Sleep(1000);
    }
}

void method2() {
    int count = 0;
    while (true) {
        HDC hdc = GetDC(NULL);
        if (hdc) {
            if (GetPixel(hdc, 605, 372) == 0x34C25F
                && GetPixel(hdc, 605, 373) == 0x34C25F) {
                // 进入
                Click(549, 430);
                Sleep(1000);
            } else if (GetPixel(hdc, 1185, 35) == 0xADC8DD
                       && GetPixel(hdc, 1197, 39) == 0xADC8DD) {
                Click(645, 54);
                Sleep(1000);
                Click(268, 371);
                Sleep(1000);
                Click(590, 598);
                Sleep(1000);
                // 竞技
                count=0;
            }

            if (GetPixel(hdc, 1163, 31) == 0xADC8DD && GetPixel(hdc, 1204, 41) == 0xADC8DD) {
                // 进入
                Click(1261, 137);
                Sleep(1000);
                Click(1114, 214);
                Sleep(1000);
                Click(534, 346);
                Sleep(1000);
                Click(1073, 74);
                Sleep(1000);
                Click(1260, 137);
                Sleep(10000);
                count++;
                if (count > 30) {
                    Click(1158,522);
                    Sleep(1000);
                }
            }

            ReleaseDC(NULL, hdc);
        }
        Sleep(1000);
    }
}

DWORD WINAPI Strategy5(LPVOID lpParam) {
//    while (true) {
//        Click(569, 582);
//        Sleep(5000);
//        Click(985, 347);
//        Sleep(5000);
//        Click(845, 581);
//        Sleep(10000);
//    }
    method2();
    return 0;
}

DWORD WINAPI Strategy6(LPVOID lpParam) {
    while (true) {
        HDC hdc = GetDC(NULL);
        if (hdc) {


            if (GetPixel(hdc, 240, 93) == 0xADC8DD && GetPixel(hdc, 240, 94) == 0xADC8DD) {
                Click(868, 612);
                Sleep(1000);
                Click(868, 612);
                Sleep(1000);
                Click(409, 615);
                Sleep(3000);
            }
            ReleaseDC(NULL, hdc);
        }
        Sleep(1000);
//        Click(659,613);
//        Sleep(1000);
//        Click(659,613);
//        Sleep(5000);
//        Click(949,190);
//        Sleep(1000);
    }

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

int main(void) {
    INIT_LIST_HEAD(&testlist);

// https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes

    RegisterHotKeyWithoutModifier(VK_F11, "强化"); // I
    RegisterHotKeyWithoutModifier(VK_F10, "强化"); // I
    RegisterHotKeyWithoutModifier(VK_F9, "强化"); // I
    RegisterHotKeyWithoutModifier(VK_F8, "强化");
    RegisterHotKeyWithoutModifier(VK_F7, "强化");
    RegisterHotKeyWithoutModifier(VK_F6, "强化");

    thread threads[4] = {{0},
                         {0},
                         {0},
                         {0}};
    HWND hWnd = 0;
    MSG msg = {0};

    while (GetMessage(&msg, NULL, 0, 0) != 0) {
        if (msg.message != WM_HOTKEY)
            continue;
        HandleHotKeyWithThread(VK_F7, 2, Strategy5);
        HandleHotKeyWithThread(VK_F6, 3, Strategy6);
        HandleHotKeyWithThread(VK_F11, 0, Strategy);
        if (msg.wParam == VK_F9) {

            if (!threads[1].handle) {
                threads[1].handle =
                        CreateThread(NULL, 0, Strategy2, &hWnd, 0, &threads[1].dwThreadId);
                threads[1].status = TRUE;
                printf("创建线程 %d = %d\n", 1, threads[1].dwThreadId);
            } else {
                if (threads[1].status) {
                    // https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-suspendthread
                    //
                    TerminateThread(threads[1].handle, 0);
                    struct listitem *item = NULL, *is = NULL;
                    list_for_each_entry_safe (item, is, &testlist, list) {
                        list_del(&item->list);
                        free(item);
                    }
                    printf("终止线程 %d = %d\n", 1, threads[1].dwThreadId);
                    threads[1].status = FALSE;

                } else {
                    threads[1].handle =
                            CreateThread(NULL, 0, Strategy2, &hWnd, 0, &threads[1].dwThreadId);
                    threads[1].status = TRUE;
                }
            }
        }
        if (msg.wParam == VK_F10) {
            Strategy1();
        } else if (msg.wParam == VK_F8) {
            POINT pv = {0};
            GetCursorPos(&pv);
            HANDLE w;
            w = WindowFromPoint(pv);
            CaptureAnImage(w);
            // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclipboarddata

        }
        //		if (msg.wParam == 0x73) {
        //			system("ffmpeg -hide_banner -rtbufsize 150M -f gdigrab
        //-framerate 30 -offset_x 0 -offset_y 0 -video_size 1280x720 -draw_mouse 1
        // -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 28
        //-pix_fmt
        // yuv420p -movflags +faststart -y
        //\"C:\\Users\\Administrator\\Desktop\\1.mp4");
        //
        //		}
    }
    return 0;
}
