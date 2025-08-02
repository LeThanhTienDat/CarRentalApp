using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.ModalViews.District
{
    internal class DistrictView
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public int? CityId { set; get; }
    }
}
