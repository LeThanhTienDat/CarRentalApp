using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.CarType;
using CAR_RENTAL.Model.ModalViews.Category;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.Repositories
{
    internal class CarTypeRepository: IRepository<CarTypeView>
    {
        private static CarTypeRepository _instance;
        public static CarTypeRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CarTypeRepository();                    
                }
                return _instance;
            }
        }
        public CarTypeView FindById(int id)
        {
            return new CarTypeView();
        }
        public void Create(CarTypeView entity)
        {

        }
        public bool Update(CarTypeView entity)
        {
            return false;
        }
        public bool Delete(CarTypeView entity)
        {
            return false;
        }
        public HashSet<CarTypeView> GetAll()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = en.tbl_Car_type.
                    Select(d => new CarTypeView
                    {
                        ID = d.car_type_id,
                        Name = d.car_type_name
                    }).ToHashSet();
                return rs;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CarTypeView>();
        }
        public HashSet<CarTypeView> GetAllPaging(int index =1, int PageSize = 10)
        {
            return new HashSet<CarTypeView>();
        }
        public HashSet<CarTypeView> FindAll(string filter)
        {
            return new HashSet<CarTypeView>();  
        }
        public HashSet<CarTypeView>FindAllPaging(string filter, int index=1, int pageSize=10)
        {
            return new HashSet<CarTypeView>();
        }


    }
}
