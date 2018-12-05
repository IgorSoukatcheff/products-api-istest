using Newtonsoft.Json;
using products_api_istest.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentUploader
{
    public partial class Form1 : Form
    {
        private string  _filepath;
        private DocumentDto dto = new DocumentDto();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                _filepath = openFileDialog1.FileName;
                label1.Text = Path.GetFileName(_filepath);

               
                dto = new DocumentDto();
                var byteArray = File.ReadAllBytes(_filepath);
                var fi = new FileInfo(_filepath);
               
                
                Shell32.Shell shell = new Shell32.Shell();
                Shell32.Folder objFolder;
                var arrHeaders = new List<string>();
                var attribList = new List<KeyValuePair<string, string>>();
                objFolder = shell.NameSpace(fi.DirectoryName);

                for (int i = 0; i < short.MaxValue; i++)
                {
                    string header = objFolder.GetDetailsOf(null, i);
                    if (String.IsNullOrEmpty(header))
                        break;
                    arrHeaders.Add(header);
                }

                foreach (Shell32.FolderItem2 item in objFolder.Items())
                {
                    if (!item.Name.Contains(fi.Name))
                    {
                        continue;
                    }
                    int i = 0;
                    foreach(var attrib in arrHeaders)
                    {
                       
                        var attribVal = objFolder.GetDetailsOf(item, i);
                        attribList.Add(new KeyValuePair<string, string>(attrib, attribVal));
                        i++;
                    }
                }               

                dto.DocumentContent = Encoding.UTF8.GetBytes(Convert.ToBase64String(File.ReadAllBytes(_filepath)));
                dto.DocumentContentLenght = dto.DocumentContent.Length;
                dto.DocumentName = label1.Text;
                dto.FileName = label1.Text;
                dto.DocumentMetadata = JsonConvert.SerializeObject(attribList);
                dto.FileSize = fi.Length; 
                dto.CreationDate = fi.CreationTime; ;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dto.DocumentContent.Length > 0)
            {
                using (var client = new HttpClient())
                {
#if DEBUG
                    client.BaseAddress = new Uri("http://localhost:50929");
#else
                    client.BaseAddress = new Uri("https://products-api-istest.azurewebsites.net");
#endif
                    var url = "/api/Documents";
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                    var result = client.PostAsync(url, content).Result;
                    toolStripStatusLabel2.Text = result.StatusCode.ToString();
                }
            }
        }
    }
}
