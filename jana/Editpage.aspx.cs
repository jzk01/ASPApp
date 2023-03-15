using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MySql.Data.MySqlClient;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Protobuf.WellKnownTypes;
using System.Xml.Linq;
using System.IdentityModel.Protocols.WSTrust;
using System.Numerics;
using MySqlX.XDevAPI;
using System.Activities.Expressions;

public partial class Editpage : System.Web.UI.Page
{
    string connectionString = "server=127.0.0.1; user=root; database=leads; password=";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Lead lead = (Lead)Session["SelectedLead"];

            if (!string.IsNullOrEmpty(lead.First_Name))
            {
                    First_NameId.Text = lead.First_Name;
                    Last_NameId.Text = lead.Last_Name;
                    EmailId.Text = lead.Email;
                    CompanyId.Text = lead.Company;
                    StatusId.Text = lead.Status;
                    PhoneId.Text = lead.Phone;
                }
            
        }

    }

    protected void Save_Click(object sender, EventArgs e)
    {
        Lead lead = (Lead)Session["SelectedLead"];

        string First_Name = First_NameId.Text;
        string Last_Name = Last_NameId.Text;
        string Status = StatusId.Text;
        string Email = EmailId.Text;
        string Phone = PhoneId.Text;
        string Company = CompanyId.Text;
        System.Diagnostics.Debug.WriteLine(First_Name + " " + Last_Name+ " " + Status + " " + Email + " " + Phone + " " + Company);
        UpdateLeadInDatabase(lead.Id, First_Name, Last_Name, Email, Company, Status, Phone);

        // Redirect to login page
        string message = "Lead updated successfully.";
        Response.Redirect("Leadsdata.aspx?message=" + Server.UrlEncode(message));




    }
    public void UpdateLeadInDatabase(string id, string first_name, string last_name, string email, string company, string status, string phone)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            string query = "UPDATE leads SET First_Name=@first_name, Last_Name=@last_name, Email=@email, Company=@company, Status=@status, Phone=@phone WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@first_name", first_name);
                command.Parameters.AddWithValue("@last_name", last_name);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@company", company);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@phone", phone);     
                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}