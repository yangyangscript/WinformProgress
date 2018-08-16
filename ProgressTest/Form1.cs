using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressTest
{
    public partial class Form1 : Form
    {

        
        private class ProgressClass
        {
            /// <summary>
            /// 循环次数
            /// </summary>
            public int counts { get; set; } = 10000;
            /// <summary>
            /// 每次循环做的事
            /// </summary>
            public Action doProgress;
            /// <summary>
            /// 循环结束之后做的事
            /// </summary>
            public Action CompleteProgress;
            /// <summary>
            /// 循环更新进度条
            /// </summary>
            public void Progress()
            {
                for (int i = 0; i < counts; i++)
                {
                    //todo:
                    doProgress();
                }

                CompleteProgress();
            }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private Action<int> otherInvoke;
        private void button1_Click(object sender, EventArgs e)
        {           
            this.progressBar1.Value=0;
            var newClass = new ProgressClass();
            newClass.doProgress += Progress;
            newClass.CompleteProgress += Complete;
            this.progressBar1.Maximum = newClass.counts;
            this.button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(newClass.Progress));
            thread.IsBackground = true;
            thread.Start();
        }


        private void Progress()
        {
            //更新进度条value和label的显示
            Action<int> otherThread = s =>
            {
                this.progressBar1.Value += s;
                this.label1.Text = $"{this.progressBar1.Value}/{this.progressBar1.Maximum}";
            };
            if (InvokeRequired)
            {
                //每次增加1
                this.Invoke(otherThread, 1);
            }         
        }

        private void Complete()
        {
            //完成之后做的事
            Action complete = () =>
            {
                this.button1.Enabled = true;
                this.label1.Text = "完成";
            };
            if (InvokeRequired)
            {
                this.Invoke(complete);
            }          
        }

    }
}
