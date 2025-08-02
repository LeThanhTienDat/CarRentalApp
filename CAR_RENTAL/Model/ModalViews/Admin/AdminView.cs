using CAR_RENTAL.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.ModalViews.Admin
{
    public class AdminView
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int Active { get; set; }
        public string Salt { get; set; }
    }
}
