using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_BrowserTemplate")]
	public class BrowserTemplateDto
	{
		[Key]
		public virtual int BrowserTemplateId
		{
			get;
			set;
		}

		public virtual string MimeType
		{
			get;
			set;
		}

		public virtual string BrowserId
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string HtmlTemplate
		{
			get;
			set;
		}

		[MaxLength	]
		public virtual string ScriptTemplate
		{
			get;
			set;
		}
	}
}
