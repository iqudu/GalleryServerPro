using System.ComponentModel.DataAnnotations;

namespace GalleryServerPro.Data
{
  [Table("gs_MimeTypeGallery")]
  public class MimeTypeGalleryDto
  {
    [Key]
    public virtual int MimeTypeGalleryId
    {
      get;
      set;
    }

    public virtual int FKGalleryId
    {
      get;
      set;
    }

    public virtual int FKMimeTypeId
    {
      get;
      set;
    }

    public virtual bool IsEnabled
    {
      get;
      set;
    }

    [ForeignKey("FKMimeTypeId")]
    public virtual MimeTypeDto MimeType
    {
      get; 
      set;
    }
  }
}
