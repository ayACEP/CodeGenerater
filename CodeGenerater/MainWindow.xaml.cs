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
        private List<EditPage> editPageList = new List<EditPage>();

        private static string BASE_PATH = "org/ls/synctest/";

        public MainWindow()
        {
            InitializeComponent();

            textBoxSource.Focus();
            try
            {
                StreamReader sr = new StreamReader("templete_list.txt", Encoding.UTF8);
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    if (line.Equals(""))
                    {
                        continue;
                    }
                    string[] param = line.Split(",".ToArray());
                    if (param.Length != 4)
                    {
                        continue;
                    }

                    EditPage ep = new EditPage();
                    ep.EditName = param[0].Trim();
                    ep.FilePath = param[1].Trim();
                    ep.ExtName = param[2].Trim();
                    ep.FileNameCase = param[3].Trim();
                    ep.TempletePath = "Templete/" + ep.EditName + "Templete.txt";

                    ep.Load();
                    editPageList.Add(ep);
                    listBoxItems.Items.Add(ep.EditName);
                }
                sr.Close();
                layoutContent.Content = editPageList.Count > 0 ? editPageList[0] : null;
                listBoxItems.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }

            //controllorEdit = new EditPage();
            //controllorEdit.EditName = "Controllor";
            //controllorEdit.FilePath = BASE_PATH/* + "controllor/"*/;
            //controllorEdit.TempletePath = "Templete/ControllorTemplete.txt";
            //controllorEdit.Load();
            //serviceEdit = new EditPage();
            //serviceEdit.EditName = "Service";
            //serviceEdit.FilePath = BASE_PATH/* + "service/"*/;
            //serviceEdit.TempletePath = "Templete/ServiceTemplete.txt";
            //serviceEdit.Load();
            //serviceImplEdit = new EditPage();
            //serviceImplEdit.EditName = "ServiceImpl";
            //serviceImplEdit.FilePath = BASE_PATH/* + "service/impl/"*/;
            //serviceImplEdit.TempletePath = "Templete/ServiceImplTemplete.txt";
            //serviceImplEdit.Load();
            //enumEdit = new EditPage();
            //enumEdit.EditName = "Enum";
            //enumEdit.FilePath = BASE_PATH/* + "enumeration/"*/;
            //enumEdit.TempletePath = "Templete/EnumTemplete.txt";
            //enumEdit.Load();
            //requestEdit = new EditPage();
            //requestEdit.EditName = "Request";
            //requestEdit.FilePath = BASE_PATH/* + "bean/"*/;
            //requestEdit.TempletePath = "Templete/RequestTemplete.txt";
            //requestEdit.Load();
            //responseEdit = new EditPage();
            //responseEdit.EditName = "Response";
            //responseEdit.FilePath = BASE_PATH/* + "bean/"*/;
            //responseEdit.TempletePath = "Templete/ResponseTemplete.txt";
            //responseEdit.Load();
            //htmlEdit = new EditPage();
            //htmlEdit.EditName = "Html";
            //htmlEdit.FilePath = "WebContent/WEB-INF/page/interface_test";
            //htmlEdit.TempletePath = "Templete/HtmlTemplete.txt";
            //htmlEdit.Load();


            
            //editPageList.Add(controllorEdit);
            //editPageList.Add(serviceEdit);
            //editPageList.Add(serviceImplEdit);
            //editPageList.Add(enumEdit);
            //editPageList.Add(requestEdit);
            //editPageList.Add(responseEdit);
            //editPageList.Add(htmlEdit);

            //listBoxItems.Items.Add("Controllor");
            //listBoxItems.Items.Add("Service");
            //listBoxItems.Items.Add("ServiceImpl");
            //listBoxItems.Items.Add("Enum");
            //listBoxItems.Items.Add("Request");
            //listBoxItems.Items.Add("Response");
            //listBoxItems.Items.Add("Html");
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
            StreamReader listSr = null;
            List<string> interfaceName = new List<string>();
            try
            {
                listSr = new StreamReader("list.txt", Encoding.UTF8);
                while (true)
                {
                    string line = listSr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    if (line.Equals(""))
                    {
                        continue;
                    }
                    interfaceName.Add(line);
                }
                listSr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败\n" + ex.ToString());
                return;
            }
            finally
            {
                if (listSr != null)
                {
                    listSr.Close();
                }
            }



            try
            {
                DirectoryInfo rootDi = Directory.CreateDirectory("GenCode");
                foreach (string name in interfaceName)
                {
                    foreach (EditPage ep in editPageList)
                    {
                        try
                        {
                            ep.Replace(name);
                            DirectoryInfo di;
                            if (ep.FileNameCase.Equals("lower"))
                            {
                                di = Directory.CreateDirectory(rootDi.Name + "/" + ep.FilePath);
                            }
                            else
                            {
                                di = Directory.CreateDirectory(rootDi.Name + "/" + ep.FilePath + "/" + ep.FromCamelToLowerCase(name));
                            }
                            FileStream fs = File.Create(di.FullName + "/" + ep.FileName + "." + ep.ExtName);
                            byte[] data = Encoding.UTF8.GetBytes(ep.DestContent);
                            fs.Write(data, 0, data.Length);
                            fs.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(name + "生成失败\n" + ex.ToString());
                        }
                    }
                }
                MessageBox.Show("生成成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败\n" + ex.ToString());
            }
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
