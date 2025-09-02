namespace Materal.Extensions;

/// <summary>
/// 文件夹信息扩展
/// </summary>
public static class DirectoryInfoExtensions
{
    /// <summary>
    /// 检查目标目录是否为源目录的子目录
    /// </summary>
    /// <param name="source">源目录</param>
    /// <param name="target">目标目录</param>
    /// <returns>如果是子目录返回true，否则返回false</returns>
    private static bool IsSubdirectory(DirectoryInfo source, DirectoryInfo target)
    {
        DirectoryInfo? current = target;
        while (current is not null)
        {
            if (string.Equals(current.FullName, source.FullName, StringComparison.OrdinalIgnoreCase)) return true;
            current = current.Parent;
        }
        return false;
    }

    /// <summary>
    /// 复制目录及其所有内容到目标路径
    /// </summary>
    /// <param name="directoryInfo">源目录信息</param>
    /// <param name="targetPath">目标路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为true</param>
    /// <exception cref="ArgumentNullException">当 directoryInfo 或 targetPath 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 targetPath 为空或仅包含空白字符时抛出</exception>
    /// <exception cref="DirectoryNotFoundException">当源目录不存在时抛出</exception>
    public static void CopyTo(this DirectoryInfo directoryInfo, string targetPath, bool overwrite = true)
        => CopyTo(directoryInfo, new DirectoryInfo(targetPath), overwrite);
    /// <summary>
    /// 复制目录及其所有内容到目标目录
    /// </summary>
    /// <param name="directoryInfo">源目录信息</param>
    /// <param name="targetDirectoryInfo">目标目录信息</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为true</param>
    /// <exception cref="ArgumentNullException">当 directoryInfo 或 targetDirectoryInfo 为 null 时抛出</exception>
    /// <exception cref="DirectoryNotFoundException">当源目录不存在时抛出</exception>
    /// <exception cref="IOException">当目标路径是源路径的子目录时抛出</exception>
    public static void CopyTo(this DirectoryInfo directoryInfo, DirectoryInfo targetDirectoryInfo, bool overwrite = true)
    {
        // 检查参数
        if (directoryInfo is null) throw new ArgumentNullException(nameof(directoryInfo));
        if (targetDirectoryInfo is null) throw new ArgumentNullException(nameof(targetDirectoryInfo));
        // 检查源目录是否存在
        if (!directoryInfo.Exists) throw new DirectoryNotFoundException($"源目录不存在: {directoryInfo.FullName}");
        // 检查目标路径是否为源路径的子目录，防止无限递归
        if (IsSubdirectory(directoryInfo, targetDirectoryInfo)) throw new IOException("目标路径不能是源路径的子目录");

        // 确保目标目录存在
        if (!targetDirectoryInfo.Exists)
        {
            targetDirectoryInfo.Create();
            targetDirectoryInfo.Refresh();
        }

        // 复制所有文件
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        foreach (FileInfo fileInfo in fileInfos)
        {
            fileInfo.CopyTo(Path.Combine(targetDirectoryInfo.FullName, fileInfo.Name), overwrite);
        }

        // 递归复制所有子目录
        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        foreach (DirectoryInfo subDirectoryInfo in directoryInfos)
        {
            subDirectoryInfo.CopyTo(Path.Combine(targetDirectoryInfo.FullName, subDirectoryInfo.Name), overwrite);
        }
    }

    /// <summary>
    /// 清空文件夹中的所有内容，包括子目录和文件，但保留文件夹本身
    /// </summary>
    /// <param name="directoryInfo">要清空的文件夹信息</param>
    /// <exception cref="ArgumentNullException">当 directoryInfo 为 null 时抛出</exception>
    /// <exception cref="DirectoryNotFoundException">当目录不存在时抛出</exception>
    /// <exception cref="UnauthorizedAccessException">当没有足够权限删除文件或目录时抛出</exception>
    /// <exception cref="IOException">当文件或目录正在使用中无法删除时抛出</exception>
    public static bool TryClear(this DirectoryInfo directoryInfo) => TryClear(directoryInfo, out _);

    /// <summary>
    /// 清空文件夹中的所有内容，包括子目录和文件，但保留文件夹本身
    /// </summary>
    /// <param name="directoryInfo">要清空的文件夹信息</param>
    /// <param name="exceptions">执行后的异常信息</param>
    /// <exception cref="ArgumentNullException">当 directoryInfo 为 null 时抛出</exception>
    /// <exception cref="DirectoryNotFoundException">当目录不存在时抛出</exception>
    /// <exception cref="UnauthorizedAccessException">当没有足够权限删除文件或目录时抛出</exception>
    /// <exception cref="IOException">当文件或目录正在使用中无法删除时抛出</exception>
    public static bool TryClear(this DirectoryInfo directoryInfo, out List<Exception> exceptions)
    {
        // 检查参数
        if (directoryInfo is null) throw new ArgumentNullException(nameof(directoryInfo));
        // 检查目录是否存在
        if (!directoryInfo.Exists) throw new DirectoryNotFoundException($"目录不存在: {directoryInfo.FullName}");
        exceptions = [];
        // 删除所有文件
        FileInfo[] allFileInfos = directoryInfo.GetFiles();
        foreach (FileInfo fileInfo in allFileInfos)
        {
            try
            {
                fileInfo.Delete();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        // 递归删除所有子目录
        DirectoryInfo[] allDirectoryInfos = directoryInfo.GetDirectories();
        foreach (DirectoryInfo subDirectoryInfo in allDirectoryInfos)
        {
            try
            {
                subDirectoryInfo.Delete(true);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        return exceptions.Count <= 0;
    }
}
