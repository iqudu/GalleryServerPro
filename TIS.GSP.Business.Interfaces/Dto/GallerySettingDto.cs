using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_GallerySetting")]
	public class GallerySettingDto
	{
		[Key]
		public virtual int GallerySettingId
		{
			get;
			set;
		}

		public virtual int FKGalleryId
		{
			get;
			set;
		}

		public virtual bool IsTemplate
		{
			get;
			set;
		}

		public virtual string SettingName
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string SettingValue
		{
			get;
			set;
		}
	}
}
