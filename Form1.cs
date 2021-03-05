using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace BaiduTranslateAPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            TranslateAPI translateAPI = new TranslateAPI();
            Root root = await translateAPI.GetTranslateAsync(textBox1.Text);

            textBox1.Clear();
            for (int i = 0; i < root.trans_result.Count; i++)
            {
                textBox1.AppendText(root.trans_result[i].src + "\r\n");
                textBox2.AppendText(root.trans_result[i].dst + "\r\n");
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            TranslateAPI translateAPI = new TranslateAPI();
            Root root = await translateAPI.GetTranslateAsync(textBox1.Text, "zh", "en");

            textBox1.Clear();
            for (int i = 0; i < root.trans_result.Count; i++)
            {
                textBox1.AppendText(root.trans_result[i].src + "\r\n");
                textBox2.AppendText(root.trans_result[i].dst + "\r\n");
            }
        }
    }

    public class Trans_resultItem
    {
        /// <summary>
        /// origin string
        /// </summary>
        public string src { get; set; }
        /// <summary>
        /// after translate
        /// </summary>
        public string dst { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string @from { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string to { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Trans_resultItem> trans_result { get; set; }
    }

    public class TranslateAPI
    {
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

        /// <summary>
        /// 使用百度的翻译接口
        /// </summary>
        /// <param name="inputString">输入待翻译的字符串</param>
        /// <param name="srcLang">输入字符串的语言</param>
        /// <param name="dstLang">目标字符串语言</param>
        /// <returns>翻译后返回的json转成的实体类</returns>
        public async Task<Root> GetTranslateAsync(string inputString, string srcLang = "en", string dstLang = "zh")
        {
            // 原文
            string q = inputString.Replace("\r\n", " "); //去除掉换行符，用空格代替
            // 源语言
            string from = srcLang;
            // 目标语言
            string to = dstLang;
            // 改成您的APP ID
            string appId = "20210122000678251";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "3rARl9A_BlZY82qf9Wop";
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            Root root = JsonConvert.DeserializeObject<Root>(retString);
            return root;
        }
    }
}
