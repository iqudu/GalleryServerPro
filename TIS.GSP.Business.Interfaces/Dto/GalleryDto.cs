using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_Gallery")]
	public class GalleryDto
	{
		[Key]
		public virtual int GalleryId
		{
			get;
			set;
		}

		public virtual string Description
		{
			get;
			set;
		}

		public virtual System.DateTime DateAdded
		{
			get;
			set;
		}
	}
}
