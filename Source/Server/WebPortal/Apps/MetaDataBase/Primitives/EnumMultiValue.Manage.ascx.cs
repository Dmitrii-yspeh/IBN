﻿using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Mediachase.Ibn.Data;
using Mediachase.Ibn.Data.Meta;
using Mediachase.Ibn.Data.Meta.Management;
using Mediachase.Ibn.Web.UI.Controls.Util;
using System.Collections.Generic;

namespace Mediachase.Ibn.Web.UI.MetaDataBase.Primitives
{
	public partial class EnumMultiValue_Manage : System.Web.UI.UserControl, IManageControl, IAutogenerateSystemNames
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		#region IManageControl Members
		public string GetDefaultValue(bool AllowNulls)
		{
			string retVal = String.Empty;
			//exists meta enum type
			if (ViewState[this.ClientID + "_IsDefaultId"] != null)
				return ViewState[this.ClientID + "_IsDefaultId"].ToString();
			//new meta enum type
			else if (ViewState[this.ClientID + "_DataSource"] != null)
			{
				DataTable dt = ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy();
				foreach (DataRow dr in dt.Rows)
					if ((bool)dr["IsDefault"])
					{
						if (retVal != String.Empty)
							retVal += ",";
						retVal += dr["Id"].ToString();
					}
			}
			return retVal;
		}

		public Mediachase.Ibn.Data.Meta.Management.AttributeCollection FieldAttributes
		{
			get
			{
				Mediachase.Ibn.Data.Meta.Management.AttributeCollection Attr = new Mediachase.Ibn.Data.Meta.Management.AttributeCollection();
				Attr.Add(McDataTypeAttribute.EnumEditable, chkEditable.Checked);
				Attr.Add(McDataTypeAttribute.EnumMultivalue, true);
				//enum changeable attribute
				Attr.Add("EnumFriendlyName", txtFriendlyName.Text.Trim());

				MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[ViewState[this.ClientID + "_TypeName"].ToString()];
				if (mft == null && ViewState[this.ClientID + "_DataSource"] != null)
				{
					//new meta enum type
					Attr.Add("NewMultiEnum", "1");

					string enumName;
					if (!AutogenerateSystemNames)
					{
						enumName = txtEnumName.Text.Trim();
					}
					else
					{
						// Generate the enum name as the number of seconds elapsed since 2000-01-01
						enumName = String.Format(CultureInfo.InvariantCulture, "Enum{0}", CHelper.GetDateDiffInSeconds(DateTime.UtcNow, new DateTime(2000, 1, 1)));
					}
					Attr.Add("EnumName", enumName);
					Attr.Add("EnumPrivate", !chkPublic.Checked);
					Attr.Add("EnumDataSource", ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy());
				}
				return Attr;
			}
		}

		//new meta field
		public void BindData(MetaClass mc, string FieldType)
		{
			ViewState[this.ClientID + "_TypeName"] = FieldType;

			MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[FieldType];
			if (mft == null)	//new meta enum type
			{
				if (!AutogenerateSystemNames)
				{
					txtEnumName.Visible = true;
					txtEnumName.Attributes.Add("onblur", "SetName('" + txtEnumName.ClientID + "','" + txtFriendlyName.ClientID + "','" + vldFriendlyName_Required.ClientID + "')");
					trName.Visible = true;
				}
				else
				{
					trName.Visible = false;
				}
				chkPublic.Enabled = true;
			}
			else	//exists meta enum type
			{
				trName.Visible = false;
				trFriendlyName.Visible = false;
				txtFriendlyName.Text = mft.FriendlyName;

				if (mft.Attributes.ContainsKey(McDataTypeAttribute.EnumPrivate) &&
					mft.Attributes[McDataTypeAttribute.EnumPrivate].ToString() == mc.Name)
					chkPublic.Checked = false;
				else
					chkPublic.Checked = true;

				chkPublic.Enabled = false;
			}

			BindGrid(GetDataTable());
		}

		//edit meta field
		public void BindData(MetaField mf)
		{
			ViewState[this.ClientID + "_TypeName"] = mf.TypeName;

			MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[mf.TypeName];

			trName.Visible = false;
			trFriendlyName.Visible = false;
			txtFriendlyName.Text = mft.FriendlyName;

			DataTable dt = GetDataTable().Copy();

			object defaultValue = DefaultValue.Evaluate(mf);
			if (defaultValue != null)
			{
				List<int> lst = new List<int>((int[])defaultValue);

				foreach (DataRow dr in dt.Rows)
					if (lst.Contains((int)dr["Id"]))
					{
						dr["IsDefault"] = true;
						string sDef = String.Empty;
						if (ViewState[this.ClientID + "_IsDefaultId"] != null)
							sDef = ViewState[this.ClientID + "_IsDefaultId"].ToString();
						if (!String.IsNullOrEmpty(sDef))
							sDef += ",";
						sDef += dr["Id"].ToString();

						ViewState[this.ClientID + "_IsDefaultId"] = sDef;
					}
			}
			ViewState[this.ClientID + "_DataSource"] = dt;

			BindGrid(dt);

			if (mf.Attributes.ContainsKey(McDataTypeAttribute.EnumEditable))
				chkEditable.Checked = (bool)mf.Attributes[McDataTypeAttribute.EnumEditable];

			if (mft.Attributes.ContainsKey(McDataTypeAttribute.EnumPrivate) &&
				mft.Attributes[McDataTypeAttribute.EnumPrivate].ToString() == mf.Owner.Name)
				chkPublic.Checked = false;
			else
				chkPublic.Checked = true;

			chkPublic.Enabled = false;
		}
		#endregion


		#region BindGrid
		private void BindGrid(DataTable dt)
		{
			DataView dv = dt.DefaultView;
			dv.Sort = "OrderId";
			grdMain.DataSource = dv;
			grdMain.DataBind();

			foreach (DataGridItem row in grdMain.Items)
			{
				ImageButton ib = (ImageButton)row.FindControl("ibDelete");
				if (ib != null)
					ib.Attributes.Add("onclick", "return confirm('" + GetGlobalResourceObject("IbnFramework.GlobalMetaInfo", "Delete").ToString() + "?')");
				TextBox tb = (TextBox)row.FindControl("txtName");
				HtmlImage im = (HtmlImage)row.FindControl("imResourceTemplate");
				if (im != null)
				{
					im.Src = CHelper.GetAbsolutePath("/images/IbnFramework/resource.gif");
					im.Attributes.Add("title", GetGlobalResourceObject("IbnFramework.GlobalMetaInfo", "ResourceTooltip").ToString());
					if (tb != null)
						im.Attributes.Add("onclick", "SetText('" + tb.ClientID + "','{ResourceName:ResourceKey}','')");
				}
			}

			if (grdMain.EditItemIndex >= 0)
			{
				DataGridItem dgi = grdMain.Items[grdMain.EditItemIndex];

				//ordering dropdownlist
				DropDownList ddl = (DropDownList)dgi.FindControl("ddlOrder");
				if (ddl != null)
				{
					for (int i = 1; i <= grdMain.Items.Count; i++)
					{
						ddl.Items.Add(i.ToString());
					}
					ddl.SelectedIndex = grdMain.EditItemIndex;
				}

				//IsDefault checking
				CheckBox cb = (CheckBox)dgi.FindControl("cbDefault");
				Label lbl = (Label)dgi.FindControl("lblDefault");
				if (cb != null)
					cb.Checked = (lbl.Text.ToLower() == "true");
			}
		}
		#endregion

		#region GetDataTable
		private DataTable GetDataTable()
		{
			if (ViewState[this.ClientID + "_TypeName"] == null)
				return null;

			MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[ViewState[this.ClientID + "_TypeName"].ToString()];

			DataTable dt = new DataTable();
			dt.Locale = CultureInfo.InvariantCulture;
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("OrderId", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Columns.Add("DisplayName", typeof(string));
			dt.Columns.Add("IsDefault", typeof(bool));

			if (mft != null)
			{
				//exists meta enum type
				foreach (MetaEnumItem item in MetaEnum.GetItems(mft))
				{
					DataRow row = dt.NewRow();
					row["Id"] = item.Handle;
					row["OrderId"] = item.OrderId;
					row["Name"] = item.Name;
					row["DisplayName"] = CHelper.GetResFileString(item.Name);
					row["IsDefault"] = false;
					if (ViewState[this.ClientID + "_IsDefaultId"] != null)
					{
						string sDef =  ViewState[this.ClientID + "_IsDefaultId"].ToString();
						string[] mas = sDef.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
						List<string> lst = new List<string>(mas);
						if(lst.Contains(item.Handle.ToString()))
							row["IsDefault"] = true;
					}	
					dt.Rows.Add(row);
				}
			}
			else
			{
				//new meta enum type
				if (ViewState[this.ClientID + "_DataSource"] != null)
					dt = ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy();
				else
					ViewState[this.ClientID + "_DataSource"] = dt;
			}
			return dt;
		}
		#endregion

		#region grdMain_CancelCommand
		protected void grdMain_CancelCommand(object source, DataGridCommandEventArgs e)
		{
			grdMain.EditItemIndex = -1;
			BindGrid(GetDataTable());
		}
		#endregion

		#region grdMain_DeleteCommand
		protected void grdMain_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			if (ViewState[this.ClientID + "_TypeName"] == null)
				return;

			MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[ViewState[this.ClientID + "_TypeName"].ToString()];

			if (mft != null)
			{
				//exists meta enum type
				MetaEnum.RemoveItem(mft, int.Parse(e.CommandArgument.ToString()));
				
				//update default values
				string sDef = String.Empty;
				if (ViewState[this.ClientID + "_IsDefaultId"] != null)
					sDef = ViewState[this.ClientID + "_IsDefaultId"].ToString();
				string[] mas = sDef.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				List<string> lst = new List<string>(mas);
				if (lst.Contains(e.CommandArgument.ToString()))
				{
					lst.Remove(e.CommandArgument.ToString());
					ViewState[this.ClientID + "_IsDefaultId"] = String.Join(",", lst.ToArray());
				}
			}
			else if (ViewState[this.ClientID + "_DataSource"] != null)
			{
				//new meta enum type
				DataTable dt = ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy();
				DataRow drDelete = null;
				foreach (DataRow dr in dt.Rows)
					if (dr["Id"].ToString() == e.CommandArgument.ToString())
					{
						drDelete = dr;
						break;
					}

				//remove ordering
				foreach (DataRow dr in dt.Rows)
					if ((int)dr["OrderId"] > (int)drDelete["OrderId"])
						dr["OrderId"] = (int)dr["OrderId"] - 1;

				if (drDelete != null)
					dt.Rows.Remove(drDelete);

				ViewState[this.ClientID + "_DataSource"] = dt;
			}

			BindGrid(GetDataTable());
		}
		#endregion

		#region grdMain_EditCommand
		protected void grdMain_EditCommand(object source, DataGridCommandEventArgs e)
		{
			grdMain.EditItemIndex = e.Item.ItemIndex;
			BindGrid(GetDataTable());
		}
		#endregion

		#region grdMain_UpdateCommand
		protected void grdMain_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			if (ViewState[this.ClientID + "_TypeName"] == null)
				return;

			MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[ViewState[this.ClientID + "_TypeName"].ToString()];

			int ItemId = int.Parse(e.CommandArgument.ToString());

			TextBox tb = (TextBox)e.Item.FindControl("txtName");
			DropDownList ddl = (DropDownList)e.Item.FindControl("ddlOrder");
			CheckBox cb = (CheckBox)e.Item.FindControl("cbDefault");
			int OrderId = int.Parse(ddl.SelectedValue);

			if (tb != null && tb.Text.Trim() != String.Empty)
			{
				if (ItemId > 0)
				{
					//edit meta enum item
					if (mft != null)
					{
						//exists meta enum type
						MetaEnum.UpdateItem(mft, ItemId, tb.Text.Trim(), OrderId);
						if (cb.Checked)
						{
							//add default value
							string sDef = String.Empty;
							if (ViewState[this.ClientID + "_IsDefaultId"] != null)
								sDef = ViewState[this.ClientID + "_IsDefaultId"].ToString();
							if (sDef != String.Empty)
								sDef += ",";
							sDef += ItemId.ToString();

							ViewState[this.ClientID + "_IsDefaultId"] = sDef;
						}
						else
						{
							//remove default value
							string sDef = String.Empty;
							if (ViewState[this.ClientID + "_IsDefaultId"] != null)
								sDef = ViewState[this.ClientID + "_IsDefaultId"].ToString();
							string[] mas = sDef.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
							List<string> lst = new List<string>(mas);
							if(lst.Contains(ItemId.ToString()))
							{
								lst.Remove(ItemId.ToString());
								ViewState[this.ClientID + "_IsDefaultId"] = String.Join(",", lst.ToArray());
							}
						}
					}
					else
					{
						//new meta enum type
						DataTable dt = ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy();
						DataRow drUpdate = null;
						foreach (DataRow dr in dt.Rows)
							if ((int)dr["Id"] == ItemId)
							{
								drUpdate = dr;
								break;
							}
						if (drUpdate != null)
						{
							drUpdate["Name"] = tb.Text.Trim();
							drUpdate["DisplayName"] = CHelper.GetResFileString(tb.Text.Trim());
							//ordering
							int oldOrderId = (int)drUpdate["OrderId"];
							foreach (DataRow dr in dt.Rows)
							{
								int curOrder = (int)dr["OrderId"];
								if (oldOrderId > OrderId)
								{
									if (curOrder >= OrderId && curOrder < oldOrderId)
										dr["OrderId"] = curOrder + 1;
								}
								else
								{
									if (curOrder > oldOrderId && curOrder <= OrderId)
										dr["OrderId"] = curOrder - 1;
								}
							}
							drUpdate["OrderId"] = OrderId;
							drUpdate["IsDefault"] = cb.Checked;
						}
						ViewState[this.ClientID + "_DataSource"] = dt;
					}
				}
				else
				{
					//add meta enum item
					if (mft != null)
					{
						//exists meta enum type
						int id = MetaEnum.AddItem(mft, tb.Text.Trim(), OrderId);
						if (cb.Checked)
						{
							//add default value
							string sDef = String.Empty;
							if (ViewState[this.ClientID + "_IsDefaultId"] != null)
								sDef = ViewState[this.ClientID + "_IsDefaultId"].ToString();
							if (sDef != String.Empty)
								sDef += ",";
							sDef += id.ToString();

							ViewState[this.ClientID + "_IsDefaultId"] = sDef;
						}
						else
						{
							//remove default value
							string sDef = String.Empty;
							if (ViewState[this.ClientID + "_IsDefaultId"] != null)
								sDef = ViewState[this.ClientID + "_IsDefaultId"].ToString();
							string[] mas = sDef.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							List<string> lst = new List<string>(mas);
							if (lst.Contains(id.ToString()))
							{
								lst.Remove(id.ToString());
								ViewState[this.ClientID + "_IsDefaultId"] = String.Join(",", lst.ToArray());
							}
						}
					}
					else
					{
						//new meta enum type
						DataTable dt = ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy();
						int id = 0;
						//id & ordering definition
						foreach (DataRow dr in dt.Rows)
						{
							if ((int)dr["Id"] >= id)
								id = (int)dr["Id"];
							if ((int)dr["OrderId"] >= OrderId)
								dr["OrderId"] = (int)dr["OrderId"] + 1;
						}
						id = id + 1;
						DataRow drNew = dt.NewRow();
						drNew["Id"] = id;
						drNew["OrderId"] = OrderId;
						drNew["Name"] = tb.Text.Trim();
						drNew["DisplayName"] = CHelper.GetResFileString(tb.Text.Trim());
						drNew["IsDefault"] = cb.Checked;
						dt.Rows.Add(drNew);
						ViewState[this.ClientID + "_DataSource"] = dt;
					}
				}
			}

			grdMain.EditItemIndex = -1;
			BindGrid(GetDataTable());
		}
		#endregion

		#region grdMain_ItemCommand
		protected void grdMain_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "NewItem")
			{
				DataTable dt = GetDataTable();

				DataRow dr = dt.NewRow();
				dr["Id"] = -1;
				dr["OrderId"] = dt.Rows.Count + 1;
				dr["Name"] = "";
				dr["DisplayName"] = "";
				dr["IsDefault"] = false;
				dt.Rows.Add(dr);

				grdMain.EditItemIndex = dt.Rows.Count - 1;
				BindGrid(dt);
			}

			if (e.CommandName == "Asc")
			{
				MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[ViewState[this.ClientID + "_TypeName"].ToString()];
				if (mft == null)
				{
					DataTable dt = ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy();
					DataView dv = dt.DefaultView;
					dv.Sort = "Name";
					for (int i = 0; i < dv.Count; i++)
						dv[i]["OrderId"] = i + 1;
					ViewState[this.ClientID + "_DataSource"] = dt;
				}
				else
				{
					DataTable dt = new DataTable();
					dt.Columns.Add(new DataColumn("Id", typeof(int)));
					dt.Columns.Add(new DataColumn("OrderId", typeof(int)));
					dt.Columns.Add(new DataColumn("Name", typeof(string)));
					DataRow dr;
					foreach (MetaEnumItem item in MetaEnum.GetItems(mft))
					{
						dr = dt.NewRow();
						dr["Id"] = item.Handle;
						dr["OrderId"] = item.OrderId;
						dr["Name"] = item.Name;
						dt.Rows.Add(dr);
					}
					DataView dv = dt.DefaultView;
					dv.Sort = "Name";

					using (MetaClassManagerEditScope scope = DataContext.Current.MetaModel.BeginEdit())
					{
						for (int i = 0; i < dv.Count; i++)
							MetaEnum.UpdateItem(mft, (int)dv[i]["Id"], dv[i]["Name"].ToString(), i + 1);

						scope.SaveChanges();
					}
				}

				BindGrid(GetDataTable());
			}

			if (e.CommandName == "Desc")
			{
				MetaFieldType mft = DataContext.Current.MetaModel.RegisteredTypes[ViewState[this.ClientID + "_TypeName"].ToString()];
				if (mft == null)
				{
					DataTable dt = ((DataTable)ViewState[this.ClientID + "_DataSource"]).Copy();
					DataView dv = dt.DefaultView;
					dv.Sort = "Name DESC";
					for (int i = 0; i < dv.Count; i++)
						dv[i]["OrderId"] = i + 1;
					ViewState[this.ClientID + "_DataSource"] = dt;
				}
				else
				{
					DataTable dt = new DataTable();
					dt.Columns.Add(new DataColumn("Id", typeof(int)));
					dt.Columns.Add(new DataColumn("OrderId", typeof(int)));
					dt.Columns.Add(new DataColumn("Name", typeof(string)));
					DataRow dr;
					foreach (MetaEnumItem item in MetaEnum.GetItems(mft))
					{
						dr = dt.NewRow();
						dr["Id"] = item.Handle;
						dr["OrderId"] = item.OrderId;
						dr["Name"] = item.Name;
						dt.Rows.Add(dr);
					}
					DataView dv = dt.DefaultView;
					dv.Sort = "Name DESC";
					using (MetaClassManagerEditScope scope = DataContext.Current.MetaModel.BeginEdit())
					{
						for (int i = 0; i < dv.Count; i++)
							MetaEnum.UpdateItem(mft, (int)dv[i]["Id"], dv[i]["Name"].ToString(), i + 1);
						scope.SaveChanges();
					}
				}

				BindGrid(GetDataTable());
			}
		}
		#endregion

		#region IAutogenerateSystemNames Members
		public bool AutogenerateSystemNames
		{
			get
			{
				bool retval = false;
				if (ViewState["AutogenerateSystemNames"] != null)
					retval = (bool)ViewState["AutogenerateSystemNames"];
				return retval;
			}
			set
			{
				ViewState["AutogenerateSystemNames"] = value;
			}
		}
		#endregion
	}
}