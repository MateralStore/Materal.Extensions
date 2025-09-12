using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Materal.Extensions.Models
{
    /// <summary>
    /// 整数转换模型
    /// </summary>
    public class IntConvertModel
    {
        /// <summary>
        /// 数字
        /// </summary>
        public ReadOnlyDictionary<int, string> Numbers { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public ReadOnlyCollection<string> Units { get; set; }
        /// <summary>
        /// 扩展
        /// </summary>
        public ReadOnlyDictionary<int, string> Extend { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public IntConvertModel()
        {
            Numbers = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>());
            Units = new ReadOnlyCollection<string>(new List<string>());
            Extend = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>());
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numbers">数字映射</param>
        /// <param name="units">单位</param>
        /// <param name="extend">扩展</param>
        public IntConvertModel(Dictionary<int, string> numbers, List<string> units, Dictionary<int, string> extend)
        {
            Numbers = new ReadOnlyDictionary<int, string>(numbers);
            Units = new ReadOnlyCollection<string>(units);
            Extend = new ReadOnlyDictionary<int, string>(extend);
        }
    }
}
