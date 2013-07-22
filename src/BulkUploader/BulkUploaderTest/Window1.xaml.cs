using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace BulkUploaderTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(OnLoaded);
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            
            //OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Multiselect = true;
            //if (dlg.ShowDialog() == true)
            //{
            //    if (MessageBox.Show("Are you sure you want to upload the selected files?", "Confirm Upload", MessageBoxButton.OK) == MessageBoxResult.OK)
            //    {

                    string cmd = "test";
                    string msg = "test";

                    VESHTER.HttpHelper helper = new VESHTER.HttpHelper(new Uri("https://secure.veshter.com/static/bulkupload/test/dummyack.php"), "POST",
            new KeyValuePair<string, string>("authKey", "key"),
            new KeyValuePair<string, string>("cmd", cmd.ToString()),
            new KeyValuePair<string, string>("msg", msg));
                    helper.ResponseComplete += new VESHTER.HttpResponseCompleteEventHandler(this.CommandComplete);
                    helper.Execute();



                    //foreach (System.IO.FileInfo info in dlg.Files)
                    //{
                    //    post.Post();
                    //}

            //    }
            //}



        }

        private void CommandComplete(VESHTER.HttpResponseCompleteEventArgs e)
        {
            MessageBox.Show(e.Response);
        }
    }
}
