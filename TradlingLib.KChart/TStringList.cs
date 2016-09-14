using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace CStock
{
    public class TStringList
    {
        private int m_Capacity;
        private string[] m_Strings;
        private int m_Size;

        /// <summary>
        /// 数据个数属性
        /// </summary>
        public int Count
        {
            get
            {
                return m_Size;
            }
        }

        /// <summary>
        /// 缓存大小属性
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_Capacity;
            }
            set
            {
                if (m_Strings == null)
                {
                    return;
                }

                if (value != m_Strings.Length)
                {
                    if (value < this.m_Size)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    if (value > 0)
                    {
                        string[] objArray1 = new string[value];
                        if (this.m_Size > 0)
                        {
                            Array.Copy(this.m_Strings, 0, objArray1, 0, this.m_Size);
                        }
                        this.m_Strings = objArray1;
                    }
                    else
                    {
                        this.m_Strings = new string[0x10];
                    }
                }
            }
        }

        public string Text
        {
            get
            {
                return this.ToString();
            }
            set
            {
                string[] ss;
                String s1;
                s1 = value.Replace("\r", "");
                ss = s1.Split('\n');
                if (m_Strings.Length > ss.Length)
                {
                    Array.Copy(ss, m_Strings, ss.Length);
                    m_Size = ss.Length;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TStringList()
            : this(100)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TStringList(int capacity)
        {
            m_Capacity = capacity;

            m_Strings = new string[capacity];
            m_Size = 0;
        }

        public void Dispose()
        {
        }
        public string values(string s)
        {
            string s1, s2;
            string[] fj;
            s1 = "";
            s = s.ToLower();
            for (int i = 0; i < m_Size; i++)
            {
                s2 = m_Strings[i].ToLower();
                fj = s2.Split('=');
                if (fj.Length > 1)
                {
                    if (fj[0] == s)
                    {
                        s1 = fj[1];
                        break;
                    }
                }
            }

            return s1;
        }
        /// <summary>
        /// 读取某行内容
        /// </summary>
        /// <param name="index"></param>
        public string this[int index]
        {
            get
            {
                if ((index < 0) || (index >= m_Size))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return this.m_Strings[index];
            }
            set
            {
                if ((index < 0) || (index >= m_Size))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.m_Strings[index] = value;
            }
        }

        /// <summary>
        /// 调整缓存大小
        /// </summary>
        protected void EnsureCapacity(int min)
        {
            if (this.m_Strings.Length < min)
            {
                int num1 = (this.m_Strings.Length == 0) ? 0x10 : (this.m_Strings.Length * 2);
                if (num1 < min)
                {
                    num1 = min;
                }
                this.Capacity = num1;
            }
        }

        /// <summary>
        /// 追加一行
        /// </summary>
        public int Add(string value)
        {
            if (this.Count == m_Strings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }

            m_Strings[this.Count] = value;
            m_Size++;

            return m_Size;
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="index"></param>
        public int InsertText(int index, string value)
        {
            if (index < 0)
            {
                index = 0;
            }

            if (this.Count == m_Strings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }

            if (index < this.Count)
            {
                Array.Copy(this.m_Strings, index, this.m_Strings, index + 1, this.m_Size - index);
            }

            m_Strings[index] = value;
            m_Size++;

            return m_Size;
        }

        /// <summary>
        /// 查找数据的位置
        /// </summary>
        public int IndexOf(string value)
        {
            return Array.IndexOf(this.m_Strings, value, 0, this.m_Size);
        }

        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.m_Size))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.m_Size--;
            if (index < this.m_Size)
            {
                Array.Copy(this.m_Strings, index + 1, this.m_Strings, index, this.m_Size - index);
            }
            this.m_Strings[this.m_Size] = null;
        }
        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="index"></param>
        public void Delete(int index)
        {
            RemoveAt(index);
        }
        /// <summary>
        /// 转换为字符串。
        /// </summary>
        public override string ToString()
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                s.Append(m_Strings[i]);
                if (i<Count-1)
                    s.Append("\r\n");
            }
            return s.ToString();
        }

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string ToString(int startIndex, int count)
        {
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            else if (startIndex >= this.Count)
            {
                return "";
            }

            if (count <= 0)
            {
                return "";
            }

            if (count + startIndex > this.Count)
            {
                count = this.Count - startIndex;
            }

            System.Text.StringBuilder s = new System.Text.StringBuilder(this.Count);

            for (int i = startIndex; i < count; i++)
            {
                s.Append(m_Strings[i] + "\r\n");
            }

            return s.ToString();
        }

        /// <summary>
        /// 清除内容
        /// </summary>
        public void Clear()
        {
            this.m_Size = 0;
        }

        /// <summary>
        /// 保存为一个文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        public void SaveToFile(string fileName, System.Text.Encoding encoding)
        {
            System.IO.StreamWriter sw2 = new System.IO.StreamWriter(fileName, false, encoding);
            for (int i = 0; i < this.Count; i++)
            {
                sw2.Write(m_Strings[i] + "\r\n");
            }

            sw2.Close();
        }

        public void SaveToFile(string fileName)
        {
            System.IO.StreamWriter sw2 = new System.IO.StreamWriter(fileName, false, System.Text.ASCIIEncoding.Default);
            for (int i = 0; i < this.Count; i++)
            {
                sw2.Write(m_Strings[i] + "\r\n");
            }

            sw2.Close();
        }

        /// <summary>
        /// 读入一个文本文件
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFromFile(string fileName)
        {
            this.Clear();
            if (fileName == null)
                return;
            if (fileName.Length == 0)
                return;
            if (File.Exists(fileName) == false)
                return;
            System.IO.StreamReader sr2 = new System.IO.StreamReader(fileName, System.Text.ASCIIEncoding.Default);

            while (sr2.Peek() >= 0)
            {
                this.Add(sr2.ReadLine());
            }

            sr2.Close();
        }

        public void LoadFromFile(string fileName, System.Text.Encoding encoding)
        {
            this.Clear();

            System.IO.StreamReader sr2 = new System.IO.StreamReader(fileName, encoding);

            while (sr2.Peek() >= 0)
            {
                this.Add(sr2.ReadLine());
            }

            sr2.Close();
        }
        /// <summary>
        /// 取得=之后数值,并返回当前行
        /// </summary>
        /// <param name="s"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public string values(string s, out int row)
        {
            string s1, s2, s3;
            s1 = "";
            s2 = s.ToLower() + "=";
            row = -1;
            for (int i = 0; i < m_Size; i++)
            {
                s3 = m_Strings[i].ToLower();
                if (s3.IndexOf(s2) > -1)
                {
                    s1 = m_Strings[i].Substring(s.Length + 1);
                    row = i;
                    break;
                }
            }
            return s1;
        }
    }

}