using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BulkUploader
{
    public partial class Page : UserControl
    {
        private IDictionary<string, string> m_params = null;

        public Page(IDictionary<string, string> parameters)
        {
            InitializeComponent();

            m_params = parameters;

        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                if (MessageBox.Show("Are you sure you want to upload the selected files?", "Confirm Upload", MessageBoxButton.OK) == MessageBoxResult.OK)
                {

                    //string cmd = "test";
                    //string msg = "test";

                    //string url = "https://secure.veshter.com/static/bulkupload/test/dummyack.php";
                    string url = "http://localhost:7777/BulkUploaderTestPage.html";

                    VESHTER.HttpHelper helper = new VESHTER.HttpHelper(url);//, "POST",
            //new KeyValuePair<string, string>("authKey", "key"),
            //new KeyValuePair<string, string>("cmd", cmd.ToString()),
            //new KeyValuePair<string, string>("msg", msg));
                    //helper.ResponseComplete += new VESHTER.HttpResponseCompleteEventHandler(this.CommandComplete);
                    //helper.Execute();



                    //foreach (System.IO.FileInfo info in dlg.Files)
                    //{
                    //    post.Post();
                    //}

                }
            }



        }

        //private void CommandComplete(VESHTER.HttpResponseCompleteEventArgs e)
        //{
        //    MessageBox.Show(e.Response);
        //}

    }
}
