using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class LocalDialog
{
    //链接指定系统函数       打开文件对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOFN([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    //链接指定系统函数        另存为对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
    public static bool GetSFN([In, Out] OpenFileName ofn)
    {
        return GetSaveFileName(ofn);
    }
}

public class WindowsControl
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool ShowOpen([In, Out] OpenFileName ofn)
    {
        ofn.dlgOwner = GetForegroundWindow();
        return GetOpenFileName(ofn);
    }
    /// <summary>
    /// 打开系统框查找文件路径
    /// </summary>
    /// <param name="extend">要加载的文件格式</param>
    /// <returns>返回文件路径</returns>
    public static string ShowOpen(string extend)
    {
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = extend + "(*." + extend + ")\0*." + extend + "\0\0";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = ("E:\\UnityCode\\Unity-Learning\\Unity-ResLoadFrame-Learning\\Unity-ResLoadFrame\\Assets" + "/../Version").Replace("/", "\\"); //默认路径

        ofn.title = "请选择" + extend + "文件";

        ofn.defExt = extend;//显示文件的类型
                            //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        if (ShowOpen(ofn))
        {
            return ofn.file;
        }
        else
        {
            return null;
        }
    }

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
    public static bool ShowSave([In, Out] OpenFileName ofn)
    {
        ofn.dlgOwner = GetForegroundWindow();
        return GetSaveFileName(ofn);
    }
    /// <summary>
    /// 打开系统框保存文件
    /// </summary>
    /// <param name="extend">保存文件为什么类型</param>
    /// <returns>返回保存的文件路径</returns>
    public static string ShowSave(string extend)
    {
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = extend + "(*." + extend + ")\0*." + extend + "\0\0";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径

        ofn.title = "请选择" + extend + "文件";

        ofn.defExt = extend;//显示文件的类型
                            //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        if (ShowSave(ofn))
        {
            return ofn.file;
        }
        else
        {
            return null;
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
    public static void PopUpWindow()
    {
        IntPtr cw = GetForegroundWindow();
        uint st = GetWindowLong(cw, -16);
        SetWindowLong(cw, -16, st & 0x80000000);
    }

    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);

    public static int ShowMessageBox(string title, string message, MessageBoxType type = MessageBoxType.MB_OK)
    {
        return MessageBox(GetForegroundWindow(), message, title, (int)type);
    }

    public enum MessageBoxType
    {
        MB_ABORTRETRYIGNORE = 2,
        MB_CANCELTRYCONTINUE = 6,
        MB_HELP = 4,
        MB_OK = 0,
        MB_OKCANCEL = 1,
        MB_RETRYCANCEL = 5,
        MB_YESNO = 4,
        MB_YESNOCANCEL = 3
    }

    public enum MessageResult
    {
        IDABORT = 3,
        IDCANCEL = 2,
        IDCONTINUE = 11,
        IDIGNORE = 5,
        IDNO = 7,
        IDOK = 1,
        IDRETRY = 4,
        IDTRYAGAIN = 10,
        IDYES = 6
    }
}
