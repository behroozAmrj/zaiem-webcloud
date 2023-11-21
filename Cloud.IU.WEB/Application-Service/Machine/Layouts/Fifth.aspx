<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fourth.aspx.cs"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="jquery-1.10.2.js"></script>
<script>
$(document).ready(function(){

	$("#showhello").click(function(){alert('this is fired from jquery function');});	

});
</script>
</head>
<body style="background-color:white">
    <form id="form1" runat="server">
    <div>
           Hi Behrooz this is 55555555555555555555555555555555555 page
		<input id="showhello" type="button" value="press Me to Show hello" />		
    </div>
    </form>
</body>
</html>
