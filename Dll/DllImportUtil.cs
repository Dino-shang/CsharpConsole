#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：DllImportUtil
// 创 建 者：Dino
// 创建时间：2025年08月17日 星期日 12:22
// 文件版本：V1.0.0
//===============================================================
// 功能描述：
//
//
//----------------------------------------------------------------*/

#endregion

using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleTest.Dll;

public class DllImportUtil
{
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    /// <summary>
    /// 精确匹配
    /// </summary>
    public static void FindTargetWindow()
    {
        IntPtr rider = FindWindow(null, "记事本");

        if (rider == IntPtr.Zero)
        {
            Console.WriteLine("not found");
        }
        else
        {
            Console.WriteLine("find target window!");
        }
    }

    /// <summary>
    /// 模糊搜索包含指定名称的窗口句柄
    /// </summary>
    public static void FindContainWIndow()
    {
        EnumWindows(FindNotepadWindow, IntPtr.Zero);
        if (_foundHandle != IntPtr.Zero)
        {
            Console.WriteLine($"成功找到窗口，句柄是: {_foundHandle}");
        }
        else
        {
            Console.WriteLine("未找到包含 '记事本' 的窗口。");
        }
    }
    
    private static IntPtr _foundHandle = IntPtr.Zero;
    

    private static bool FindNotepadWindow(IntPtr hWnd, IntPtr lParam)
    {
        StringBuilder windowTitle = new StringBuilder(256);
        GetWindowText(hWnd, windowTitle, windowTitle.Capacity);

        if (windowTitle.ToString().Contains("Notepad"))
        {
            _foundHandle = hWnd;
            return false; // 找到后返回 false，停止枚举
        }

        return true; // 继续枚举下一个窗口
    }
}