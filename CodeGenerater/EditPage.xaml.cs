using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeGenerater
{
    /// <summary>
    /// EditPage.xaml 的交互逻辑
    /// </summary>
    public partial class EditPage : Page
    {
        private string editName;

        private string fileName;
        private string filePath;

        private string templetePath;
        private string templeteContent;
        private string destContent;

        public string EditName
        {
            get
            {
                return editName;
            }

            set
            {
                editName = value;
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
            }
        }

        public string TempletePath
        {
            get
            {
                return templetePath;
            }

            set
            {
                templetePath = value;
            }
        }

        public string TempleteContent
        {
            get
            {
                return templeteContent;
            }

            set
            {
                templeteContent = value;
            }
        }

        public string DestContent
        {
            get
            {
                return destContent;
            }

            set
            {
                destContent = value;
            }
        }

        public EditPage()
        {
            InitializeComponent();
        }

        public void Load()
        {
            try
            {
                FileStream fs = new FileStream(TempletePath, FileMode.Open);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                TempleteContent = Encoding.UTF8.GetString(data);
                textBoxSrc.Text = TempleteContent;
                Replace("");
            }
            catch (Exception ex)
            {
                MessageBox.Show("模板读取失败\n" + ex.ToString());
                throw;
            }
        }

        public void Replace(string str)
        {
            ChangeNameFromLowerCase(str);
            GenCode();
        }

        private void GenCode()
        {
            StringBuilder sb = new StringBuilder(TempleteContent);
            sb.Replace(textBoxSrc1.Text, textBoxDest1.Text);
            sb.Replace(textBoxSrc2.Text, textBoxDest2.Text);
            sb.Replace(textBoxSrc3.Text, textBoxDest3.Text);
            DestContent = sb.ToString();
            textBoxDest.Text = sb.ToString();
        }

        private void ChangeNameFromLowerCase(string str)
        {
            if (str == null || "".Equals(str))
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            int begin = 0;
            int end = 0;
            string temp = str;
            while (true)
            {
                end = str.IndexOf("_", begin);
                if (end == -1)
                {
                    end = str.Length;
                }
                temp = str.Substring(begin, end - begin);
                begin = end + 1;
                sb.Append(temp.Substring(0, 1).ToUpper());
                if (temp.Length > 1)
                {
                    sb.Append(temp.Substring(1));
                }
                if (end == str.Length)
                {
                    break;
                }
            }
            ChangeNameFromCamelCase(sb.ToString());
        }

        private void ChangeNameFromCamelCase(string str)
        {
            if (str.Length == 0)
            {
                textBoxDest1.Text = "";
                textBoxDest2.Text = "";
                textBoxDest3.Text = "";
                fileName = str + EditName;
                return;
            }
            textBoxDest1.Text = str;

            string str2 = str.Substring(0, 1).ToLower() + str.Substring(1);
            textBoxDest2.Text = str2;
            
            textBoxDest3.Text = ToLowerCase(str);

            FileName = str + EditName;
        }

        public string ToLowerCase(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < sb.Length; i++)
            {
                if (Char.IsUpper(sb[i]))
                {
                    sb[i] = Char.ToLower(sb[i]);
                    if (i != 0)
                    {
                        sb.Insert(i, "_");
                    }
                }
            }
            return sb.ToString();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
