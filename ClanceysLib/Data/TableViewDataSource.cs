using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections;
namespace ClanceysLib
{
	public class TableViewDataSource : UITableViewDataSource
	{
		object[] dataArray;
		public string CellTextLabelMember = "";
		public string CellDetailTextLabelMember = "";
		public string CellSubsections = "";
		public string CellSectionHeader = "";
		public String CellSectionFooter = "";
		public UITableViewCellAccessory cellAccessory = UITableViewCellAccessory.None;
		public UITableViewCellStyle cellStyle = UITableViewCellStyle.Default;
		public string CellID = "cell";
		public UIImage checkedImage;
		public UIImage unCheckedImage;
		public bool isChecked;
		private bool subSectionsFound = false;

		public TableViewDataSource (object[] inArray)
		{
			dataArray = inArray;
		}
		public TableViewDataSource (IList inList)
		{
			dataArray = (new ArrayList ((IList)inList)).ToArray ();
		}

		public TableViewDataSource (object[] inArray, string TextLabelMember)
		{
			dataArray = inArray;
			CellTextLabelMember = TextLabelMember;
		}

		/// <summary>
		/// Called by the TableView to determine how many sections(groups) there are.
		/// </summary>
		public override int NumberOfSections (UITableView tableView)
		{
			if (string.IsNullOrEmpty (CellSubsections))
			{
				return 1;
			}

			else
			{
				return dataArray.Count ();
			}
		}

		/// <summary>
		/// Called by the TableView to determine how many cells to create for that particular section.
		/// </summary>
		public override int RowsInSection (UITableView tableview, int section)
		{
			if (string.IsNullOrEmpty (CellSubsections))
			{
				return dataArray.Count ();
			}

			else
			{
				return getPropertyArray (this.dataArray[section], CellSubsections).Count ();
			}
		}

		/// <summary>
		/// Called by the TableView to retrieve the header text for the particular section(group)
		/// </summary>
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return this.getPropertyValue (this.dataArray[section], CellSectionHeader);
		}

		/// <summary>
		/// Called by the TableView to retrieve the footer text for the particular section(group)
		/// </summary>
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return this.getPropertyValue (this.dataArray[section], CellSectionFooter);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			
			var cell = tableView.DequeueReusableCell (CellID);
			if (cell == null)
				cell = new UITableViewCell (cellStyle, CellID);
			
			object l;
			if (string.IsNullOrEmpty (CellSubsections))
			{
				l = dataArray.ElementAt (indexPath.Row);
			}

			else
			{
				l = getPropertyArray (dataArray[indexPath.Section], CellSubsections).ElementAt (indexPath.Row);
			}
			
			if (cell.TextLabel != null)
				cell.TextLabel.Text = getPropertyValue (l, CellTextLabelMember);
			
			if (cell.DetailTextLabel != null)
			{
				cell.DetailTextLabel.Text = getPropertyValue (l, CellDetailTextLabelMember);
				//cell.DetailTextLabel.BackgroundColor = UIColor.Purple;
			}
			UIView bg = new UIView (cell.Frame);
			//bg.BackgroundColor = UIColor.Purple;
			cell.BackgroundView = bg;
			cell.Accessory = cellAccessory;
			//cell.BackgroundColor = UIColor.Purple;
			return cell;
		}

		private string getPropertyValue (object inObject, string propertyName)
		{
			PropertyInfo[] props = inObject.GetType ().GetProperties ();
			PropertyInfo prop = props.Select (p => p).Where (p => p.Name == propertyName).FirstOrDefault ();
			if (prop != null)
				return prop.GetValue (inObject, null).ToString ();
			return "";
		}
		private object[] getPropertyArray (object inObject, string propertyName)
		{
			PropertyInfo[] props = inObject.GetType ().GetProperties ();
			PropertyInfo prop = props.Select (p => p).Where (p => p.Name == propertyName).FirstOrDefault ();
			if (prop != null)
			{
				var currentObject = prop.GetValue (inObject, null);
				if (currentObject.GetType ().GetGenericTypeDefinition () == typeof(List<>))
				{
					return (new ArrayList ((IList)currentObject)).ToArray ();
				}

				else if (currentObject is Array)
				{
					return (object[])currentObject;
				}
				else
				{
					return new object[1];
				}
			}
			return new object[1];
		}
		
		
		
	}
}

