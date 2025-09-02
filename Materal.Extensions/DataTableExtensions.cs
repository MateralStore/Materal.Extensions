using System.Collections.Concurrent;
using System.Data;

namespace Materal.Extensions;

/// <summary>
/// DataTable扩展
/// </summary>
public static class DataTableExtensions
{
    /// <summary>
    /// 将IEnumerable集合转换为DataTable
    /// </summary>
    /// <typeparam name="T">集合元素类型，必须有无参构造函数</typeparam>
    /// <param name="listM">要转换的集合</param>
    /// <returns>包含集合数据的DataTable</returns>
    /// <exception cref="ArgumentNullException">当listM为null时抛出</exception>
    public static DataTable ToDataTable<T>(this IEnumerable<T> listM)
        where T : new()
    {
        listM ??= [];
        Type type = typeof(T);
        DataTable dt = type.ToDataTable();
        foreach (T item in listM)
        {
            if (item is null) continue;
            dt.Rows.Add(item.ToDataRow(dt.NewRow()));
        }
        return dt;
    }

    /// <summary>
    /// 将对象转换为数据行
    /// </summary>
    /// <param name="obj">要转换的对象</param>
    /// <param name="dr">数据行模板</param>
    /// <returns>填充了对象数据的数据行</returns>
    /// <exception cref="ArgumentNullException">当obj或dr为null时抛出</exception>
    /// <exception cref="ExtensionException">当dr为null时抛出</exception>
    public static DataRow ToDataRow(this object obj, DataRow dr)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));
        if (dr is null) throw new ExtensionException("数据行不可为空");

        Type type = obj.GetType();
        PropertyInfo[] props = type.GetProperties();
        foreach (PropertyInfo prop in props)
        {
            object? value = prop.GetValue(obj, null);
            dr[prop.Name] = value ?? DBNull.Value;
        }
        return dr;
    }

    /// <summary>
    /// 将对象转换为数据行
    /// </summary>
    /// <param name="obj">要转换的对象</param>
    /// <returns>包含对象数据的新数据行</returns>
    /// <exception cref="ArgumentNullException">当obj为null时抛出</exception>
    public static DataRow ToDataRow(this object obj)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        Type type = obj.GetType();
        DataTable dt = type.ToDataTable();
        DataRow dr = dt.NewRow();
        return obj.ToDataRow(dr);
    }

    /// <summary>
    /// 通过数据行设置对象的属性值
    /// </summary>
    /// <param name="obj">要设置属性值的对象</param>
    /// <param name="dr">包含属性值的数据行</param>
    /// <param name="exceptions">用于收集转换过程中发生的异常的列表，如果为null则直接抛出异常</param>
    /// <exception cref="ArgumentNullException">当obj或dr为null时抛出</exception>
    public static void SetValueByDataRow(this object obj, DataRow dr, List<Exception>? exceptions = null)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));
        if (dr is null) throw new ArgumentNullException(nameof(dr));

        Type type = obj.GetType();
        PropertyInfo[] props = PropertyInfoCache.GetProperties(type);
        foreach (PropertyInfo prop in props)
        {
            try
            {
                prop.SetValue(obj, dr[prop.Name].ConvertTo(prop.PropertyType), null);
            }
            catch (Exception exception)
            {
                if (exceptions is not null)
                {
                    exceptions.Add(exception);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// 从数据行获取指定类型的对象
    /// </summary>
    /// <typeparam name="T">要创建的对象类型，必须有无参构造函数</typeparam>
    /// <param name="dataRow">包含对象数据的数据行</param>
    /// <param name="exceptions">用于收集转换过程中发生的异常的列表，如果为null则直接抛出异常</param>
    /// <returns>从数据行创建的对象</returns>
    /// <exception cref="ArgumentNullException">当dataRow为null时抛出</exception>
    /// <exception cref="ExtensionException">当对象实例化失败时抛出</exception>
    public static T GetValue<T>(this DataRow dataRow, List<Exception>? exceptions = null)
        where T : new()
    {
        if (dataRow is null) throw new ArgumentNullException(nameof(dataRow));

        T result = typeof(T).Instantiation<T>() ?? throw new ExtensionException("转换失败");
        result.SetValueByDataRow(dataRow, exceptions);
        return result;
    }

    /// <summary>
    /// 从数据行的指定索引获取字符串值
    /// </summary>
    /// <param name="row">数据行</param>
    /// <param name="index">列索引</param>
    /// <returns>指定位置的字符串值，如果索引超出范围则返回null</returns>
    /// <exception cref="ArgumentNullException">当row为null时抛出</exception>
    public static string? GetStringValue(this DataRow row, int index)
    {
        if (row is null) throw new ArgumentNullException(nameof(row));

        if (row.ItemArray is null || row.ItemArray.Length <= index) return null;
        string? result = row[index].ToString();
        return result;
    }

    /// <summary>
    /// 将数据表转换为对象列表
    /// </summary>
    /// <typeparam name="T">列表元素类型，必须有无参构造函数</typeparam>
    /// <param name="dataTable">要转换的数据表</param>
    /// <param name="exceptions">用于收集转换过程中发生的异常的列表，如果为null则直接抛出异常</param>
    /// <returns>包含数据表数据的对象列表</returns>
    /// <exception cref="ArgumentNullException">当dataTable为null时抛出</exception>
    public static List<T> ToList<T>(this DataTable dataTable, List<Exception>? exceptions = null)
        where T : new()
    {
        if (dataTable is null) throw new ArgumentNullException(nameof(dataTable));
        List<T> result = new(dataTable.Rows.Count);
        DataRowCollection rows = dataTable.Rows;
        foreach (DataRow dr in rows)
        {
            T? value = dr.GetValue<T>(exceptions);
            if (value is null) continue;
            result.Add(value);
        }
        return result;
    }

    /// <summary>
    /// 将数据表转换为对象数组
    /// </summary>
    /// <typeparam name="T">数组元素类型，必须有无参构造函数</typeparam>
    /// <param name="dataTable">要转换的数据表</param>
    /// <param name="exceptions">用于收集转换过程中发生的异常的列表，如果为null则直接抛出异常</param>
    /// <returns>包含数据表数据的对象数组</returns>
    /// <exception cref="ArgumentNullException">当dataTable为null时抛出</exception>
    public static T?[] ToArray<T>(this DataTable dataTable, List<Exception>? exceptions = null)
        where T : new()
    {
        if (dataTable is null) throw new ArgumentNullException(nameof(dataTable));

        int count = dataTable.Rows.Count;
        T?[] result = new T?[count];

        // 优化：缓存行集合引用
        DataRowCollection rows = dataTable.Rows;
        for (int i = 0; i < count; i++)
        {
            result[i] = rows[i].GetValue<T>(exceptions);
        }
        return result;
    }

    /// <summary>
    /// 将数据集转换为对象列表的列表
    /// </summary>
    /// <typeparam name="T">列表元素类型，必须有无参构造函数</typeparam>
    /// <param name="dataSet">要转换的数据集</param>
    /// <param name="exceptions">用于收集转换过程中发生的异常的列表，如果为null则直接抛出异常</param>
    /// <returns>包含数据集数据的对象列表的列表</returns>
    /// <exception cref="ArgumentNullException">当dataSet为null时抛出</exception>
    public static List<List<T>> ToList<T>(this DataSet dataSet, List<Exception>? exceptions = null)
        where T : new()
    {
        if (dataSet is null) throw new ArgumentNullException(nameof(dataSet));
        List<List<T>> result = new(dataSet.Tables.Count);
        DataTableCollection tables = dataSet.Tables;
        foreach (DataTable dt in tables)
        {
            result.Add(dt.ToList<T>(exceptions));
        }
        return result;
    }

    /// <summary>
    /// 将数据集转换为二维对象数组
    /// </summary>
    /// <typeparam name="T">数组元素类型，必须有无参构造函数</typeparam>
    /// <param name="dataSet">要转换的数据集</param>
    /// <param name="exceptions">用于收集转换过程中发生的异常的列表，如果为null则直接抛出异常</param>
    /// <returns>包含数据集数据的二维对象数组</returns>
    /// <exception cref="ArgumentNullException">当dataSet为null时抛出</exception>
    public static T?[,] ToArray<T>(this DataSet dataSet, List<Exception>? exceptions = null)
        where T : new()
    {
        if (dataSet is null) throw new ArgumentNullException(nameof(dataSet));

        int tableCount = dataSet.Tables.Count;
        DataTableCollection tables = dataSet.Tables;
        int[] rowCounts = new int[tableCount];
        int rowCount = 0;

        // 优化：在单次循环中计算所有行数并找到最大值
        for (int i = 0; i < tableCount; i++)
        {
            rowCounts[i] = tables[i].Rows.Count;
            if (rowCount < rowCounts[i])
                rowCount = rowCounts[i];
        }

        T?[,] result = new T?[tableCount, rowCount];
        for (int i = 0; i < tableCount; i++)
        {
            DataTable table = tables[i];
            int currentRowCount = rowCounts[i];
            DataRowCollection rows = table.Rows;
            for (int j = 0; j < currentRowCount; j++)
            {
                result[i, j] = rows[j].GetValue<T>(exceptions);
            }
        }
        return result;
    }

    /// <summary>
    /// 将数据表转换为字典列表
    /// </summary>
    /// <param name="dataTable">要转换的数据表</param>
    /// <param name="customFunc">自定义值转换函数，可为null</param>
    /// <returns>包含数据表数据的字典列表</returns>
    /// <exception cref="ArgumentNullException">当dataTable为null时抛出</exception>
    public static List<Dictionary<string, object?>> ToDictionaries(this DataTable dataTable, Func<object?, object?>? customFunc = null)
    {
        if (dataTable is null) throw new ArgumentNullException(nameof(dataTable));
        List<Dictionary<string, object?>> list = new(dataTable.Rows.Count);
        DataColumnCollection columns = dataTable.Columns;
        DataRowCollection rows = dataTable.Rows;

        foreach (DataRow row in rows)
        {
            Dictionary<string, object?> dictionary = new(columns.Count);
            for (int i = 0; i < columns.Count; i++)
            {
                object? value = row[i];
                if (customFunc is not null)
                {
                    value = customFunc(value);
                }
                dictionary.Add(columns[i].ColumnName, value);
            }
            list.Add(dictionary);
        }
        return list;
    }

    /// <summary>
    /// PropertyInfo缓存
    /// </summary>
    internal static class PropertyInfoCache
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
        /// <summary>
        /// 获取类型的属性信息数组
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>属性信息数组</returns>
        public static PropertyInfo[] GetProperties(Type type) => _propertyCache.GetOrAdd(type, t => t.GetProperties());
    }
}
