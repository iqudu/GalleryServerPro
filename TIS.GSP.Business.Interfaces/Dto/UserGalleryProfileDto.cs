using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_UserGalleryProfile")]
	public class UserGalleryProfileDto
	{
		[Key]
		public virtual int ProfileId
		{
			get;
			set;
		}

		public virtual string UserName
		{
			get;
			set;
		}

		public virtual int FKGalleryId
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
