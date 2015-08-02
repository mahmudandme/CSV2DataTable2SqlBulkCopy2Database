<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="CSVToDatabase2.TestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Enter the file name : "></asp:Label></td>
            <td>
                <asp:TextBox ID="mainFileTextBox" runat="server"></asp:TextBox></td>
            <td>
                <asp:Button ID="importMainFileButton" runat="server" Width="150px" Text="Import main file" OnClick="importMainFileButton_Click" /></td>
            <td>
                <asp:Label ID="mainFileStatusLabel" runat="server" Text=""></asp:Label></td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
