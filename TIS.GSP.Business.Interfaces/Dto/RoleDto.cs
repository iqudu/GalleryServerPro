using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
	[Table("gs_Role")]
	public class RoleDto
	{
		[Key]
		public virtual string RoleName
		{
			get;
			set;
		}

		public virtual bool AllowViewAlbumsAndObjects
		{
			get;
			set;
		}

		public virtual bool AllowViewOriginalImage
		{
			get;
			set;
		}

		public virtual bool AllowAddChildAlbum
		{
			get;
			set;
		}

		public virtual bool AllowAddMediaObject
		{
			get;
			set;
		}

		public virtual bool AllowEditAlbum
		{
			get;
			set;
		}

		public virtual bool AllowEditMediaObject
		{
			get;
			set;
		}

		public virtual bool AllowDeleteChildAlbum
		{
			get;
			set;
		}

		public virtual bool AllowDeleteMediaObject
		{
			get;
			set;
		}

		public virtual bool AllowSynchronize
		{
			get;
			set;
		}

		public virtual bool HideWatermark
		{
			get;
			set;
		}

		public virtual bool AllowAdministerGallery
		{
			get;
			set;
		}

		public virtual bool AllowAdministerSite
		{
			get;
			set;
		}

		public virtual ICollection<RoleAlbumDto> RoleAlbums
		{
			get;
			set;
		}
	}
}
