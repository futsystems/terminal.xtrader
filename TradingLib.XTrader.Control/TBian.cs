using System;
using System.Collections.Generic;
using System.Text;
namespace CStock
{
public class TBian
{
    const int DATALEN = 20000;
    public double[] value;
    public string name;
    public int len;

    public TBian()
    {

    }
    /// <summary>
    /// 数据集 默认设置4万个数据
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Len"></param>
    public TBian(string Name, int Len)
    {
        name = Name;
        value = new double[DATALEN];
        len = Len;
        Array.Clear(value, 0, value.Length);
    }

   /// <summary>
   /// 复制变量
   /// </summary>
   /// <param name="b1">原变量</param>
    public void SetBian(TBian b1)
    {
        //name = b1.name;
        len = b1.len;
        Array.Copy(b1.value, value, b1.len);
    }
    ///// <summary>
    ///// 追加到尾部
    ///// </summary>
    ///// <param name="source">原数据</param>
    ///// <param name="sourceLen">长度</param>
    ///// <param name="append">是否追加</param>
    //public void SetDouble(double[] source, int sourceLen,Boolean append)
    //{
    //    if (append)
    //    {
    //        Array.Copy(source, 0, value, len, sourceLen);
    //        len += sourceLen;
    //    }
    //    else
    //    {
    //        Array.Copy(source, value, sourceLen);
    //        len = sourceLen;
    //    }
    //}
    
    /// <summary>
    /// 在数据集前面插入
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sourceLen"></param>
    public void FInsert(double[] source, int sourceLen)
    {
        double[] tmp = new double[len];
        Array.Copy(value, tmp, len);
        Array.Copy(source, value, sourceLen);
        Array.Copy(tmp,0, value, sourceLen, len);
        len += sourceLen;
    }

     /// <summary>
     /// 在数据集后面插入
     /// </summary>
     /// <param name="source"></param>
     /// <param name="sourceLen"></param>
    public void BInsert(double[] source, int sourceLen)
    {
        Array.Copy(source, 0, value, len, sourceLen);
        len += sourceLen;
    }

    /// <summary>
    /// 在数据集末尾添加一个数据
    /// </summary>
    /// <param name="val"></param>
    public void AppendValue(double val)
    {
        value[len] = val;
        len++;
    }

    /// <summary>
    /// 修改某指定位置的值
    /// </summary>
    /// <param name="index"></param>
    /// <param name="val"></param>
    public void EditValue(int index, double val)
    {
        //if (len <= index)
        //{
        //    value[index] = val;
        //    len = index + 1;
        //}
        value[index] = val;
        if (index + 1 > len)
            len = index+1;
    }

    /// <summary>
    /// 设置数据最后位置
    /// 通过调整length可以控制数据末尾有效访问位置
    /// 比如在末尾添加一段数据，数据首位通过查询位置为5
    /// 则调整length为5将有效数据控制在位置0-4,然后将新数据直接复制到当前数据集末尾则完成数据屏蔽
    /// index调整后 该index之后的数据将无效
    /// </summary>
    /// <param name="index"></param>
    public void ResetToIndex(int index)
    {
        len = index;
    }

    /// <summary>
    /// 更新最后一个数据,或追加一个数据
    /// </summary>
    /// <param name="f1"></param>
    /// <param name="app"></param>
    //public void AddOne(double f1, Boolean app)
    //{
    //    if (app || (len == 0))
    //    {
    //        value[len] = f1;
    //        len++;
    //    }
    //    else
    //        value[len - 1] = f1;
    //}

    ///// <summary>
    ///// 设置某个位置的数据
    ///// </summary>
    ///// <param name="index"></param>
    ///// <param name="val"></param>
    //public void SetValue(int index, double val)
    //{
    //    value[index] = val;
    //    if (index + 1 > len)
    //        len = index+1;
    //}

    ///// <summary>
    ///// 再数据集末尾追加一个值
    ///// </summary>
    ///// <param name="val"></param>
    //public void AppendValue(double val)
    //{
    //    value[len] = val;
    //    len++;
    //}
    


    public static Array Redim(Array origArray, int desiredSize)
    {
        Type t = origArray.GetType().GetElementType();
        Array newArray = Array.CreateInstance(t, desiredSize);
        Array.Copy(origArray, 0, newArray, 0, Math.Min(origArray.Length, desiredSize));
        return newArray;
    }
 

}


}