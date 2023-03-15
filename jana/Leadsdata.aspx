<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeFile="Leadsdata.aspx.cs" Inherits="Leadsdata" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 245px">
    <form id="form1" runat="server">
       
        <div>
            <asp:Button ID="Button2" runat="server" style="margin-left: 58px; font-weight: bold" Text="Refresh Data" Width="94px" BackColor="#cccccc" BorderWidth="1px" OnClick="Button2_ClickAsync" />
        </div>
        <div>
            <asp:GridView ID="gridView" runat="server" AutoGenerateColumns="False"  Width="870px" Height="500px" style="margin-left: 164px" OnRowCommand="gridView_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Edit Row">
                    <ItemTemplate>
                        <asp:HiddenField ID="hiddenId" runat="server" Value='<%# Eval("Id") %>' />
                        <asp:Button ID="Button2" style="border: none;background-color: #fff;font-weight:bold;" runat="server"  Text="Edit" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="First_Name" HeaderText="First_Name" />
                <asp:BoundField DataField="Last_Name" HeaderText="Last_Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Company" HeaderText="Company" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                
            </Columns>
            </asp:GridView>
        </div>
        <div>
            <asp:Button ID="btnPrev" runat="server" Text="Previous" OnClick="btnPrev_Click" BackColor="Silver" Height="23px" style="margin-left: 400px; font-weight: bold; margin-top: 8px" Width="71px" />
            <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" BackColor="Silver" Height="23px" style="margin-left: 200px; font-weight: bold" Width="71px" />
        </div>
         <div>
             <asp:Label ID="MessageLabel" style="font-size: 40px; color:crimson; text-align:center;margin-left:60px" runat="server" Visible="false"></asp:Label>

         </div>
    </form>
       
</body>
</html>
