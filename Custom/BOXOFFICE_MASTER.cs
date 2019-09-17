using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOfficeCroller
{
    public partial class BOXOFFICE_MASTER
    {
        [NotMapped]
        public List<BOXOFFICE_DETAIL> BoxOfficeDetails { get; set; }
    }
}
