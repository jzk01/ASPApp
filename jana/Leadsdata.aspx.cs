using System.Net.Http;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.IO;
using static Leadsdata;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.UI.WebControls;
using System.Drawing.Printing;
using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using System.Web;

public partial class Leadsdata : System.Web.UI.Page
{
    string username = "taleenbaydountest";
    string password = "password";
    string endpoint = "beta.cirrus-shield.net/RestApi";
    string token = "";
    string action = "upsert";
    string matchingFieldName = "id";
    string useExternalId = "false";
    string connectionString = "server=127.0.0.1; user=root; database=leads; password=";
    string file = "C:\\Users\\Asus\\Desktop\\jana\\getData2.txt";
    protected async void Page_Load(object sender, EventArgs e)
    {
        List<Lead> leads = new List<Lead>();

        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            using (connection)
            {
                await connection.OpenAsync();

                foreach (Lead lead in leads)
                {
                    string query = "INSERT INTO leads (Id,First_Name,Last_Name, Email, Company, Status, Phone) VALUES (@Id, @First_Name, @Last_Name, @Email, @Company, @Status, @Phone)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", lead.Id);
                        cmd.Parameters.AddWithValue("@First_Name", lead.First_Name);
                        cmd.Parameters.AddWithValue("@Last_Name", lead.Last_Name);
                        cmd.Parameters.AddWithValue("@Email", lead.Email);
                        cmd.Parameters.AddWithValue("@Company", lead.Company);
                        cmd.Parameters.AddWithValue("@Status", lead.Status);
                        cmd.Parameters.AddWithValue("@Phone", lead.Phone);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

            }

        }
        catch (Exception ex)
        { 
            System.Diagnostics.Debug.WriteLine("Error:", ex.ToString());


        }
        finally { connection.Close(); }
        if (!IsPostBack)
        {
            int pageNumber = 1;
            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out pageNumber);
            }

            DisplayLeads(pageNumber);
            if (!string.IsNullOrEmpty(Request.QueryString["message"]))
    {
        string message = Request.QueryString["message"];
        MessageLabel.Text = message;
        MessageLabel.Visible = true;
    }
        }
    }


     public  List<Lead> getInfoSystem() {
       string xmlData = File.ReadAllText(file);
        List<Lead> leads = new List<Lead>();
        XElement response = XElement.Parse(xmlData);
        foreach (XElement xe in response.Descendants("Lead"))
        {
            Lead prj = new Lead() { Id = xe.Element("Id").Value, First_Name = xe.Element("First_Name").Value, Last_Name = xe.Element("Last_Name").Value, Email = xe.Element("Email").Value, Company = xe.Element("Company").Value, Status = xe.Element("Status").Value, Phone = xe.Element("Phone").Value };
            leads.Add(prj);
        }
        return leads.OrderBy(o => o.First_Name).ToList();




    }
    public async Task<string> getToken()
    {
        HttpClient client = new HttpClient();
        string url = "https://" + endpoint + "/AuthToken?Username=" + username + "&password=" + password;
        token = await client.GetStringAsync(url);
        token = token.Replace("\"", "");
        return token;
    }

    public async Task<List<Lead>> getData()
    {
        string token = await getToken();
        string completeUrl = "https://" + endpoint + "/Query?authToken=" + token + "&selectQuery=" + "select Id,First_Name,Last_Name,Email,Company,Status,Phone from Lead";
        System.Diagnostics.Debug.WriteLine(completeUrl);
        HttpClient client = new HttpClient();
        List<Lead> leads = new List<Lead>();
        var response= await client.GetStringAsync(completeUrl); //internal server error 
            XElement responseXml = XElement.Parse(response);
            foreach (XElement xe in responseXml.Descendants("Lead"))
            {
                Lead prj = new Lead() { Id = xe.Element("Id").Value, First_Name = xe.Element("First_Name").Value, Last_Name = xe.Element("Last_Name").Value, Email = xe.Element("Email").Value, Company = xe.Element("Company").Value, Status = xe.Element("Status").Value, Phone = xe.Element("Phone").Value };
                leads.Add(prj);
            }
        return leads.OrderBy(o => o.First_Name).ToList();

}

   
   
    public List<Lead> GetLeadsFromDatabase()
    {
        string localquery = "SELECT * FROM leads";
        List<Lead> localleads = new List<Lead>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            using (MySqlCommand command = new MySqlCommand(localquery, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Lead lead = new Lead()
                        {
                            Id = reader.GetString("Id"),
                            First_Name = reader.GetString("First_Name"),
                            Last_Name = reader.GetString("Last_Name"),
                            Email = reader.GetString("Email"),
                            Company = reader.GetString("Company"),
                            Status = reader.GetString("Status"),
                            Phone = reader.GetString("Phone")
                        };

                        localleads.Add(lead);
                    }
                }
            }
            return localleads;
        }
    }
    public void DisplayLeads(int pageNumber)
    {
        List<Lead> allLeads = GetLeadsFromDatabase();

        int pageSize = 10;
        int currentPage = pageNumber - 1;
        if (currentPage == 0)
        {
            btnPrev.Enabled = false;
        }
        else
        {
            btnPrev.Enabled = true;
        }
        if ((currentPage + 1) * pageSize >= allLeads.Count)
        {
            btnNext.Enabled = false;
        }
        else
        {
            btnNext.Enabled = true;
        }

        List<Lead> leads = allLeads.Skip(currentPage * pageSize).Take(pageSize).ToList();

        gridView.DataSource = leads;
        gridView.DataBind();
    }

    protected async void Button2_ClickAsync(object sender, EventArgs e)
    {
        List<Lead> refreshLeads = GetLeadsFromDatabase();
        string token = await getToken();
        string postUrl = "https://" + endpoint + "/DataAction/Lead?authToken=" + token + "&action=" + action + "&matchingFieldName=" + matchingFieldName + "&useExternalId=" + useExternalId;
        System.Diagnostics.Debug.WriteLine(postUrl);
        HttpClient client = new HttpClient();
            string xmlData = ConvertLeadToXML(refreshLeads);
            System.Diagnostics.Debug.WriteLine(xmlData);
        
            HttpContent content = new StringContent("=" + HttpUtility.UrlEncode(xmlData.ToString()), Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(postUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();
        
            
        
    }

    protected void btnPrev_Click(object sender, EventArgs e)
    {
        int pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber > 1)
        {
            pageNumber -= 1;
        }

        Response.Redirect("Leadsdata.aspx?page=" + pageNumber);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        int pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        int pageCount = (int)Math.Ceiling(GetLeadsFromDatabase().Count / 10.0);
        if (pageNumber < pageCount)
        {
            pageNumber += 1;
        }

        Response.Redirect("Leadsdata.aspx?page=" + pageNumber);
    }

    protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            List<Lead> allLeads = GetLeadsFromDatabase();
            GridViewRow row = gridView.Rows[index];
            HiddenField hiddenId = (HiddenField)row.FindControl("hiddenId");
            string id = hiddenId.Value;
            Lead lead = allLeads.FirstOrDefault(l => l.Id == id);
            System.Diagnostics.Debug.WriteLine(id);

            if (lead != null)
            {
                Session["SelectedLead"] = lead;
                Response.Redirect("EditPage.aspx");
            }
        }
    }
    public string ConvertLeadToXML(List<Lead> leads)
    {
        StringBuilder xmlBuilder = new StringBuilder("<Data>");
        foreach (Lead lead in leads.Take(10))
        {
            xmlBuilder.Append("<Lead>");
            xmlBuilder.Append($"<Id>{lead.Id}</Id>");
            xmlBuilder.Append($"<First_Name>{lead.First_Name}</First_Name>");
            xmlBuilder.Append($"<Last_Name>{lead.Last_Name}</Last_Name>");
            xmlBuilder.Append($"<Company>{lead.Company}</Company>");
            xmlBuilder.Append($"<Email>{lead.Email}</Email>");
            xmlBuilder.Append($"<Status>{lead.Status}</Status>");
            xmlBuilder.Append($"<Phone>{lead.Phone}</Phone>");
            xmlBuilder.Append("</Lead>");
        }
        xmlBuilder.Append("</Data>");
        return xmlBuilder.ToString();
    }


}