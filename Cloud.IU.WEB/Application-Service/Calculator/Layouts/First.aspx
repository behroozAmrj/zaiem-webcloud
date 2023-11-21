<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html>
<script runat="server">

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://wwww.google.com");
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Button  ID="btnRedirect" runat="server" Text="RedirectToPage:!" OnClick="btnRedirect_Click"/>
    </div>
    </form>
</body>
</html>
