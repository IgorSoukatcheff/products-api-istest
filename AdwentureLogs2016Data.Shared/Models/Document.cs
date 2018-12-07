using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdwentureLogs2016Data.Shared.Models
{
    public partial class Document
    {
        [Key]
        public SqlHierarchyId DocumentNode { get; set; }
        public string Title { get; set; }

        public int Owner { get; set; }
        public Employee OwnerEmployee { get; set; }
        public bool FolderFlag { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Revision { get; set; }
        public int ChangeNumber { get; set; }
        public int Status { get; set; }
        public string DocumentSummary { get; set; }
        [Column("Document") ]
        public byte[] DocumentContent { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
