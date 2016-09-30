using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.DataFarmManager
{

    public class ValueObject<T>
    {

        private string _name;
        private T _value;
        public T Value { get { return _value; } set { _value = value; } }
        public string Name { get { return _name; } set { _name = value; } }

    }


    public interface IDataSource
    {
        object DataSource { get; set; }
        string DisplayMember { get; set; }
        string ValueMember { get; set; }
        void BindDataSource(ArrayList list);
    }

    public class Invalid2IDataSource : IDataSource
    {

        public Invalid2IDataSource()
        {

        }

        public object DataSource { get; set; }
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }

        /// <summary>
        /// 绑定对应的数据
        /// </summary>
        /// <param name="list"></param>
        public void BindDataSource(ArrayList list)
        {
            this.DataSource = list;
            this.ValueMember = "Value";
            this.DisplayMember = "Name";

        }
    }

    public class CheckedListBox2IDataSource : IDataSource
    {
        CheckedListBox _clb;
        public CheckedListBox2IDataSource(CheckedListBox c)
        {
            _clb = c;
        }
        public object DataSource { get { return _clb.DataSource; } set { _clb.DataSource = value; } }
        public string DisplayMember { get { return _clb.DisplayMember; } set { _clb.DisplayMember = value; } }
        public string ValueMember { get { return _clb.ValueMember; } set { _clb.ValueMember = value; } }

        /// <summary>
        /// 绑定对应的数据
        /// </summary>
        /// <param name="list"></param>
        public void BindDataSource(ArrayList list)
        {
            this.DataSource = list;
            this.ValueMember = "Value";
            this.DisplayMember = "Name";

        }
    }


    public class ListBox2IDataSource : IDataSource
    {
        ListBox _lc;
        public ListBox2IDataSource(ListBox lc)
        {
            _lc = lc;
        }

        public object DataSource { get { return _lc.DataSource; } set { _lc.DataSource = value; } }
        public string DisplayMember { get { return _lc.DisplayMember; } set { _lc.DisplayMember = value; } }
        public string ValueMember { get { return _lc.ValueMember; } set { _lc.ValueMember = value; } }

        /// <summary>
        /// 绑定对应的数据
        /// </summary>
        /// <param name="list"></param>
        public void BindDataSource(ArrayList list)
        {
            this.DataSource = list;
            this.ValueMember = "Value";
            this.DisplayMember = "Name";

        }
    }

    public class ComboBox2IDataSource : IDataSource
    {
        ComboBox _lc;
        public ComboBox2IDataSource(ComboBox lc)
        {
            _lc = lc;
        }

        public object DataSource { get { return _lc.DataSource; } set { _lc.DataSource = value; } }
        public string DisplayMember { get { return _lc.DisplayMember; } set { _lc.DisplayMember = value; } }
        public string ValueMember { get { return _lc.ValueMember; } set { _lc.ValueMember = value; } }

        /// <summary>
        /// 绑定对应的数据
        /// </summary>
        /// <param name="list"></param>
        public void BindDataSource(ArrayList list)
        {
            this.DataSource = list;
            this.ValueMember = "Value";
            this.DisplayMember = "Name";

        }
    }
}
