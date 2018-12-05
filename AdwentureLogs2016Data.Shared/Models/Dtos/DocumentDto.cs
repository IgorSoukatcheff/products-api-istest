using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwentureLogs2016Data.Shared.Models.Dtos
{
    public class DocumentDto
    {
        public string DocumentName { get; set; }
        public string DocumentMetadata { get; set; }
        public string FileName { get; set; }
        public string AzureFileName { get; set; }
        public byte[] DocumentContent { get; set; }
        public int DocumentContentLenght { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
