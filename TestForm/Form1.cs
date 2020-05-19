using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CallWebAPI_Click(object sender, EventArgs e)
        {
            using (WebClient webClient = new WebClient())
            // 從 url 讀取資訊至 stream
            using (Stream stream = webClient.OpenRead("http://localhost:52006/api/index/1"))
            // 使用 StreamReader 讀取 stream 內的字元
            using (StreamReader reader = new StreamReader(stream))
            {
                // 將 StreamReader 所讀到的字元轉為 string
                string request = reader.ReadToEnd();
                MessageBox.Show(request);
            }
        }
    }
}
