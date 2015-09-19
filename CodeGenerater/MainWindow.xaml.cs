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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private EditPage controllorEdit;
        private EditPage serviceEdit;
        private EditPage serviceImplEdit;
        private EditPage enumEdit;
        private EditPage requestEdit;
        private EditPage responseEdit;
        private EditPage htmlEdit;
        private List<EditPage> editPageList;

        public MainWindow()
        {
            InitializeComponent();

            textBoxSource.Focus();

            controllorEdit = new EditPage();
            controllorEdit.EditName = "Controllor";
            controllorEdit.FilePath = "org/ls/bekind/client/controllor/";
            controllorEdit.TempletePath = "Templete/ControllorTemplete.txt";
            controllorEdit.Load();
            serviceEdit = new EditPage();
            serviceEdit.EditName = "Service";
            serviceEdit.FilePath = "org/ls/bekind/client/service/";
            serviceEdit.TempletePath = "Templete/ServiceTemplete.txt";
            serviceEdit.Load();
            serviceImplEdit = new EditPage();
            serviceImplEdit.EditName = "ServiceImpl";
            serviceImplEdit.FilePath = "org/ls/bekind/client/service/impl/";
            serviceImplEdit.TempletePath = "Templete/ServiceImplTemplete.txt";
            serviceImplEdit.Load();
            enumEdit = new EditPage();
            enumEdit.EditName = "Enum";
            enumEdit.FilePath = "org/ls/bekind/client/enumeration/";
            enumEdit.TempletePath = "Templete/EnumTemplete.txt";
            enumEdit.Load();
            requestEdit = new EditPage();
            requestEdit.EditName = "Request";
            requestEdit.FilePath = "org/ls/bekind/client/bean";
            requestEdit.TempletePath = "Templete/RequestTemplete.txt";
            requestEdit.Load();
            responseEdit = new EditPage();
            responseEdit.EditName = "Response";
            responseEdit.FilePath = "org/ls/bekind/client/bean";
            responseEdit.TempletePath = "Templete/ResponseTemplete.txt";
            responseEdit.Load();
            htmlEdit = new EditPage();
            htmlEdit.EditName = "Html";
            htmlEdit.FilePath = "WebContent/WEB-INF/page/interface_test";
            htmlEdit.TempletePath = "Templete/HtmlTemplete.txt";
            htmlEdit.Load();
            layoutContent.Content = controllorEdit;

            editPageList = new List<EditPage>();
            editPageList.Add(controllorEdit);
            editPageList.Add(serviceEdit);
            editPageList.Add(serviceImplEdit);
            editPageList.Add(enumEdit);
            editPageList.Add(requestEdit);
            editPageList.Add(responseEdit);
            editPageList.Add(htmlEdit);

            listBoxItems.Items.Add("Controllor");
            listBoxItems.Items.Add("Service");
            listBoxItems.Items.Add("ServiceImpl");
            listBoxItems.Items.Add("Enum");
            listBoxItems.Items.Add("Request");
            listBoxItems.Items.Add("Response");
            listBoxItems.Items.Add("Html");
            listBoxItems.SelectedIndex = 0;
        }
        
        private void listBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String str = listBoxItems.SelectedItem.ToString();
            foreach (EditPage ep in editPageList)
            {
                if (ep.EditName.Equals(str))
                {
                    layoutContent.Content = ep;
                    break;
                }
            }
        }
        
        private void buttonGen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> interfaceName = new List<string>();
                //interfaceName.Add("GetFriendList");
                //interfaceName.Add("GetMessageList");
                //interfaceName.Add("GetHomeworkTodayFriendList");
                //interfaceName.Add("GetMyHomeworkList");
                //interfaceName.Add("GetHomeworkFriendList");
                //interfaceName.Add("GetFriend");
                //interfaceName.Add("GetCreditTopNAndMyRank");
                //interfaceName.Add("PutUser");
                //interfaceName.Add("PutCertification");
                //interfaceName.Add("SyncCredit");
                //interfaceName.Add("SyncCreditRecord");
                //interfaceName.Add("SyncHomeworkCount");
                //interfaceName.Add("AttentionUser");
                //interfaceName.Add("CancelAttentionUser");
                //interfaceName.Add("PutUserHeadImg");
                //interfaceName.Add("PutUserName");
                //interfaceName.Add("PutUserNickname");
                //interfaceName.Add("PutUserSex");
                //interfaceName.Add("PutUserBirthday");
                //interfaceName.Add("PutUserRegion");
                interfaceName.Add("AddHomework");
                foreach (string name in interfaceName)
                {
                    foreach (EditPage ep in editPageList)
                    {
                        ep.Replace(name);
                    }
                    DirectoryInfo rootDi = Directory.CreateDirectory("GenCode");
                    foreach (EditPage ep in editPageList)
                    {
                        DirectoryInfo di = Directory.CreateDirectory(rootDi.Name + "/" + ep.FilePath);
                        FileStream fs = File.Create(di.FullName + "/" + ep.FileName + ".java");
                        byte[] data = Encoding.UTF8.GetBytes(ep.DestContent);
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("生成失败");
            }
            MessageBox.Show("生成成功");
        }

        private void textBoxSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (EditPage ep in editPageList)
            {
                ep.Replace(textBoxSource.Text);
            }
        }
    }
}
