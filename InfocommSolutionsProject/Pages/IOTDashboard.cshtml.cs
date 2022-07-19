using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Net;

namespace InfocommSolutionsProject.Pages
{
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://infocommsolutionsprojectsgae.scm.azurewebsites.net/api/triggeredwebjobs/IOTSim/run");
            request.Method = "POST";
            var byteArray = Encoding.ASCII.GetBytes("$InfocommSolutionsProjectSGAE:njTfurAvwCtcJtDamfdbTL4njqczqyicBdEbN7G3lZurQDxfbjZ5kSQvx2WM"); //we could find user name and password in Azure web app publish profile 
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));
            request.ContentLength = 0;
           
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
        }
    }
}
