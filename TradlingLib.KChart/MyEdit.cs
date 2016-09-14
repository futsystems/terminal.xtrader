using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CStock
{
    public class SyntaxEditor : RichTextBox
    {
        private System.ComponentModel.Container components = null;
        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;

        public SyntaxEditor()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();
            base.WordWrap = false;
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            //
            // SyntaxEditor
            //
            this.Name = "SyntaxEditor";

        }
        #endregion

        //重写基类的OnTextChanged方法。为了提高效率，程序是对当前文本插入点所在行进行扫描，

        //以空格为分割符，判断每个单词是否为关键字，并进行着色。

        protected override void OnTextChanged(EventArgs e)
        {
            if (base.Text != "")
            {
                int selectStart = base.SelectionStart;
                int line = base.GetLineFromCharIndex(selectStart);

                string lineStr = base.Lines[line];
                int linestart = 0;
                for (int i = 0; i < line; i++)
                {
                    linestart += base.Lines.Length + 1;
                }

                SendMessage(base.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

                base.SelectionStart = linestart;
                base.SelectionLength = lineStr.Length;
                base.SelectionColor = Color.Black;
                base.SelectionStart = selectStart;
                base.SelectionLength = 0;

                SendMessage(base.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                base.Refresh();
            }
            base.OnTextChanged(e);
        }

   }
    
}
