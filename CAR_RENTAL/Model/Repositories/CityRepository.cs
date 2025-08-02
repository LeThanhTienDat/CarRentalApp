using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.City;

namespace CAR_RENTAL.Model.Repositories
{
    internal class CityRepository
    {
        private static CityRepository _instance = null;
        public static CityRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CityRepository();
                }
                return _instance;
            }
        }
        public CityRepository FindById(int id)
        {
            return new CityRepository();
        }

        public void Create()
        {

        }
        public bool Update(CityView entity)
        {
            return false;
        }
        public bool Delete(CityView entity)
        {
            return false;
        }
        public HashSet<CityView> GetAll()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from city in en.tbl_City
                          select new CityView
                          {
                              ID = city.city_id,
                              Name = city.city_name
                          }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CityView>();
        }
        public HashSet<CityView> GetAllPaging(int index=1, int pageSize = 10)
        {
            return new HashSet<CityView>();
        }
        public HashSet<CityView> FindAll(string filter)
        {
            return new HashSet<CityView>();
        }
        public HashSet<CityView> FindAllPaging(string filter, int index =1, int pageSize = 10)
        {
            return new HashSet<CityView>();
        }
    }
}
