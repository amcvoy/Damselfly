﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Damselfly.Core.Utils.Images;

namespace Damselfly.Core.Models;

/// <summary>
/// An image, or photograph file on disk. Has a folder associated
/// with it. There's a BasketEntry which, if it exists, indicates
/// the picture is selected.
/// It also has a many-to-many relationship with IPTC keyword tags; so
/// a tag can apply to many images, and an image can have many tags.
/// </summary>
public class Image
{
    [Key]
    public int ImageId { get; set; }

    public int FolderId { get; set; }
    public virtual Folder Folder { get; set; }

    // Image File metadata
    public string FileName { get; set; }
    public int FileSizeBytes { get; set; }
    public DateTime FileCreationDate { get; set; }
    public DateTime FileLastModDate { get; set; }

    // Date used for search query orderby
    public DateTime SortDate { get; set; }

    // Damselfy state metadata
    public DateTime LastUpdated { get; set; }

    public virtual ImageMetaData MetaData { get; set; }
    public virtual Hash Hash { get; set; }

    // An image can appear in many baskets
    public virtual List<BasketEntry> BasketEntries { get; } = new List<BasketEntry>();
    // An image can have many tags
    public virtual List<ImageTag> ImageTags { get; } = new List<ImageTag>();

    // Machine learning fields
    public int? ClassificationId { get; set; }
    public virtual ImageClassification Classification { get; set; }
    public double ClassificationScore { get; set; }

    public virtual List<ImageObject> ImageObjects { get; } = new List<ImageObject>();

    public override string ToString()
    {
        return $"{FileName} [{ImageId}]";
    }

    [NotMapped]
    public string FullPath { get { return Path.Combine(Folder.Path, FileName); } }

    [NotMapped]
    public string RawImageUrl { get { return $"/rawimage/{ImageId}"; } }
    [NotMapped]
    public string DownloadImageUrl { get { return $"/dlimage/{ImageId}"; } }

    /// <summary>
    /// URL mapped with last-updated time to ensure we always refresh the thumb
    /// when the image is updated.
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public string ThumbUrl(ThumbSize size)
    {
        return $"/thumb/{size}/{this.ImageId}?nocache={this?.LastUpdated:yyyyMMddHHmmss}";
    }
    public void FlagForMetadataUpdate() { this.LastUpdated = DateTime.UtcNow; }
}

