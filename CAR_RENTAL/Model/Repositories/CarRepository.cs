using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.Car;
using CAR_RENTAL.Views.Car;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CAR_RENTAL.Model.Repositories
{
    internal class CarRepository: IRepository<CarView>
    {
        private static CarRepository _instance = null;
        public static CarRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CarRepository();
                }
                return _instance;
            }
        }
        public CarView FindById(int id)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from car in en.tbl_Car
                          join cate in en.tbl_Category on car.cate_id equals cate.cate_id
                          join cartype in en.tbl_Car_type on car.car_type_id equals cartype.car_type_id
                          join city in en.tbl_City on car.city_id equals city.city_id into cityGroup
                          from city in cityGroup.DefaultIfEmpty()
                          join dis in en.tbl_District on car.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          where car.car_id == id
                          select new CarView
                          {
                              ID = car.car_id,
                              Brand = car.brand,
                              Model = car.model,
                              PricePerDay = car.price_per_day ?? 0,
                              CarStatus = car.car_status,
                              Image = car.image,
                              LicensePlate = car.license_plate,
                              SeatCount = car.seat_count ?? 0,
                              Color = car.color,
                              CarTypeId = car.car_type_id ?? 0,
                              CategoryName = cate.title,
                              CarTypeName = cartype.car_type_name,
                              Active = car.active ?? 0,
                              Address = car.address,
                              CityId = car.city_id ?? 0,
                              DistrictId = car.district_id ?? 0,
                              CityName = city.city_name ?? null,
                              DistrictName = dis.district_name ?? null,
                          }).FirstOrDefault();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new CarView();
        }
        public void Create(CarView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = new tbl_Car
                {
                    brand = entity.Brand,
                    cate_id = entity.CateId,
                    model = entity.Model,
                    price_per_day = entity.PricePerDay,
                    car_status = entity.CarStatus,
                    image = entity.Image,
                    license_plate = entity.LicensePlate,
                    seat_count = entity.SeatCount,
                    color = entity.Color,
                    car_type_id = entity.CarTypeId,
                    active = entity.Active ?? 0,
                    address = entity.Address,
                    city_id = entity.CityId,
                    district_id = entity.DistrictId
                };
                en.tbl_Car.Add(item);
                en.SaveChanges();
                entity.ID = item.car_id;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
        public bool Update(CarView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Car.Where(d => d.car_id == entity.ID).FirstOrDefault();
                item.brand = entity.Brand;
                item.model = entity.Model;
                item.cate_id = entity.CateId;
                item.price_per_day = entity.PricePerDay;
                item.car_status = entity.CarStatus;
                item.image = entity.Image;
                item.license_plate = entity.LicensePlate;
                item.seat_count = entity.SeatCount;
                item.color = entity.Color;
                item.car_type_id = entity.CarTypeId;
                item.active = entity.Active;
                item.city_id = entity.CityId;
                item.district_id = entity.DistrictId;
                item.address = entity.Address;
                en.SaveChanges();
                return true;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public bool Delete(CarView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Car.Where(d => d.car_id == entity.ID).FirstOrDefault();
                if(item != null)
                {
                    en.tbl_Car.Remove(item);
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
        public HashSet<CarView> GetAll()
        {
            return new HashSet<CarView>();
        }
        public HashSet<CarView> GetAll (string brand = null, 
                                        string model = null, 
                                        string color = null, 
                                        string licensePlate = null, 
                                        int? seatCount = null,
                                        string status = null,
                                        string category = null,
                                        string type = null,
                                        decimal? price =null)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from car in en.tbl_Car
                           join cate in en.tbl_Category on car.cate_id equals cate.cate_id
                           join cartype in en.tbl_Car_type on car.car_type_id equals cartype.car_type_id
                           join city in en.tbl_City on car.city_id equals city.city_id into cityGroup
                           from city in cityGroup.DefaultIfEmpty()
                           join dis in en.tbl_District on car.district_id equals dis.district_id into disGroup
                           from dis in disGroup.DefaultIfEmpty()
                           orderby car.car_id descending
                           select new CarView
                           {
                               ID = car.car_id,
                               Brand = car.brand,
                               Model = car.model,
                               PricePerDay = car.price_per_day ?? 0,
                               CarStatus = car.car_status,
                               Image = car.image,
                               LicensePlate = car.license_plate,
                               SeatCount = car.seat_count ?? 0,
                               Color = car.color,
                               CarTypeId = car.car_type_id ?? 0,
                               CategoryName = cate.title,
                               CarTypeName = cartype.car_type_name,
                               Active = car.active ?? 0
                           });
                if (!string.IsNullOrWhiteSpace(brand)) rs=rs.Where(x =>x.Brand.ToLower().Contains(brand.ToLower()));
                if (!string.IsNullOrWhiteSpace(model)) rs=rs.Where(x=>x.Model.ToLower().Contains(model.ToLower()));
                if (!string.IsNullOrWhiteSpace(color)) rs = rs.Where(x => x.Color.ToLower().Contains(color.ToLower()));
                if (!string.IsNullOrWhiteSpace(licensePlate)) rs = rs.Where(x => x.LicensePlate.ToLower().Contains(licensePlate.ToLower()));
                if (seatCount != null) rs = rs.Where(x => x.SeatCount == seatCount);
                if (!string.IsNullOrWhiteSpace(status)) rs = rs.Where(x => x.CarStatus.ToLower().Contains(status.ToLower()));
                if (!string.IsNullOrWhiteSpace(category)) rs = rs.Where(x => x.CategoryName.ToLower().Contains(category.ToLower()));
                if (!string.IsNullOrWhiteSpace(type)) rs = rs.Where(x => x.CarTypeName.ToLower().Contains(type.ToLower()));
                if (price != null) rs = rs.Where(x => x.PricePerDay >= (price.Value - 100000) && x.PricePerDay <= (price.Value + 100000));                           
                return rs.ToHashSet();
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CarView>();
        }
        public HashSet<CarView> GetAllPaging(int index =1, int pageSize = 10)
        {
            return new HashSet<CarView>();
        }
        public HashSet<CarView> FindAll(string filter)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from car in en.tbl_Car
                          join cate in en.tbl_Category on car.cate_id equals cate.cate_id
                          join cartype in en.tbl_Car_type on car.car_type_id equals cartype.car_type_id
                          join city in en.tbl_City on car.city_id equals city.city_id into cityGroup
                          from city in cityGroup.DefaultIfEmpty()
                          join dis in en.tbl_District on car.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          where car.brand.Contains(filter) || 
                                car.model.Contains(filter) || 
                                car.address.Contains(filter) || 
                                dis.district_name.Contains(filter) ||
                                city.city_name.Contains(filter)
                          select new CarView
                          {
                              ID = car.car_id,
                              Brand = car.brand,
                              Model = car.model,
                              PricePerDay = car.price_per_day ?? 0,
                              CarStatus = car.car_status,
                              Image = car.image,
                              LicensePlate = car.license_plate,
                              SeatCount = car.seat_count ?? 0,
                              Color = car.color,
                              CarTypeId = car.car_type_id ?? 0,
                              CategoryName = cate.title,
                              CarTypeName = cartype.car_type_name,
                              Active = car.active ?? 0,
                              Address = car.address,
                              CityId = car.city_id ?? 0,
                              DistrictId = car.district_id ?? 0,
                              CityName = city.city_name ?? null,
                              DistrictName = dis.district_name ?? null,
                          }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CarView>();
        }
        public HashSet<CarView> FindAllPaging(string filter, int index = 1, int pageSize = 10)
        {
            return new HashSet<CarView>();
        }
        public HashSet<CarView> FindAllWaiting(string filter)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from car in en.tbl_Car
                          join cate in en.tbl_Category on car.cate_id equals cate.cate_id
                          join cartype in en.tbl_Car_type on car.car_type_id equals cartype.car_type_id
                          join city in en.tbl_City on car.city_id equals city.city_id into cityGroup
                          from city in cityGroup.DefaultIfEmpty()
                          join dis in en.tbl_District on car.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          where car.car_status == filter
                          select new CarView
                          {
                              ID = car.car_id,
                              Brand = car.brand,
                              Model = car.model,
                              PricePerDay = car.price_per_day ?? 0,
                              CarStatus = car.car_status,
                              Image = car.image,
                              LicensePlate = car.license_plate,
                              SeatCount = car.seat_count ?? 0,
                              Color = car.color,
                              CarTypeId = car.car_type_id ?? 0,
                              CategoryName = cate.title,
                              CarTypeName = cartype.car_type_name,
                              Active = car.active ?? 0,
                              Address = car.address,
                              CityId = car.city_id ?? 0,
                              DistrictId = car.district_id ?? 0,
                              CityName = city.city_name ?? null,
                              DistrictName = dis.district_name ?? null,
                          }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CarView>();
        }
        public HashSet<CarView> FindByCategory(string category)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from car in en.tbl_Car
                          join cate in en.tbl_Category on car.cate_id equals cate.cate_id
                          join cartype in en.tbl_Car_type on car.car_type_id equals cartype.car_type_id
                          join city in en.tbl_City on car.city_id equals city.city_id into cityGroup
                          from city in cityGroup.DefaultIfEmpty()
                          join dis in en.tbl_District on car.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          select new CarView
                          {
                              ID = car.car_id,
                              Brand = car.brand,
                              Model = car.model,
                              PricePerDay = car.price_per_day ?? 0,
                              CarStatus = car.car_status,
                              Image = car.image,
                              LicensePlate = car.license_plate,
                              SeatCount = car.seat_count ?? 0,
                              Color = car.color,
                              CarTypeId = car.car_type_id ?? 0,
                              CategoryName = cate.title,
                              CarTypeName = cartype.car_type_name,
                              Active = car.active ?? 0,
                              Address = car.address,
                              CityId = car.city_id ?? 0,
                              DistrictId = car.district_id ?? 0,
                              CityName = city.city_name ?? null,
                              DistrictName = dis.district_name ?? null,
                          });
                if(category != "All")
                {
                    rs = rs.Where(d => d.CategoryName == category && d.CarStatus == "Waiting");
                }
                else
                {
                    rs = rs.Where(d => d.CarStatus == "Waiting");
                }
                    return rs.ToHashSet();
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CarView>();
        }
        public bool UpdateStatusCar(int? car_id, string status)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Car.Where(d => d.car_id == car_id).FirstOrDefault();
                if (item != null)
                {
                    item.car_status = status;
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
        public bool UpdateActive(CarView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Car.Where(d => d.car_id == entity.ID).FirstOrDefault();
                item.active = entity.Active;
                en.SaveChanges();
                return true;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }


        //Report Query
        public int CountAllCar()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var totalCar = (from c in en.tbl_Car
                                where c.active == 1
                                select c.car_id).Count();
                return totalCar;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }

        public int CountOnHiring()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var onHiring = (from c in en.tbl_Car
                                where c.active == 1 && c.car_status == "Booked"
                                select c.car_id).Count();
                return onHiring;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }

        public int CountWaiting()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var totalWaiting = (from c in en.tbl_Car
                                    where c.active == 1 && c.car_status == "Waiting"
                                    select c.car_id).Count();
                return totalWaiting;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }
        public int CountMaintaining()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var totalMaintaining = (from c in en.tbl_Car
                                        where c.active == 1 && c.car_status == "Maintain"
                                        select c.car_id).Count();
                return totalMaintaining;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }
        public int CountDeactive()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var totalDeactive = (from c in en.tbl_Car
                                     where c.active != 1
                                     select c.car_id).Count();
                return totalDeactive;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }
        public CarView FindMostRented()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rentCount = (from bd in en.tbl_Booking_details
                                 group bd by bd.car_id into g
                                 select new
                                 {
                                     CarId = g.Key,
                                     RentCount = g.Count()
                                 }).ToList();
                int maxCount = rentCount.Max(x => x.RentCount);
                var item = (from rc in rentCount
                            where rc.RentCount == maxCount
                            join c in en.tbl_Car on rc.CarId equals c.car_id
                            join ctype in en.tbl_Car_type on c.car_type_id equals ctype.car_type_id
                            join cate in en.tbl_Category on c.cate_id equals cate.cate_id
                            join city in en.tbl_City on c.city_id equals city.city_id into cityGroup
                            from city in cityGroup.DefaultIfEmpty()
                            join dis in en.tbl_District on c.district_id equals dis.district_id into disGroup
                            from dis in disGroup.DefaultIfEmpty()
                            orderby rc.RentCount descending
                            select new CarView
                            {
                                ID = c.car_id,
                                CateId = c.cate_id,
                                Brand = c.brand,
                                Model = c.model,
                                PricePerDay = c.price_per_day ?? 0,
                                CarStatus = c.car_status,
                                Image = c.image,
                                LicensePlate = c.license_plate,
                                SeatCount = c.seat_count ?? 0,
                                Color = c.color,
                                Active = c.active,
                                CarTypeId = c.car_type_id,
                                CarTypeName = ctype.car_type_name,
                                CategoryName = cate.title,
                                RentCount = rc.RentCount,
                                Address = c.address,
                                CityId = c.city_id ?? 0,
                                DistrictId = c.district_id ?? 0,
                                CityName = city.city_name ?? null,
                                DistrictName = dis.district_name ?? null,
                            }).FirstOrDefault();
                return item;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new CarView();
        }
        public CarView FindLeastRented()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rentCount = (from bd in en.tbl_Booking_details
                                 group bd by bd.car_id into g
                                 select new
                                 {
                                     CarId = g.Key,
                                     RentCount = g.Count()
                                 }).ToList();
                int minCount = rentCount.Min(x => x.RentCount);
                var item = (from rc in rentCount
                            where rc.RentCount == minCount
                            join c in en.tbl_Car on rc.CarId equals c.car_id
                            join ctype in en.tbl_Car_type on c.car_type_id equals ctype.car_type_id
                            join cate in en.tbl_Category on c.cate_id equals cate.cate_id
                            join city in en.tbl_City on c.city_id equals city.city_id into cityGroup
                            from city in cityGroup.DefaultIfEmpty()
                            join dis in en.tbl_District on c.district_id equals dis.district_id into disGroup
                            from dis in disGroup.DefaultIfEmpty()
                            orderby rc.RentCount ascending
                            select new CarView
                            {
                                ID = c.car_id,
                                CateId = c.cate_id,
                                Brand = c.brand,
                                Model = c.model,
                                PricePerDay = c.price_per_day ?? 0,
                                CarStatus = c.car_status,
                                Image = c.image,
                                LicensePlate = c.license_plate,
                                SeatCount = c.seat_count ?? 0,
                                Color = c.color,
                                Active = c.active,
                                CarTypeId = c.car_type_id,
                                CarTypeName = ctype.car_type_name,
                                CategoryName = cate.title,
                                RentCount = rc.RentCount,
                                Address = c.address,
                                CityId = c.city_id ?? 0,
                                DistrictId = c.district_id ?? 0,
                                CityName = city.city_name ?? null,
                                DistrictName = dis.district_name ?? null,
                            }).FirstOrDefault();
                return item;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new CarView();
        }

    }
}
