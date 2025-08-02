using CAR_RENTAL.Views.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAR_RENTAL.Model.ModalViews.Admin;

namespace CAR_RENTAL.Components
{
    public class Session
    {
        public bool IsAdmin { get; set; }
        public AdminView CurrentUser { get; set; }
    }
}
