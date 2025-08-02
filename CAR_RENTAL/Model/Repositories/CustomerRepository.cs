using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.Customer;
using CAR_RENTAL.Views.Customer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.Repositories
{
    internal class CustomerRepository: IRepository<CustomerView>
    {
        private static CustomerRepository _instance = null;
        public static CustomerRepository Instance
        {
            get
            {
                if( _instance == null)
                {
                    _instance = new CustomerRepository();
                }
                return _instance;
            }
        }
        public CustomerView FindById(int id)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from cus in en.tbl_Customer
                            join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                            from dis in disGroup.DefaultIfEmpty()
                            join ci in en.tbl_City on cus.city_id equals ci.city_id into ciGroup
                            from ci in ciGroup.DefaultIfEmpty()
                            where cus.cus_id == id
                            select new CustomerView
                            {
                                ID = cus.cus_id,
                                Name = cus.name,
                                Email = cus.email,
                                Password = cus.password,
                                Phone = cus.phone,
                                Address = cus.address,
                                CityId = cus.city_id,
                                DistrictId = cus.district_id,
                                CityName = ci.city_name,
                                DistrictName = dis.district_name,
                                Active = cus.active,
                                CusIdCard = cus.cus_id_card,
                                CreateDate = cus.create_date
                            }).FirstOrDefault();
                return item;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex);
            }
                
            return new CustomerView();
        }
        public HashSet<CustomerView> FindByIdNumber(string idNumber)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from cus in en.tbl_Customer
                            join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                            from dis in disGroup.DefaultIfEmpty()
                            join ci in en.tbl_City on cus.city_id equals ci.city_id into ciGroup
                            from ci in ciGroup.DefaultIfEmpty()
                            where cus.cus_id_card.Contains(idNumber) || cus.name.Contains(idNumber)
                            select new CustomerView
                            {
                                ID = cus.cus_id,
                                Name = cus.name,
                                Email = cus.email,
                                Password = cus.password,
                                Phone = cus.phone,
                                Address = cus.address,
                                CityId = cus.city_id,
                                DistrictId = cus.district_id,
                                CityName = ci.city_name,
                                DistrictName = dis.district_name,
                                Active = cus.active,
                                CusIdCard = cus.cus_id_card,
                                CreateDate = cus.create_date
                            }).ToHashSet();
                return item;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex);
            }

            return new HashSet<CustomerView>();
        }
        public bool Update(CustomerView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Customer.Where(d=>d.cus_id == entity.ID).FirstOrDefault();
                if (item != null)
                {
                    item.email = entity.Email;
                    item.password = entity.Password;
                    item.name = entity.Name;
                    item.phone = entity.Phone;
                    item.address = entity.Address;
                    item.district_id = entity.DistrictId;
                    item.city_id = entity.CityId;
                    item.active = entity.Active;
                    item.cus_id_card=entity.CusIdCard;
                    en.SaveChanges();
                    return true;
                }               
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }
        public bool Delete(CustomerView entity)
        {
            try
            {
                DbCarRental en =new DbCarRental();
                var item = en.tbl_Customer.Where(d=>d.cus_id==entity.ID).FirstOrDefault();
                if (item != null)
                {
                    en.tbl_Customer.Remove(item);
                    en.SaveChanges();
                    return true;
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public void Create(CustomerView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = new tbl_Customer
                {
                    name = entity.Name,
                    email = entity.Email,
                    password = entity.Password != null ? entity.Password : null,
                    phone = entity.Phone,
                    address = entity.Address,
                    active = entity.Active,
                    city_id = entity.CityId,
                    district_id = entity.DistrictId,              
                    cus_id_card = entity.CusIdCard,
                    create_date = entity.CreateDate
                };
                en.tbl_Customer.Add(item);
                en.SaveChanges();
                entity.ID = item.cus_id;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public HashSet<CustomerView> GetAll()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from cus in en.tbl_Customer
                          join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()

                          join ci in en.tbl_City on cus.city_id equals ci.city_id into ciGroup
                          from ci in ciGroup.DefaultIfEmpty()
                          select new CustomerView
                          {
                              ID = cus.cus_id,
                              Name = cus.name,
                              Email = cus.email,
                              Password = cus.password,
                              Phone = cus.phone,
                              Address = cus.address,
                              CityId = cus.city_id,
                              DistrictId = cus.district_id,
                              CityName = ci!=null ? ci.city_name : null,
                              DistrictName = dis!=null ? dis.district_name : null,
                              Active = cus.active,
                              CusIdCard = cus.cus_id_card,
                              CreateDate = cus.create_date
                          }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CustomerView>();
        }
        public HashSet<CustomerView> GetAllPaging(int index = 1, int pageSize = 10)
        {
            return new HashSet<CustomerView>();
        }
        public HashSet<CustomerView> FindAll(string filter)
        {
            return new HashSet<CustomerView>();
        }
        public HashSet<CustomerView> FindAllPaging(string filter, int indec =1, int pageSize = 10)
        {
            return new HashSet<CustomerView>();
        }
        public bool IsExistIdNumber(string IdNumber)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from cus in en.tbl_Customer
                            where cus.cus_id_card == IdNumber
                            select new CustomerView
                            {
                                ID = cus.cus_id
                            }).FirstOrDefault();
                if(item != null)
                {
                    return true;
                }
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public bool IsExistEmail(string Email)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from cus in en.tbl_Customer
                            where cus.email == Email
                            select new CustomerView
                            {
                                ID = cus.cus_id
                            }).FirstOrDefault();
                if ((item != null))
                {
                    return true;
                }
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public bool IsExistPhone(string phone)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from cus in en.tbl_Customer
                            where cus.phone == phone
                            select new CustomerView
                            {
                                ID = cus.cus_id
                            }).FirstOrDefault();
                if ((item != null))
                {
                    return true;
                }
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }

    }
}
