using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;
using GalleryServerPro.Web.Controller;
using Image = GalleryServerPro.Business.Image;

namespace GalleryServerPro.Web.Pages.Task
{
	/// <summary>
	/// A page-like user control that handles the Delete high-res images task.
	/// </summary>
	public partial class deletehires : Pages.TaskPage
	{
		#region Private Fields

		private long _totalHiResSizeKB = long.MinValue; // Holds the total size of all hi-res images in this album
		private IGalleryObjectCollection _galleryObjects;

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handles the Init event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Init(object sender, EventArgs e)
		{
			this.TaskHeaderPlaceHolder = phTaskHeader;
			this.TaskFooterPlaceHolder = phTaskFooter;
		}

		/// <summary>
		/// Handles the Load event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (GallerySettings.MediaObjectPathIsReadOnly)
				RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotEditGalleryIsReadOnly).ToString(CultureInfo.InvariantCulture));

			this.CheckUserSecurity(SecurityActions.EditMediaObject);

			if (!IsPostBack)
			{
				ConfigureControls();
			}

			ConfigureControlsEveryPageLoad();
		}

		/// <summary>
		/// Determines whether the event for the server control is passed up the page's UI server control hierarchy.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="args">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		/// <returns>
		/// true if the event has been canceled; otherwise, false. The default is false.
		/// </returns>
		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			//An event from a control has bubbled up.  If it's the Ok button, then run the
			//code to synchronize; otherwise ignore.
			Button btn = source as Button;
			if ((btn != null) && (((btn.ID == "btnOkTop") || (btn.ID == "btnOkBottom"))))
			{
				if (btnOkClicked())
				{
					this.RedirectToAlbumViewPage("msg={0}", ((int)Message.HiResImagesSuccessfullyDeleted).ToString(CultureInfo.InvariantCulture));
				}
			}

			return true;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the gallery objects that are candidates for deleting high res images. This includes all images in the current album
		/// and all child albums.
		/// </summary>
		/// <value>The gallery objects that are candidates for deleting high res images.</value>
		public IGalleryObjectCollection GalleryObjects
		{
			get
			{
				if (this._galleryObjects == null)
				{
					this._galleryObjects = this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.Image);
					this._galleryObjects.AddRange(this.GetAlbum().GetChildGalleryObjects(GalleryObjectType.Album));
					this._galleryObjects.Sort();
				}

				return this._galleryObjects;
			}
		}

		/// <summary>
		/// Gets the total file size, in KB, of all the high resolution images in the current album, including all 
		/// child albums. The total does not include images that do not have a high resolution version, nor does 
		/// it include file size of any other type of media object, such as video or audio files.
		/// </summary>
		public long TotalFileSizeKbAllHiResImages
		{
			get
			{
				if (_totalHiResSizeKB == long.MinValue)
				{
					this._totalHiResSizeKB = GetFileSizeKbAllHiResImagesInAlbum(this.GetAlbum());
				}

				return this._totalHiResSizeKB;
			}
		}

		/// <summary>
		/// Gets the total file size, in KB, of all the high resolution images in the <paramref name="album"/>, including all 
		/// child albums. The total does not include images that do not have a high resolution version, nor does 
		/// it include file size of any other type of media object, such as video or audio files.
		/// </summary>
		/// <param name="album">The album for which to retrieve the file size of all high res images.</param>
		/// <returns>Returns the total file size, in KB, of all the high resolution images in the <paramref name="album"/>.</returns>
		private static long GetFileSizeKbAllHiResImagesInAlbum(IAlbum album)
		{
			// Get the total file size, in KB, of all the high resolution images in the specified album
			long sumTotal = 0;
			foreach (IGalleryObject go in album.GetChildGalleryObjects(GalleryObjectType.Image))
			{
				if (go.Original.FileName != go.Optimized.FileName)
					sumTotal += go.Original.FileSizeKB;
			}

			foreach (IAlbum childAlbum in album.GetChildGalleryObjects(GalleryObjectType.Album))
			{
				sumTotal += GetFileSizeKbAllHiResImagesInAlbum(childAlbum);
			}

			return sumTotal;
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Return a string representing the title of the gallery object. It is truncated and purged of HTML tags
		/// if necessary.
		/// </summary>
		/// <param name="title">The title of the gallery object as stored in the data store.</param>
		/// <param name="galleryObjectType">The type of the object to which the title belongs.</param>
		/// <returns>Returns a string representing the title of the media object. It is truncated and purged of HTML tags
		/// if necessary.</returns>
		protected string GetGalleryObjectText(string title, Type galleryObjectType)
		{
			if (String.IsNullOrEmpty(title))
				return String.Empty;

			// If this is an album, return an empty string. Otherwise, return the title, truncated and purged of HTML
			// tags if necessary. If the title is truncated, add an ellipses to the text.
			int maxLength = GallerySettings.MaxMediaObjectThumbnailTitleDisplayLength;

			if (galleryObjectType == typeof(Album))
			{
				maxLength = GallerySettings.MaxAlbumThumbnailTitleDisplayLength;
			}

			string truncatedText = Utils.TruncateTextForWeb(title, maxLength);

			if (truncatedText.Length != title.Length)
				return String.Concat(truncatedText, "...");
			else
				return truncatedText;
		}

		/// <summary>
		/// Gets a value indicating whether the page should display a "no hi-res image" message for the <paramref name="galleryObject"/>.
		/// Albums should never show the "no hi-res image" message, and neither should images without hi-res versions.
		/// </summary>
		/// <param name="galleryObject">The gallery object.</param>
		/// <returns>Returns true if a "no hi-res image" message should be shown; otherwise returns false.</returns>
		protected static bool ShouldShowNoHiResImageMsg(IGalleryObject galleryObject)
		{
			if (galleryObject == null)
				throw new ArgumentNullException("galleryObject");

			if (galleryObject.GetType() == typeof(Album))
				return false;

			return !(DoesHiResImageExist(galleryObject.Optimized.FileName, galleryObject.Original.FileName));
		}

		/// <summary>
		/// Gets a value indicating whether the page should display a "delete hi-res" checkbox for the <paramref name="galleryObject"/>.
		/// Albums should always show the checkbox; images should only if a hi-res image exists.
		/// </summary>
		/// <param name="galleryObject">The gallery object.</param>
		/// <returns>Returns true if a "delete hi-res" checkbox should be shown; otherwise returns false.</returns>
		protected static bool ShouldShowCheckbox(IGalleryObject galleryObject)
		{
			if (galleryObject == null)
				throw new ArgumentNullException("galleryObject");

			if (galleryObject.GetType() == typeof(Album))
				return true;

			return (DoesHiResImageExist(galleryObject.Optimized.FileName, galleryObject.Original.FileName));
		}

		protected static string GetId(IGalleryObject galleryObject)
		{
			if (galleryObject == null)
				throw new ArgumentNullException("galleryObject");

			// Prepend an 'a' (for album) or 'm' (for media object) to the ID to indicate whether it is
			// an album ID or media object ID.
			if (galleryObject is Album)
				return "a" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);
			else
				return "m" + galleryObject.Id.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Calculate the potential hard drive savings, in KB, if all high res images were deleted from <paramref name="galleryObject"/>.
		/// If <paramref name="galleryObject"/> is an Album, then the value includes the sum of the size of all high res images
		/// within the album.
		/// </summary>
		/// <param name="galleryObject">The gallery object.</param>
		/// <returns>Returns the potential hard drive savings, in KB, if all high res images were deleted from <paramref name="galleryObject"/>.</returns>
		protected static string GetSavings(IGalleryObject galleryObject)
		{
			if (galleryObject == null)
				throw new ArgumentNullException("galleryObject");

			if (galleryObject.GetType() == typeof(Album))
				return String.Format(CultureInfo.CurrentCulture, "({0} KB)", GetFileSizeKbAllHiResImagesInAlbum((IAlbum)galleryObject));
			else
				return String.Format(CultureInfo.CurrentCulture, "({0} KB)", galleryObject.Original.FileSizeKB);
		}

		protected static bool DoesHiResImageExist(string optimizedFileName, string originalFileName)
		{
			// A hi-res version of an image exists if the optimized and original filenames are different.
			return String.Equals(optimizedFileName, originalFileName, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Gets a user-friendly string indicating the potential savings if the user deleted all high res images in the current album.
		/// </summary>
		/// <returns>Returns a string.</returns>
		protected string GetPotentialSavings()
		{
			return String.Format(CultureInfo.CurrentCulture, Resources.GalleryServerPro.Task_Delete_HiRes_Potential_Savings_Text, this.TotalFileSizeKbAllHiResImages, ((float)this.TotalFileSizeKbAllHiResImages / 1024F).ToString("N", CultureInfo.CurrentCulture));
		}

		/// <summary>
		/// Gets the CSS class to apply to the thumbnail object.
		/// </summary>
		/// <param name="galleryObjectType">The gallery object.</param>
		/// <returns>Returns a CSS class.</returns>
		protected static string GetThumbnailCssClass(Type galleryObjectType)
		{
			// If it's an album then specify the appropriate CSS class so that the "Album"
			// header appears over the thumbnail. This is to indicate to the user that the
			// thumbnail represents an album.
			if (galleryObjectType == typeof(Album))
				return "thmb album";
			else
				return "thmb";
		}

		#endregion

		#region Private Methods

		private void ConfigureControls()
		{
			this.TaskHeaderText = Resources.GalleryServerPro.Task_Delete_HiRes_Header_Text;
			this.TaskBodyText = Resources.GalleryServerPro.Task_Delete_HiRes_Body_Text;
			this.OkButtonText = Resources.GalleryServerPro.Task_Delete_HiRes_Ok_Button_Text;
			this.OkButtonToolTip = Resources.GalleryServerPro.Task_Delete_HiRes_Ok_Button_Tooltip;

			this.PageTitle = Resources.GalleryServerPro.Task_Delete_HiRes_Page_Title;

			if (GalleryObjects.Count > 0)
			{
				rptr.DataSource = GalleryObjects;
				rptr.DataBind();
			}
			else
			{
				this.RedirectToAlbumViewPage("msg={0}", ((int)Message.CannotDeleteHiResImagesNoObjectsExistInAlbum).ToString(CultureInfo.InvariantCulture));
			}
		}

		private void ConfigureControlsEveryPageLoad()
		{
			const int thumbHeightBuffer = 20; // Add a little height padding
			SetThumbnailCssStyle(GalleryObjects, 0, thumbHeightBuffer);
		}

		private bool btnOkClicked()
		{
			//User clicked 'Delete images'.  Delete the hi-res images for the selected albums and images.
			string[] selectedItems = RetrieveUserSelections();

			if (selectedItems.Length == 0)
			{
				// No images were selected. Inform user and exit function.
				string msg = String.Format(CultureInfo.CurrentCulture, "<p class='gsp_msgwarning'><span class='gsp_bold'>{0} </span>{1}</p>", Resources.GalleryServerPro.Task_No_Objects_Selected_Hdr, Resources.GalleryServerPro.Task_No_Objects_Selected_Dtl);
				phMsg.Controls.Clear();
				phMsg.Controls.Add(new System.Web.UI.LiteralControl(msg));

				return false;
			}

			try
			{
				HelperFunctions.BeginTransaction();

				// Convert the string array of IDs to integers. Also assign whether each is an album or media object.
				// (Determined by the first character of each id's string: a=album; m=media object)
				foreach (string selectedItem in selectedItems)
				{
					int id = Convert.ToInt32(selectedItem.Substring(1), CultureInfo.InvariantCulture);
					char idType = Convert.ToChar(selectedItem.Substring(0, 1), CultureInfo.InvariantCulture); // 'a' or 'm'

					if (idType == 'm')
					{
						Image image;
						try
						{
							image = (Image) Factory.LoadMediaObjectInstance(id, GalleryObjectType.Image, true);
						}
						catch (InvalidMediaObjectException)
						{
							continue; // Image may have been deleted by someone else, so just skip it.
						}

						image.DeleteHiResImage();

						GalleryObjectController.SaveGalleryObject(image);
					}

					if (idType == 'a')
					{
						DeleteHiResImagesFromAlbum(AlbumController.LoadAlbumInstance(id, true, true));
					}
				}
				HelperFunctions.CommitTransaction();
			}
			catch
			{
				HelperFunctions.RollbackTransaction();
				throw;
			}

			HelperFunctions.PurgeCache();

			return true;
		}

		private static void DeleteHiResImagesFromAlbum(IAlbum album)
		{
			// Delete the hi-res image for each image in the album. Then recursively do the same thing to all child albums.
			foreach (GalleryServerPro.Business.Image image in album.GetChildGalleryObjects(GalleryObjectType.Image))
			{
				image.DeleteHiResImage();

				GalleryObjectController.SaveGalleryObject(image);
			}

			foreach (IAlbum childAlbum in album.GetChildGalleryObjects(GalleryObjectType.Album))
			{
				DeleteHiResImagesFromAlbum(childAlbum);
			}
		}

		private string[] RetrieveUserSelections()
		{
			// Iterate through all the checkboxes, saving checked ones to an array. The gallery object IDs are stored 
			// in a hidden input tag. Albums have an 'a' prefix; images have a 'm' prefix (e.g. "a322", "m999")
			CheckBox chkbx;
			HiddenField gc;
			List<string> ids = new List<string>();

			// Loop through each item in the repeater control. If an item is checked, extract the ID.
			foreach (RepeaterItem rptrItem in rptr.Items)
			{
				chkbx = (CheckBox)rptrItem.FindControl("chkbx");

				if (chkbx.Checked)
				{
					// Checkbox is checked. Save media object ID to array.
					gc = (HiddenField)rptrItem.FindControl("hdn");

					ids.Add(gc.Value);
				}
			}

			// Convert the int array to an array of strings of exactly the right length.
			string[] idArray = new string[ids.Count];
			ids.CopyTo(idArray);

			return idArray;
		}

		#endregion
	}
}