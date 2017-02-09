<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="GoogleCloudAspNet.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Hello,&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; World!
        <br />
        <br />
        <asp:TextBox ID="TextBox1" runat="server" Width="735px" Height="17px"></asp:TextBox>
        <br />
        <br />
    
    </div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Write Log" />
        <br />
        <p>
            <asp:TextBox ID="TextBox2" runat="server" Height="257px" TextMode="MultiLine" Width="617px"></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Batch Write 20 log entries" />
        </p>
    </form>
</body>
</html>
