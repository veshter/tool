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

using VESHTER.TokensWS;
using VESHTER.SiteDetailsWS;
using VESHTER.SupportTicketsWS;
using VESHTER.ScriptsWS;

namespace WsdlTest
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void GoSoaping(object sender, RoutedEventArgs e)
        {
            try
            {

                string resource = "191B855E819465FA49A5156EC8722DBB";
                string externalToken = "b56d5dd628cba2e6458c2ae498f24a1c41b97949";
                string internalToken = "EC208A86D6DF5564F608F2448B6C2E98";
                
#if TOKENTEST
//#else
                TokensClient tokenClient = new TokensClient();

                internalToken = tokenClient.GenerateToken(resource, externalToken);

                System.Diagnostics.Trace.Assert(tokenClient.ValidateToken(resource, internalToken));
                
                //System.Diagnostics.Trace.Assert(tokenClient.InvalidateToken(internalToken));
                
#endif

#if SITEDETAILSTEST
//#else
                SiteDetailsClient detailsClient = new SiteDetailsClient();
                float balance = detailsClient.GetCurrentBalance(resource, internalToken);//, out time);
                System.Diagnostics.Trace.Assert(balance != 0);


                Service[] services = detailsClient.GetCurrentContract(resource, internalToken);
                System.Diagnostics.Trace.Assert(services.Length > 0);
#endif

#if TICKETSTEST
//#else
                SupportTicketsClient supportClient = new SupportTicketsClient();


                SupportTicket details = new SupportTicket();
                details.name = "Test ticket for " + DateTime.Now.ToString();
                details.priority = 4;
                details.notes = new SupportTicketNote[2];

                SupportTicketNote note = new SupportTicketNote();
                note.about = "My site needs  new design";
                note.author = "test user";

                details.notes[0] = note;
                details.notes[1] = note;
                

                //details.notes[1] = "My site needs  new design and more";
                
                string ticket = supportClient.OpenTicket(resource, internalToken, details);
                System.Diagnostics.Trace.Assert(!string.IsNullOrEmpty(ticket));

                note.about = "Ticket was closed because it is a test ticket";
                bool isClosed = supportClient.CloseTicket(resource, internalToken, ticket, note);
                System.Diagnostics.Trace.Assert(isClosed);

                note.about = "Ticket was reopened";
                bool isReopened = supportClient.ReopenTicket(resource, internalToken, ticket, note);
                System.Diagnostics.Trace.Assert(isReopened);

                note.about = "Note " + Guid.NewGuid().ToString();
                bool isNoteAdded = supportClient.AddTicketNote(resource, internalToken, ticket, note);
                System.Diagnostics.Trace.Assert(isNoteAdded);

                SupportTicket[] allTickets = supportClient.GetAllTickets(resource, internalToken);
                System.Diagnostics.Trace.Assert(allTickets.Length > 0);

                SupportTicket[] allOpenTickets = supportClient.GetAllOpenTickets(resource, internalToken);
                System.Diagnostics.Trace.Assert(allOpenTickets.Length > 0);

                details = supportClient.GetTicketDetails(resource, internalToken, ticket);
                System.Diagnostics.Trace.Assert(details != null);


                
#endif

#if SCRIPTSSTEST
#else
                ScriptsClient scriptsClient = new ScriptsClient();

                Script[] scripts = scriptsClient.GetLatestScripts(resource, internalToken);

                string folder = "base";
                string filename = "class.environment.php";
                Script script = scriptsClient.GetScriptDetails(resource, internalToken, folder, filename);




#endif

                MessageBox.Show("Test successful");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Web service call failed: " + ex.Message);
            }
        }
    }
}
