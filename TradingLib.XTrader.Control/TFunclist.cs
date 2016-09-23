using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


public delegate Boolean  FUNC(string name,string func,string[] para);
namespace CStock
{
    public class TFunclist
    {

        public class fclist
        {
            public string name;
            public FUNC Sub;
        }
        List<fclist> SubList = new List<fclist>();
        public int Count
        {
            get { return SubList.Count; }
        }

        public TFunclist()
        {

        }
        public Boolean AddSub(string name, FUNC sub)
        {

            for (int i = 0; i < SubList.Count; i++)
            {
                fclist fl = SubList[i];
                if (fl.name == name.ToLower())
                {
                    MessageBox.Show("函数已经存在!", "信息窗口", MessageBoxButtons.OK);
                    return false;
                }
            }
            fclist fl1 = new fclist();
            fl1.name = name.ToLower();
            fl1.Sub = sub;
            SubList.Add(fl1);
            return true;
        }
        public FUNC GetFunc(string name)
        {

            for (int i = 0; i < SubList.Count; i++)
            {
                fclist fl = SubList[i];
                if (fl.name == name.ToLower())
                {
                    return fl.Sub;
                }
            }
            return null;
        }

        public string GetName(int Index)
        {
            string rs = "";
            if (Index < SubList.Count)
            {
                rs = SubList[Index].name;
            }
            return rs;
        }

        public string GetFuncList()
        {
            return SubList.ToString();
        }
    }

}