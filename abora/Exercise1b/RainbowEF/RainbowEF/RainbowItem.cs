using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RainbowEF
{
    class RainbowItem
    {
        public int Id { get; set; }
        public string Password { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(32)]
        [Index("index_md5",IsClustered = false)]
        public string Md5Hash { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        [Index("index_sha1", IsClustered = false)]
        public string Sha1Hash { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        [Index("index_sha256", IsClustered = false)]
        public string Sha256Hash { get; set; }

    }
}
