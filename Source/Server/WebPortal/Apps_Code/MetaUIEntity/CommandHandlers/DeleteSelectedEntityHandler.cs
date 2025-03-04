﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Mediachase.Ibn.Core.Business;
using Mediachase.Ibn.Data;
using Mediachase.Ibn.Web.UI.WebControls;
using Mediachase.UI.Web.Apps.MetaUI.Grid;

namespace Mediachase.Ibn.Web.UI.MetaUIEntity.CommandHandlers
{
	public class DeleteSelectedEntityHandler : ICommand
	{
		#region ICommand Members

		public void Invoke(object Sender, object Element)
		{
			CommandParameters cp = Element as CommandParameters;
			if (cp != null)
			{
				string[] selectedElements = EntityGrid.GetCheckedCollection(((CommandManager)Sender).Page, cp.CommandArguments["GridId"]);
				if (selectedElements != null && selectedElements.Length > 0)
				{
					int errorCount = 0;
					using (TransactionScope tran = DataContext.Current.BeginTransaction())
					{
						foreach (string elem in selectedElements)
						{
							string[] parts = elem.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
							string id = parts[0];

							// O.R. [2009-04-21] fix for empty row
							if (id != null && id != "null")
							{
								PrimaryKeyId key = PrimaryKeyId.Parse(id);
								string className = parts[1];

								try
								{
									BusinessManager.Delete(className, key);
								}
								catch (Exception ex)
								{
									CHelper.GenerateErrorReport(ex);
									errorCount++;
								}
							}
						}

						tran.Commit();
					}

					if (errorCount > 0)
						((CommandManager)Sender).InfoMessage = CHelper.GetResFileString("{IbnFramework.Common:NotAllSelectedItemsWereProcessed}");
				}
			}
			CHelper.RequireDataBind();
		}
		#endregion
	}
}
