using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public delegate void TabClickHandle(object sender, int Index);

    //点击位置事件
    public delegate void StockEventHandler(object sender, StockEventArgs e);


    public delegate void StockMouseWheel(object sender, MouseEventArgs e);


    //
    //public delegate void KViewLoadMoreDataEventHandler(object sender, StockEventArgs e);

}
