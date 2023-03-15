<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Editpage.aspx.cs" Inherits="Editpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <style>
        .page-center {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-center">
            <h2 style="margin-bottom: 63px"> Edit Record page</h2>
             </div>
        <div class="page-center">
         <div>
            <asp:Label Text ="First Name" runat ="server" ></asp:Label>
            <asp:TextBox ID="First_NameId" runat="server" style="margin-left: 29px;"></asp:TextBox>
            
            <asp:Label runat="server" style="margin-left: 30px" Text="Status"></asp:Label>
            <asp:TextBox ID="StatusId" runat="server" style="margin-left: 23px; margin-top: 0px; margin-bottom: 0px;"></asp:TextBox>

            

        </div>
        <div style="margin-top: 30px">
            <asp:Label Text="Last Name" runat="Server"> </asp:Label>
            <asp:TextBox ID="Last_NameId" runat="server" style="margin-left: 12px; margin-top: 0px; margin-bottom: 0px;"></asp:TextBox>


            

            <asp:Label runat="server" style="margin-left: 30px" Text="Phone"></asp:Label>
            <asp:TextBox ID="PhoneId" runat="server" style="margin-left: 25px; margin-top: 0px; margin-bottom: 0px;"></asp:TextBox>



            

        </div>
        <div style="margin-top: 30px">
            <asp:Label Text="Company" runat="server"></asp:Label>
            <asp:TextBox ID="CompanyId" runat="server" style="margin-left: 30px"></asp:TextBox>

            <asp:Label runat="server" style="margin-left: 30px" Text="Email"></asp:Label>
            <asp:TextBox ID="EmailId" runat="server" style="margin-left: 30px; margin-top: 0px; margin-bottom: 0px;"></asp:TextBox>


        </div>
        <p>
            <asp:Button ID="SaveId" runat="server" Height="26px" style="margin-left: 64px" Text="Save" Width="108px" BackColor="#CCCCCC" BorderWidth="1px" OnClick="Save_Click" />
        </p>
    </div>
    </form>
</body>
</html>
