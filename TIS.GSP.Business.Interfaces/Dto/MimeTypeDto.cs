using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_MimeType")]
	public class MimeTypeDto
	{
		[Key]
		public virtual int MimeTypeId
		{
			get;
			set;
		}

		public virtual string FileExtension
		{
			get;
			set;
		}

		public virtual string MimeTypeValue
		{
			get;
			set;
		}

		public virtual string BrowserMimeTypeValue
		{
			get;
			set;
		}
	}
}
