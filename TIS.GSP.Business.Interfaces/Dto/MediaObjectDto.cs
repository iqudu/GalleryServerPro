using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_MediaObject")]
	public class MediaObjectDto
	{
		[Key]
		public virtual int MediaObjectId
		{
			get;
			set;
		}

		public virtual int FKAlbumId
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string Title
		{
			get;
			set;
		}

		public virtual string HashKey
		{
			get;
			set;
		}

		public virtual string ThumbnailFilename
		{
			get;
			set;
		}

		public virtual int ThumbnailWidth
		{
			get;
			set;
		}

		public virtual int ThumbnailHeight
		{
			get;
			set;
		}

		public virtual int ThumbnailSizeKB
		{
			get;
			set;
		}

		public virtual string OptimizedFilename
		{
			get;
			set;
		}

		public virtual int OptimizedWidth
		{
			get;
			set;
		}

		public virtual int OptimizedHeight
		{
			get;
			set;
		}

		public virtual int OptimizedSizeKB
		{
			get;
			set;
		}

		public virtual string OriginalFilename
		{
			get;
			set;
		}

		public virtual int OriginalWidth
		{
			get;
			set;
		}

		public virtual int OriginalHeight
		{
			get;
			set;
		}

		public virtual int OriginalSizeKB
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string ExternalHtmlSource
		{
			get;
			set;
		}

		public virtual string ExternalType
		{
			get;
			set;
		}

		public virtual int Seq
		{
			get;
			set;
		}

		public virtual string CreatedBy
		{
			get;
			set;
		}

		public virtual System.DateTime DateAdded
		{
			get;
			set;
		}

		public virtual string LastModifiedBy
		{
			get;
			set;
		}

		public virtual System.DateTime DateLastModified
		{
			get;
			set;
		}

		public virtual bool IsPrivate
		{
			get;
			set;
		}
	}
}
