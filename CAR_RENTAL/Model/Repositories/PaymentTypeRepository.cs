using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews;
using CAR_RENTAL.Model.ModalViews.PaymentType;

namespace CAR_RENTAL.Model.Repositories
{
    internal class PaymentTypeRepository:IRepository<PaymentTypeView>
    {
        private static PaymentTypeRepository _instance = null;
        public static PaymentTypeRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new PaymentTypeRepository();
                }
                return _instance;
            }
        }
        public PaymentTypeView FindById(int id)
        {
            return new PaymentTypeView();
        }
        public void Create(PaymentTypeView entity)
        {
            
        }
        public bool Update(PaymentTypeView entity)
        {
            return false;
        }
        public bool Delete(PaymentTypeView entity)
        {
            return false;
        }
        public HashSet<PaymentTypeView> GetAll()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from payment in en.tbl_Payment_type
                          select new PaymentTypeView
                          {
                              ID = payment.payment_type_id,
                              Name = payment.name
                          }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);    
            }
            return new HashSet<PaymentTypeView>();
        }
        public HashSet<PaymentTypeView> GetAllPaging(int index =1 , int pageSize = 10)
        {
            return new HashSet<PaymentTypeView>();
        }
        public HashSet<PaymentTypeView> FindAll(string filter)
        {
            return new HashSet<PaymentTypeView>();
        }
        public HashSet<PaymentTypeView> FindAllPaging(string filter, int index=1, int pageSize = 10)
        {
            return new HashSet<PaymentTypeView>();
        }

    }
}
