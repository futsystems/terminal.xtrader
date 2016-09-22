using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public partial class TStock
    {



        private void PrepareStockMenu()
        {
            StockMenu.Items.Clear();
            ToolStripMenuItem item = null;
            ToolStripMenuItem subitem = null;
            ToolStripMenuItem sub2item = null;
            item = new ToolStripMenuItem("变换画面");
            //m1.Click += StockMenu_Click;
            StockMenu.Items.Add(item);

            subitem = new ToolStripMenuItem(ChartMenuItems.MENU_VIEW_TIME);
            subitem.Click += StockMenu_Click;
            subitem.Enabled = !this.IsIntraView;
            item.DropDownItems.Add(subitem);

            subitem = new ToolStripMenuItem(ChartMenuItems.MENU_VIEW_K);
            subitem.Click += StockMenu_Click;
            subitem.Enabled = !this.IsBarView;
            item.DropDownItems.Add(subitem);

            StockMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());


            if (this.IsBarView)
            {
                //画线工具
                item = new ToolStripMenuItem(ChartMenuItems.MENU_DRAW);
                item.Click += StockMenu_Click;
                StockMenu.Items.Add(item);

                //技术分析
                item = new ToolStripMenuItem("技术分析");
                StockMenu.Items.Add(item);

                subitem = new ToolStripMenuItem("当前指标");
                item.DropDownItems.Add(subitem);
                sub2item = new ToolStripMenuItem(ChartMenuItems.MENU_TECH_TXT);
                sub2item.Click += StockMenu_Click;
                subitem.DropDownItems.Add(sub2item);
                sub2item = new ToolStripMenuItem(ChartMenuItems.MENU_TECH_ARGS);
                sub2item.Click += StockMenu_Click;
                subitem.DropDownItems.Add(sub2item);
                sub2item = new ToolStripMenuItem(ChartMenuItems.MENU_TECH_EDIT);
                sub2item.Click += StockMenu_Click;
                subitem.DropDownItems.Add(sub2item);
                sub2item = new ToolStripMenuItem(ChartMenuItems.MENU_TECH_RESTORE);
                sub2item.Click += StockMenu_Click;
                subitem.DropDownItems.Add(sub2item);




                subitem = new ToolStripMenuItem(ChartMenuItems.MENU_TECH_LOAD);
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                item.DropDownItems.Add(new System.Windows.Forms.ToolStripSeparator());

                FuncStr FS = new FuncStr();
                for (int i = 0; i < FS.Count(); i++)
                {
                    if (FS.functype[i] == 1)
                    {
                        //TabList.Add(FS.funcname[i]);
                        subitem = new ToolStripMenuItem(FS.funcname[i].ToUpper());
                        subitem.Tag = 0x8000 + i;
                        item.DropDownItems.Add(subitem);
                        subitem.Click += StockMenu_Click;
                    }
                }

                //周期菜单
                StockMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
                item = new ToolStripMenuItem("变换周期");
                StockMenu.Items.Add(item);

                subitem = new ToolStripMenuItem("日线");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_Day;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("周线");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_Week;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("月线");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_Month;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("季线");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_Quarter;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("年线");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_Year;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("1分钟");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_1Min;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("5分钟");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_5Min;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("15分钟");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_15Min;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("30分钟");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_30Min;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem("60分钟");
                subitem.Tag = 0x4000 + (int)KFrequencyType.F_60Min;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                //subitem = new ToolStripMenuItem("分笔");
                //subitem.Tag = 0x4000 + (int)KFrequencyType.F_Trade;
                //item.DropDownItems.Add(subitem);
                //subitem.Click += StockMenu_Click;


                //复权菜单
                item = new ToolStripMenuItem("除权状态");
                StockMenu.Items.Add(item);

                subitem = new ToolStripMenuItem(ChartMenuItems.MENU_POWER_NO);
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                item.DropDownItems.Add(new System.Windows.Forms.ToolStripSeparator());

                subitem = new ToolStripMenuItem(ChartMenuItems.MENU_POWER_BEFORE);
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
                subitem = new ToolStripMenuItem(ChartMenuItems.MENU_POWER_AFTER);
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
            }
            if (this.IsIntraView)
            {
                item = new ToolStripMenuItem("历史回忆");
                StockMenu.Items.Add(item);

                for (int i = 0; i < 10; i++)
                {
                    string s1 = "当日分时图";
                    if (i > 0)
                        s1 = String.Format("最近{0}日", i + 1);
                    subitem = new ToolStripMenuItem(s1);
                    subitem.Tag = 0x5000 + i;
                    item.DropDownItems.Add(subitem);
                    subitem.Click += StockMenu_Click;
                }

            }

            item = new ToolStripMenuItem("视图组合");
            StockMenu.Items.Add(item);
            for (int i = 0; i < this.GetMaxWindowCount(); i++)
            {
                subitem = new ToolStripMenuItem(dx[i] + "个窗口");
                subitem.Tag = 0x2000 + i + 1;
                item.DropDownItems.Add(subitem);
                subitem.Click += StockMenu_Click;
            }




            StockMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());

            item = new ToolStripMenuItem(ChartMenuItems.MENU_DETAILBOARD);
            item.Click += StockMenu_Click;
            StockMenu.Items.Add(item);

            item = new ToolStripMenuItem(ChartMenuItems.MENU_SAVE_IMG);
            item.Click += StockMenu_Click;
            StockMenu.Items.Add(item);

            item = new ToolStripMenuItem(ChartMenuItems.MENU_SAVE_DATA);
            item.Click += StockMenu_Click;
            StockMenu.Items.Add(item);
        }

        private int GetMaxWindowCount()
        {
            if (this.IsIntraView) return 2;
            if (this.IsBarView) return 6;

            return 2;
        }
    }

    
}
