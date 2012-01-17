using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_AppError")]
	public class AppErrorDto
	{
		[Key]
		public virtual int AppErrorId
		{
			get;
			set;
		}

		public virtual int FKGalleryId
		{
			get;
			set;
		}

		public virtual System.DateTime TimeStamp
		{
			get;
			set;
		}

		public virtual string ExceptionType
		{
			get;
			set;
		}

		public virtual string Message
		{
			get;
			set;
		}

		public virtual string Source
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string TargetSite
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string StackTrace
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string ExceptionData
		{
			get;
			set;
		}

		public virtual string InnerExType
		{
			get;
			set;
		}

		public virtual string InnerExMessage
		{
			get;
			set;
		}

		public virtual string InnerExSource
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string InnerExTargetSite
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string InnerExStackTrace
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string InnerExData
		{
			get;
			set;
		}

		public virtual string Url
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string FormVariables
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string Cookies
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string SessionVariables
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string ServerVariables
		{
			get;
			set;
		}
	}
}
