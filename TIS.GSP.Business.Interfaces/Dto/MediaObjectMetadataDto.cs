using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_MediaObjectMetadata")]
	public class MediaObjectMetadataDto
	{
		[Key]
		public virtual int MediaObjectMetadataId
		{
			get;
			set;
		}

		public virtual int FKMediaObjectId
		{
			get;
			set;
		}

		public virtual int MetadataNameIdentifier
		{
			get;
			set;
		}

		public virtual string Description
		{
			get;
			set;
		}

		[MaxLength]
		public virtual string Value
		{
			get;
			set;
		}
	}
}
