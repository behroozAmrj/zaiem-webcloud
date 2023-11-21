<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html>
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        string rl = string.Empty;
        if (Request.QueryString["url"] != null)
        {
            rl = Request.QueryString["url"].ToString();
            iframe.Src = rl;
        }
        
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://localhost:5146/vmmanagement/main/u2r1hz5d2qzyt30yjtbrhyaq/bfc0106814f04de6ae3e77c1149523ea");
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/design.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
    	<div class="functionbar"></div>
        <iframe src="" runat="server" id="iframe"></iframe>
    
    </form>
</body>
</html>
