using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.District;

namespace CAR_RENTAL.Model.Repositories
{
    internal class DistrictRepository
    {
        private static DistrictRepository _instance = null;
        public static DistrictRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DistrictRepository();
                }
                return _instance;
            }
        }
        public DistrictView FindById(int id)
        {
            return new DistrictView();
        }
        public void Create()
        {

        }
        public bool Update(DistrictView entity)
        {
            return false;
        }
        public bool Delete(DistrictView entity)
        {
            return false;
        }
        public HashSet<DistrictView> GetAll()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from dist in en.tbl_District
                          select new DistrictView
                          {
                              ID = dist.district_id,
                              Name = dist.district_name,
                              CityId =dist.city_id
                          }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);    
            }
            return new HashSet<DistrictView>();
        }
        public HashSet<DistrictView> GetAllPaging(int index=1, int pageSize = 10)
        {
            return new HashSet<DistrictView>();
        }
        public HashSet<DistrictView> FindAll(string filter)
        {
            return new HashSet<DistrictView>();
        }
        public HashSet<DistrictView> FindAllPaging(string filter, int index=1, int pageSize = 10)
        {
            return new HashSet<DistrictView>();
        }
        public HashSet<DistrictView> FindByCityId(int cityId)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from dist in en.tbl_District
                          where dist.city_id == cityId
                          select new DistrictView
                          {
                              ID = dist.district_id,
                              Name = dist.district_name,
                              CityId = dist.city_id
                          }).ToHashSet();
                return rs;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<DistrictView>();
        }
        
    }
}
