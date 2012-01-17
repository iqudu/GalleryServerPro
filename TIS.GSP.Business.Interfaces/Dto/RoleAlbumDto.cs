using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_Role_Album")]
	public class RoleAlbumDto
	{
		[Key, Column(Order = 0)]
		public virtual string FKRoleName
		{
			get;
			set;
		}

		[Key, Column(Order = 1)]
		public virtual int FKAlbumId
		{
			get;
			set;
		}

		[ForeignKey("FKRoleName")]
		public virtual RoleDto Role
		{
			get;
			set;
		}

		[ForeignKey("FKAlbumId")]
		public virtual AlbumDto Album
		{
			get;
			set;
		}
	}
}
