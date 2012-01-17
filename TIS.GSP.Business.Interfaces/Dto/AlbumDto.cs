using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_Album")]
	public class AlbumDto
	{
		[Key]
		public virtual int AlbumId
		{
			get;
			set;
		}

		public virtual int FKGalleryId
		{
			get;
			set;
		}

		public virtual int AlbumParentId
		{
			get;
			set;
		}

		public virtual string Title
		{
			get;
			set;
		}

		public virtual string DirectoryName
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string Summary
		{
			get;
			set;
		}

		public virtual int ThumbnailMediaObjectId
		{
			get;
			set;
		}

		public virtual int Seq
		{
			get;
			set;
		}

		public virtual System.DateTime? DateStart
		{
			get;
			set;
		}

		public virtual System.DateTime? DateEnd
		{
			get;
			set;
		}

		public virtual System.DateTime DateAdded
		{
			get;
			set;
		}

		public virtual string CreatedBy
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

		public virtual string OwnedBy
		{
			get;
			set;
		}

		public virtual string OwnerRoleName
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
