<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CSVToDatabase2.Home" %>

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
                <asp:Button ID="importMainFileButton" runat="server" Width="150px" Text="Import main file" OnClick="importMainFileButton_Click"  /></td>
            <td>
                <asp:Label ID="mainFileStatusLabel" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:TextBox ID="addFileTextBox" runat="server"></asp:TextBox></td>
            <td>
                <asp:Button ID="importAddFileButton" runat="server" Text="Import add file" Width="150px" OnClick="importAddFileButton_Click" style="height: 26px"  /></td> 
             <td>
                <asp:Label ID="addFileStatusLabel" runat="server" Text=""></asp:Label></td>          
        </tr>
        <tr>
             <td></td>
            <td>
                <asp:TextBox ID="deductFileTextBox" runat="server"></asp:TextBox></td>
            <td>
                <asp:Button ID="importDeductFileButton" runat="server" Width="150px" Text="Import deduct file" OnClick="importDeductFileButton_Click"  /></td> 
             <td>
                <asp:Label ID="deductFileStatusLabel" runat="server" Text=""></asp:Label></td>          
        </tr>
    </table>

    </div>
        <div>
            <asp:GridView ID="mainFileCSVDataGridView" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>
