using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_AppSetting")]
	public class AppSettingDto
	{
		[Key]
		public virtual int AppSettingId
		{
			get;
			set;
		}

		public virtual string SettingName
		{
			get;
			set;
		}

		[MaxLength	]
		public virtual string SettingValue
		{
			get;
			set;
		}
	}
}
