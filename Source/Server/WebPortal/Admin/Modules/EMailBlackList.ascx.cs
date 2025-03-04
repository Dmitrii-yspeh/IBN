namespace Mediachase.UI.Web.Admin.Modules
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Resources;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Mediachase.IBN.Business;
	using Mediachase.IBN.Business.EMail;
	using System.Reflection;

	/// <summary>
	///		Summary description for EMailBlackList.
	/// </summary>
	public partial class EMailBlackList : System.Web.UI.UserControl
	{

		protected ResourceManager LocRM = new ResourceManager("Mediachase.Ibn.WebResources.App_GlobalResources.Admin.Resources.strMailIncidents", Assembly.Load(new AssemblyName("Mediachase.Ibn.WebResources")));

		UserLightPropertyCollection pc = Security.CurrentUser.Properties;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
				BindDataGrid();
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			secHeader.Title = LocRM.GetString("tBlackList");
			btnSearch.Text = LocRM.GetString("tApply");
			btnReset.Text = LocRM.GetString("tReset");
			btnAdd.Text = LocRM.GetString("tAdd");
			secHeader.AddLink("<img width='16' height='16' title='" + LocRM.GetString("tImportList") + "' border='0' align='top' src='" +
				this.Page.ResolveUrl("~/Layouts/Images/import.gif") + "'/>&nbsp;" + LocRM.GetString("tImportList"), string.Format("javascript:OpenWindow('{0}?listtype=Black', 400,200,false);", this.Page.ResolveUrl("~/Admin/EmailListImport.aspx")));
			secHeader.AddLink("<img width='16' height='16' title='" + LocRM.GetString("tExportList") + "' border='0' align='top' src='" +
				this.Page.ResolveUrl("~/Layouts/Images/Icons/txtexport.gif") + "'/>&nbsp;" + LocRM.GetString("tExportList"), this.Page.ResolveUrl("~/Admin/EmailListExport.aspx?listtype=Black"));
			secHeader.AddLink("<img width='16' height='16' title='" + LocRM.GetString("tDeleteChecked") + "' border='0' align='top' src='" +
				this.Page.ResolveUrl("~/Layouts/Images/delete.gif") + "'/>&nbsp;" + LocRM.GetString("tDeleteChecked"), "javascript:ActionDelete()");
			secHeader.AddLink("<img alt='' src='" + this.Page.ResolveUrl("~/Layouts/Images/cancel.gif") + "'/> " + LocRM.GetString("tHDM"), ResolveUrl("~/Apps/Administration/Pages/default.aspx?NodeId=MAdmin5"));

		}

		#region BindDataGrid
		private void BindDataGrid()
		{
			int i = 1;
			dgBlackList.Columns[i++].HeaderText = "ID";
			dgBlackList.Columns[i++].HeaderText = LocRM.GetString("tAddress");

			DataTable dt = new DataTable();
			dt.Columns.Add(new DataColumn("Id", typeof(int)));
			dt.Columns.Add(new DataColumn("Address", typeof(string)));

			DataRow dr;
			string sSearch = txtSearch.Text;
			foreach (BlackListItem bi in BlackListItem.List(sSearch))
			{
				dr = dt.NewRow();
				dr["Id"] = bi.Id;
				dr["Address"] = bi.From;
				dt.Rows.Add(dr);
			}
			DataView dv = dt.DefaultView;
			dv.Sort = "Address";

			dgBlackList.DataSource = dv;

			if (pc["emr_BL_PageSize"] != null)
				dgBlackList.PageSize = int.Parse(pc["emr_BL_PageSize"].ToString());

			if (pc["emr_BL_Page"] != null)
			{
				int iPageIndex = int.Parse(pc["emr_BL_Page"].ToString());
				int ppi = dv.Count / dgBlackList.PageSize;
				if (dv.Count % dgBlackList.PageSize == 0)
					ppi = ppi - 1;
				if (iPageIndex <= ppi)
					dgBlackList.CurrentPageIndex = iPageIndex;
				else dgBlackList.CurrentPageIndex = 0;
			}
			dgBlackList.DataBind();

			foreach (DataGridItem dgi in dgBlackList.Items)
			{
				ImageButton ib = (ImageButton)dgi.FindControl("ibDelete");
				if (ib != null)
				{
					ib.Attributes.Add("title", LocRM.GetString("tDelete"));
					ib.Attributes.Add("onclick", "return confirm('" + LocRM.GetString("tWarning2") + "')");
				}
			}
		}
		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgBlackList.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dg_PageIndexChanged);
			this.dgBlackList.PageSizeChanged += new Mediachase.UI.Web.Modules.DGExtension.DataGridPageSizeChangedEventHandler(this.dg_PageSizeChange);
			this.dgBlackList.DeleteCommand += new DataGridCommandEventHandler(dg_DeleteCommand);
			this.lbDeleteChecked.Click += new EventHandler(lbDeleteChecked_Click);
			this.btnSearch.Click += new EventHandler(btnSearch_Click);
			this.btnReset.Click += new EventHandler(btnReset_Click);
			this.btnAdd.Click += new EventHandler(btnAdd_Click);
		}
		#endregion

		#region DataGrid_Events
		private void dg_PageSizeChange(object source, Mediachase.UI.Web.Modules.DGExtension.DataGridPageSizeChangedEventArgs e)
		{
			pc["emr_BL_PageSize"] = e.NewPageSize.ToString();
			BindDataGrid();
		}

		private void dg_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			pc["emr_BL_Page"] = e.NewPageIndex.ToString();
			BindDataGrid();
		}

		private void dg_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			int Id = int.Parse(e.CommandArgument.ToString());
			BlackListItem.Delete(Id);
			Response.Redirect("~/Admin/EMailBlackList.aspx");
		}
		#endregion

		#region Delete_Checked
		private void lbDeleteChecked_Click(object sender, EventArgs e)
		{
			string sIds = hidForDelete.Value;
			ArrayList alIds = new ArrayList();
			while (sIds.Length > 0)
			{
				string sId = sIds.Substring(0, sIds.IndexOf(","));
				alIds.Add(int.Parse(sId));
				sIds = sIds.Remove(0, sIds.IndexOf(",") + 1);
			}
			BlackListItem.Delete(alIds);
			Response.Redirect("~/Admin/EMailBlackList.aspx");
		}
		#endregion

		#region Search-Reset
		private void btnSearch_Click(object sender, EventArgs e)
		{
			BindDataGrid();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			txtSearch.Text = "";
			BindDataGrid();
		}
		#endregion

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (txtAdd.Text != "")
			{
				BlackListItem.Create(txtAdd.Text);
				Response.Redirect(this.Page.ResolveUrl("~/Admin/EMailBlackList.aspx"));
			}
		}
	}
}
