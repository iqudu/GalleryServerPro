using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_Synchronize")]
	public class SynchronizeDto
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public virtual int FKGalleryId
		{
			get;
			set;
		}

		public virtual string SynchId
		{
			get;
			set;
		}

		public virtual int SynchState
		{
			get;
			set;
		}

		public virtual int TotalFiles
		{
			get;
			set;
		}

		public virtual int CurrentFileIndex
		{
			get;
			set;
		}
	}
}
