namespace Materal.Extensions;

/// <summary>
/// 文件帮助类
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// 将文件转换为Base64字符串
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>Base64字符串</returns>
    public static string ConvertToBase64String(string filePath) => new FileInfo(filePath).GetBase64String();
    
    /// <summary>
    /// 判断是否是图片文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="imageTyp">图片类型</param>
    /// <returns>是图片文件返回true，否则返回false</returns>
    public static bool IsImageFile(string filePath, out string? imageTyp) => new FileInfo(filePath).IsImageFile(out imageTyp);
    
    /// <summary>
    /// 判断是否是图片文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>是图片文件返回true，否则返回false</returns>
    public static bool IsImageFile(string filePath) => new FileInfo(filePath).IsImageFile();
    
    /// <summary>
    /// 获取Base64格式的图片字符串
    /// </summary>
    /// <param name="filePath">图片文件路径</param>
    /// <returns>Base64格式的图片字符串</returns>
    public static string ConvertToBase64Image(string filePath) => new FileInfo(filePath).GetBase64Image();
    
    /// <summary>
    /// 获取文件的MD5签名(32位)
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="isLower">是否小写</param>
    /// <returns>32位MD5签名</returns>
    public static string GetFileMd5_32(string filePath, bool isLower = false) => new FileInfo(filePath).GetMd5_32(isLower);
    
    /// <summary>
    /// 获取文件的MD5签名(16位)
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="isLower">是否小写</param>
    /// <returns>16位MD5签名</returns>
    public static string GetFileMd5_16(string filePath, bool isLower = false) => new FileInfo(filePath).GetMd5_16(isLower);
}
