<%@ Page Language="c#" Inherits="Mediachase.UI.Web.Documents.DeleteDocumentModal" CodeBehind="DeleteDocumentModal.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
	
	<title><%=LocRM.GetString("Delete")%></title>
</head>
<body style="padding-right: 7px; padding-left: 7px; padding-bottom: 7px; padding-top: 7px">
	<%=LocRM.GetString("DeleteTxt") %>
	<form id="PModal" method="post" runat="server">
	<p align="center">
		<input type="button" class="text" style="width: 100px" onclick="All();" value='<%#LocRM.GetString("DeleteAll") %>' />&nbsp;&nbsp;
		<input type="button" class="text" value='<%#LocRM.GetString("DeleteDocument") %>' style="width: 110px" onclick="Document();" />&nbsp;&nbsp;
		<input type="button" class="text" value='<%#LocRM.GetString("Cancel") %>' style="width: 100px" onclick="cancel();" />
	</p>
	</form>

	<script type="text/javascript">
		//<![CDATA[
		function cancel() {
			returnValue = 'Cancel';
			window.close();
		}

		function All() {
			returnValue = 'All';
			window.close();
		}

		function Document() {
			returnValue = 'Document';
			window.close();
		}
		//]]>
	</script>

</body>
</html>
