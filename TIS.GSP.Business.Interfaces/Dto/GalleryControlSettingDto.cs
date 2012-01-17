using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_GalleryControlSetting")]
	public class GalleryControlSettingDto
	{
		[Key]
		public virtual int GalleryControlSettingId
		{
			get;
			set;
		}

		public virtual string ControlId
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
