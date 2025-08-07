using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.Category;

namespace CAR_RENTAL.Model.Repositories
{
    internal class CategoryRepository: IRepository<CategoryView>
    {
        private static CategoryRepository _instance = null;
        public static CategoryRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CategoryRepository();
                }
                return _instance;
            }
        }
        public CategoryView FindById(int id)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Category.FirstOrDefault(d => d.cate_id == id);
                if (item != null)
                {
                    CategoryView editInfo = new CategoryView();
                    editInfo.ID = item.cate_id;
                    editInfo.Title = item.title;
                    editInfo.Description = item.description;
                    editInfo.Active = item.active ?? 0;
                    return editInfo;
                }
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new CategoryView();
        }
        public void Create (CategoryView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = new tbl_Category { title = entity.Title, description = entity.Description, active = entity.Active };
                en.tbl_Category.Add(item);
                en.SaveChanges();
                entity.ID = item.cate_id;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
        public bool Update(CategoryView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var newUpdate = en.tbl_Category.Where(d => d.cate_id == entity.ID).FirstOrDefault();
                newUpdate.title = entity.Title;
                newUpdate.description = entity.Description;
                newUpdate.active = entity.Active;
                en.SaveChanges();
                return true;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            return false;
        }
        public bool Delete(CategoryView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var del = en.tbl_Category.FirstOrDefault(d => d.cate_id == entity.ID);
                if(del != null)
                {
                    en.tbl_Category.Remove(del);
                    en.SaveChanges();
                    return true;
                }
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public HashSet<CategoryView> GetAll()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = en.tbl_Category.
                    Select(d => new CategoryView
                    {
                        ID = d.cate_id,
                        Title = d.title,
                        Description = d.description,
                        Active = d.active ?? 0
                    }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CategoryView>();
        }
        public HashSet<CategoryView> GetAllPaging(int index =1, int pageSize = 10)
        {
            return new HashSet<CategoryView>();
        }
        public HashSet<CategoryView> FindAll(string filter)
        {
            return new HashSet<CategoryView>();
        }
        public HashSet<CategoryView> FindAllPaging(string filter, int index =1, int pageSize=10)
        {
            return new HashSet<CategoryView>();
        }

        //Query Report
        public HashSet<CategoryView> CountByStatus(string status)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = (from cate in en.tbl_Category
                            where cate.active == (status == "Active" ? 1 : 0)
                            select new CategoryView
                            {
                                ID = cate.cate_id
                            }).ToHashSet();
                return item;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CategoryView>();
        }


    }
}
