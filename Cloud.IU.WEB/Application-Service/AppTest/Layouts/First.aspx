<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html>
<script runat="server">

    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://wwww.google.com");
    }

    protected void secondbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("second.aspx");
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body{background-color:white;}
        textarea {
            background-color:black;
            color:white;
            width:500px;
            height:500px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <div>
                <textarea></textarea>

            </div>
            <asp:Button ID="secondbtn" runat="server" Text="Go To second Page" OnClick="secondbtn_Click" />
    </div>
    </form>
</body>
</html>
