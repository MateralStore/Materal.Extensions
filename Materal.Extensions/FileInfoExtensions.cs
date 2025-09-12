namespace Materal.Extensions;

/// <summary>
/// 文件信息扩展
/// </summary>
public static class FileInfoExtensions
{
    /// <summary>
    /// 获取文件的Base64字符串
    /// </summary>
    /// <param name="fileInfo">文件信息对象</param>
    /// <returns>文件的Base64字符串</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出异常</exception>
    public static string GetBase64String(this FileInfo fileInfo)
    {
        using FileStream fileStream = OpenFile(fileInfo);
        return fileStream.ToBase64();
    }
    
    /// <summary>
    /// 判断文件是否为图片文件
    /// </summary>
    /// <param name="fileInfo">文件信息对象</param>
    /// <param name="imageType">图片类型</param>
    /// <returns>是图片文件返回true，否则返回false</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出异常</exception>
    public static bool IsImageFile(this FileInfo fileInfo, out string? imageType)
    {
        using FileStream fileStream = OpenFile(fileInfo);
        return fileStream.IsImage(out imageType);
    }
    
    /// <summary>
    /// 判断文件是否为图片文件
    /// </summary>
    /// <param name="fileInfo">文件信息对象</param>
    /// <returns>是图片文件返回true，否则返回false</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出异常</exception>
    public static bool IsImageFile(this FileInfo fileInfo)
    {
        using FileStream fileStream = OpenFile(fileInfo);
        return fileStream.IsImage();
    }
    
    /// <summary>
    /// 获取Base64格式的图片字符串
    /// </summary>
    /// <param name="fileInfo">文件信息对象</param>
    /// <returns>Base64格式的图片字符串</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出异常</exception>
    /// <exception cref="ExtensionException">文件扩展名不支持时抛出异常</exception>
    public static string GetBase64Image(this FileInfo fileInfo)
    {
        using FileStream fileStream = OpenFile(fileInfo);
        return fileStream.ToBase64Image();
    }
    
    /// <summary>
    /// 获取文件的MD5签名(32位)
    /// </summary>
    /// <param name="fileInfo">文件信息对象</param>
    /// <param name="isLower">是否小写</param>
    /// <returns>32位MD5签名</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出异常</exception>
    public static string GetMd5_32(this FileInfo fileInfo, bool isLower = false)
    {
        using FileStream fileStream = OpenFile(fileInfo);
        return fileStream.ToMd5_32Encode(isLower);
    }
    
    /// <summary>
    /// 获取文件的MD5签名(16位)
    /// </summary>
    /// <param name="fileInfo">文件信息对象</param>
    /// <param name="isLower">是否小写</param>
    /// <returns>16位MD5签名</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出异常</exception>
    public static string GetMd5_16(this FileInfo fileInfo, bool isLower = false)
    {
        using FileStream fileStream = OpenFile(fileInfo);
        return fileStream.ToMd5_16Encode(isLower);
    }
    
    /// <summary>
    /// 打开文件并返回文件流
    /// </summary>
    /// <param name="fileInfo">文件信息对象</param>
    /// <returns>文件流</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出异常</exception>
    private static FileStream OpenFile(FileInfo fileInfo)
    {
        if (!fileInfo.Exists) throw new FileNotFoundException("文件不存在", fileInfo.FullName);
        return fileInfo.OpenRead();
    }
}
