using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.Admin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CAR_RENTAL.Model.Repositories
{
    internal class AdminRepository:IRepository<AdminView>
    {
        private static AdminRepository _instance = null;
        public static AdminRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new AdminRepository();
                }
                return _instance;
            }
        }
        public AdminView FindById(int id)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from ad in en.tbl_Admin
                            where ad.admin_id == id
                            select new AdminView
                            {
                                ID = ad.admin_id,
                                Phone = ad.phone,
                                Name = ad.name,
                                Email = ad.email,
                                Active = ad.active ?? 0,
                                Salt = ad.salt,
                            }).FirstOrDefault();
                return item;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new AdminView();
        }
        public void Create(AdminView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = new tbl_Admin
                {
                    name = entity.Name,
                    email = entity.Email,
                    phone = entity.Phone,
                    password = entity.Password,
                    active = entity.Active,
                    salt = entity.Salt
                };
                en.tbl_Admin.Add(item);
                en.SaveChanges();
                entity.ID = item.admin_id;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public bool Update(AdminView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Admin.Where(d => d.admin_id == entity.ID && d.email == entity.Email).FirstOrDefault();
                item.name = entity.Name;
                item.phone = entity.Phone;
                en.SaveChanges();
                return true;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public bool Delete(AdminView entity)
        {
            return false;
        }
        public HashSet<AdminView> GetAll()
        {
            return new HashSet<AdminView>();
        }
        public HashSet<AdminView> GetAllPaging(int index =1, int pageSize = 10)
        {
            return new HashSet<AdminView>();
        }
        public HashSet<AdminView> FindAll(string filter)
        {
            return new HashSet<AdminView>();
        }
        public HashSet<AdminView> FindAllPaging(string filter, int index = 1, int pageSize = 10)
        {
            return new HashSet<AdminView>();
        }
        public AdminView FindByEmail(string email)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from a in en.tbl_Admin
                            where a.email == email
                            select new AdminView
                            {
                                ID = a.admin_id,
                                Name = a.name,
                                Email = a.email,
                                Phone = a.phone,
                                Password = a.password,
                                Salt = a.salt,
                                Active = a.active ?? 0
                            }).FirstOrDefault();
                return item;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new AdminView();
        }
        public bool UpdatePassword(AdminView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Admin.Where(d => d.email == entity.Email && d.admin_id == entity.ID).FirstOrDefault();
                item.password = entity.Password;
                en.SaveChanges();
                return true;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public AdminView CheckAccount(string phone, string email)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from ad in en.tbl_Admin
                            where ad.email == email && ad.phone == phone
                            select new AdminView
                            {
                                ID = ad.admin_id,
                                Email = ad.email,
                                Salt = ad.salt
                            }).FirstOrDefault();
                if(item != null)
                {
                    return item;
                }
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return new AdminView();
        }
        public bool UpdateForgotPassword(AdminView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Admin.Where(d=> d.admin_id == entity.ID && d.email == entity.Email).FirstOrDefault();
                item.password = entity.Password;
                en.SaveChanges();
                return true;
            }
            catch(EntityException ex)
            {
                Debug.Write(ex.Message);
            }
            return false;
        }
    }

}
