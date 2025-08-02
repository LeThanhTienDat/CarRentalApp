using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.ModalViews.Customer;
using CAR_RENTAL.Views.Car;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.Repositories
{
    internal class BookingRepository:IRepository<BookingView>
    {
        private static BookingRepository _instance = null;
        public static BookingRepository Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new BookingRepository();
                }
                return _instance;
            }
        }public BookingView FindById(int id)
        {
            return new BookingView();
        }
        public BookingView FindById(int? id = null)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rawItems = (from b in en.tbl_Booking
                                join cus in en.tbl_Customer on b.cus_id equals cus.cus_id into cusGroup
                                from cus in cusGroup.DefaultIfEmpty()
                                join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                                from dis in disGroup.DefaultIfEmpty()
                                join city in en.tbl_City on cus.city_id equals city.city_id into cityGroup
                                from city in cityGroup.DefaultIfEmpty()
                                join bd in en.tbl_Booking_details on b.booking_id equals bd.booking_id into bdGroup
                                from bd in bdGroup.DefaultIfEmpty()
                                join car in en.tbl_Car on bd.car_id equals car.car_id into carGroup
                                from car in carGroup.DefaultIfEmpty()
                                join ctype in en.tbl_Car_type on car.car_type_id equals ctype.car_type_id into carTypeGroup
                                from ctype in carTypeGroup.DefaultIfEmpty()
                                join cate in en.tbl_Category on car.cate_id equals cate.cate_id into cateGroup
                                from cate in cateGroup.DefaultIfEmpty()
                                join pt in en.tbl_Payment_type on b.payment_type_id equals pt.payment_type_id into ptGroup
                                from pt in ptGroup.DefaultIfEmpty()
                                where cus.cus_id == id && b.booking_status == "Waiting" && b.is_cancel != 1
                                select new
                                {
                                    b,
                                    cus,
                                    dis,
                                    city,
                                    bd,
                                    car,
                                    ctype,
                                    cate,
                                    pt
                                }).ToList();
                
                var items = rawItems
                             .GroupBy(d => d.b.booking_id)
                             .Select(g =>
                             {
                                 var d = g.First();
                                 return new BookingView
                                 {
                                     ID = d.b.booking_id,
                                     CusId = d.b.cus_id,
                                     BookingStatus = d.b.booking_status,
                                     Discount = d.b.discount,
                                     Deposit = d.b.deposit,
                                     PaymentTypeId = d.b.payment_type_id,
                                     PaymentTypeName = d.pt.name,
                                     TotalPrice = d.b.total_price ?? 0,
                                     DepositCash = d.b.deposit_cash ?? 0,
                                     OrderDate = d.b.order_date,
                                     Paid = d.b.paid,
                                     CustomerView = new ModalViews.Customer.CustomerView
                                     {
                                         Name = d.cus != null ? d.cus.name : null,
                                         Email = d.cus.email,
                                         Password = d.cus.password,
                                         Phone = d.cus.phone,
                                         Address = d.cus.address,
                                         CityId = d.cus.city_id,
                                         CityName = d.city.city_name,
                                         DistrictId = d.cus.district_id,
                                         DistrictName = d.dis != null ? d.dis.district_name : null,
                                         Active = d.cus.active,
                                         CusIdCard = d.cus.cus_id_card,
                                         CreateDate = d.cus.create_date
                                     },
                                     BookingDetailsView = g
                                        .Where(x => x.bd != null)
                                        .Select(x => new ModalViews.BookingDetails.BookingDetailsView
                                        {
                                            CarId = x.bd.car_id,
                                            BookingDetailsStatus = x.bd.booking_details_status,
                                            BookingDate = x.bd.booking_date,
                                            StartDate = x.bd.start_date,
                                            EndDate = x.bd.end_date,
                                            ActualReturnDate = x.bd.actual_return_date,
                                            PricePerCar = x.bd != null ? x.bd.price_per_car : 0,
                                            Fine = x.bd.fine,
                                            Refund = x.bd.refund,
                                            StatusReturn = x.bd.status_return,
                                            CarView = new ModalViews.Car.CarView
                                            {
                                                CateId = x.car.cate_id,
                                                Brand = x.car != null ? x.car.brand : null,
                                                Model = x.car.model,
                                                PricePerDay = x.car.price_per_day ?? 0,
                                                CarStatus = x.car.car_status,
                                                Image = x.car.image,
                                                LicensePlate = x.car.license_plate,
                                                SeatCount = x.car.seat_count,
                                                Color = x.car.color,
                                                CarTypeId = x.car.car_type_id,
                                                CarTypeName = x.ctype.car_type_name,
                                                Active = x.car.active,
                                                CategoryName = x.cate.title
                                            }
                                        }).ToHashSet()
                                 };
                             }).FirstOrDefault();
                return items;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex);
            }
            return new BookingView();
        }
        public BookingView FindByBookingId(int? id = null)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rawItems = (from b in en.tbl_Booking
                                join cus in en.tbl_Customer on b.cus_id equals cus.cus_id into cusGroup
                                from cus in cusGroup.DefaultIfEmpty()
                                join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                                from dis in disGroup.DefaultIfEmpty()
                                join city in en.tbl_City on cus.city_id equals city.city_id into cityGroup
                                from city in cityGroup.DefaultIfEmpty()
                                join bd in en.tbl_Booking_details on b.booking_id equals bd.booking_id into bdGroup
                                from bd in bdGroup.DefaultIfEmpty()
                                join car in en.tbl_Car on bd.car_id equals car.car_id into carGroup
                                from car in carGroup.DefaultIfEmpty()
                                join ctype in en.tbl_Car_type on car.car_type_id equals ctype.car_type_id into carTypeGroup
                                from ctype in carTypeGroup.DefaultIfEmpty()
                                join cate in en.tbl_Category on car.cate_id equals cate.cate_id into cateGroup
                                from cate in cateGroup.DefaultIfEmpty()
                                join pt in en.tbl_Payment_type on b.payment_type_id equals pt.payment_type_id into ptGroup
                                from pt in ptGroup.DefaultIfEmpty()
                                where b.booking_id == id && b.booking_status == "Waiting" && b.is_cancel != 1
                                select new
                                {
                                    b,
                                    cus,
                                    dis,
                                    city,
                                    bd,
                                    car,
                                    ctype,
                                    cate,
                                    pt
                                }).ToList();

                var items = rawItems
                             .GroupBy(d => d.b.booking_id)
                             .Select(g =>
                             {
                                 var d = g.First();
                                 return new BookingView
                                 {
                                     ID = d.b.booking_id,
                                     CusId = d.b.cus_id,
                                     BookingStatus = d.b.booking_status,
                                     Discount = d.b.discount,
                                     Deposit = d.b.deposit,
                                     PaymentTypeId = d.b.payment_type_id,
                                     PaymentTypeName = d.pt.name,
                                     DepositCash = d.b.deposit_cash ?? 0,
                                     DepositHasPaid = d.b.deposit_cash_has_paid ?? 0,
                                     TotalPrice = d.b.total_price ?? 0,
                                     OrderDate = d.b.order_date,
                                     PaymentConfirm = d.b.payment_confirm ?? 0,
                                     IsCancel = d.b.is_cancel ?? 0,
                                     ReasonCancel = d.b.reason_cancel,
                                     CustomerView = new ModalViews.Customer.CustomerView
                                     {
                                         Name = d.cus != null ? d.cus.name : null,
                                         Email = d.cus.email,
                                         Password = d.cus.password,
                                         Phone = d.cus.phone,
                                         Address = d.cus.address,
                                         CityId = d.cus.city_id,
                                         CityName = d.city.city_name,
                                         DistrictId = d.cus.district_id,
                                         DistrictName = d.dis != null ? d.dis.district_name : null,
                                         Active = d.cus.active,
                                         CusIdCard = d.cus.cus_id_card,
                                         CreateDate = d.cus.create_date
                                     },
                                     BookingDetailsView = g
                                        .Where(x => x.bd != null)
                                        .Select(x => new ModalViews.BookingDetails.BookingDetailsView
                                        {
                                            CarId = x.bd.car_id,
                                            BookingDetailsStatus = x.bd.booking_details_status,
                                            BookingDate = x.bd.booking_date,
                                            StartDate = x.bd.start_date,
                                            EndDate = x.bd.end_date,
                                            ActualReturnDate = x.bd.actual_return_date,
                                            PricePerCar = x.bd != null ? x.bd.price_per_car : 0,
                                            Fine = x.bd.fine,
                                            Refund = x.bd.refund,
                                            StatusReturn = x.bd.status_return,
                                            CarView = new ModalViews.Car.CarView
                                            {
                                                CateId = x.car.cate_id,
                                                Brand = x.car != null ? x.car.brand : null,
                                                Model = x.car.model,
                                                PricePerDay = x.car.price_per_day ?? 0,
                                                CarStatus = x.car.car_status,
                                                Image = x.car.image,
                                                LicensePlate = x.car.license_plate,
                                                SeatCount = x.car.seat_count,
                                                Color = x.car.color,
                                                CarTypeId = x.car.car_type_id,
                                                CarTypeName = x.ctype.car_type_name,
                                                Active = x.car.active,
                                                CategoryName = x.cate.title
                                            }
                                        }).ToHashSet()
                                 };
                             }).FirstOrDefault();
                return items;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex);
            }
            return new BookingView();
        }
        public void Create(BookingView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = new tbl_Booking
                {
                    cus_id = entity.CusId,
                    booking_status = entity.BookingStatus,
                    discount = entity.Discount,
                    deposit = entity.Deposit,
                    payment_type_id = entity.PaymentTypeId,
                    order_date = entity.OrderDate,
                    paid = entity.Paid
                };
                en.tbl_Booking.Add(item);
                en.SaveChanges();
                Debug.WriteLine("Generated ID: " + item.booking_id); // should NOT be 0
                entity.ID = item.booking_id;
                
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public bool Update(BookingView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var itemUpdate = en.tbl_Booking.Where(d => d.booking_id == entity.ID).FirstOrDefault();
                if (itemUpdate != null)
                {
                    itemUpdate.discount = entity.Discount;
                    itemUpdate.deposit = entity.Deposit;
                    if(entity.DepositCash != null)
                    {
                        itemUpdate.deposit_cash = entity.DepositCash;
                    }
                    itemUpdate.payment_type_id = entity.PaymentTypeId;
                    itemUpdate.deposit_cash_has_paid = entity.DepositHasPaid;
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
        public bool Delete(BookingView entity)
        {
            return false;
        }
        public HashSet<BookingView> GetAll()
        {
            return new HashSet<BookingView>();
        }
        public HashSet<BookingView> GetAllPaging(int index =1, int pageSize = 10)
        {
            return new HashSet<BookingView>();
        }
        public HashSet<BookingView>FindAll(string filter = null)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rawItems = (from b in en.tbl_Booking
                                join cus in en.tbl_Customer on b.cus_id equals cus.cus_id into cusGroup
                                from cus in cusGroup.DefaultIfEmpty()
                                join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                                from dis in disGroup.DefaultIfEmpty()
                                join city in en.tbl_City on cus.city_id equals city.city_id into cityGroup
                                from city in cityGroup.DefaultIfEmpty()
                                join bd in en.tbl_Booking_details on b.booking_id equals bd.booking_id into bdGroup
                                from bd in bdGroup.DefaultIfEmpty()
                                join car in en.tbl_Car on bd.car_id equals car.car_id into carGroup
                                from car in carGroup.DefaultIfEmpty()
                                join ctype in en.tbl_Car_type on car.car_type_id equals ctype.car_type_id into carTypeGroup
                                from ctype in carTypeGroup.DefaultIfEmpty()
                                join cate in en.tbl_Category on car.cate_id equals cate.cate_id into cateGroup
                                from cate in cateGroup.DefaultIfEmpty()
                                join pt in en.tbl_Payment_type on b.payment_type_id equals pt.payment_type_id into ptGroup
                                from pt in ptGroup.DefaultIfEmpty()
                                where b.is_cancel != 1
                                select new
                                {
                                    b,
                                    cus,
                                    dis,
                                    city,
                                    bd,
                                    car,
                                    ctype,
                                    cate,
                                    pt
                                }).ToList();
                if (!string.IsNullOrEmpty(filter))
                {
                    rawItems = rawItems
                             .Where(x => x.cus != null && (
                                       (x.cus.cus_id_card != null && x.cus.cus_id_card.Contains(filter)) ||
                                       (x.cus.name != null && x.cus.name.Contains(filter)) ||
                                       (x.cus.phone != null && x.cus.phone.Contains(filter)))).ToList();
                }
                var items = rawItems
                             .GroupBy(d => d.b.booking_id)
                             .Select(g =>
                             {
                                 var d = g.First();
                                 return new BookingView
                                 {
                                     ID = d.b.booking_id,
                                     CusId = d.b.cus_id,
                                     BookingStatus = d.b.booking_status,
                                     Discount = d.b.discount,
                                     Deposit = d.b.deposit,
                                     PaymentTypeId = d.b.payment_type_id,
                                     PaymentTypeName = d.pt.name,
                                     TotalPrice = d.b.total_price,
                                     OrderDate = d.b.order_date,
                                     Paid = d.b.paid,
                                     CustomerView = new ModalViews.Customer.CustomerView
                                     {
                                         Name = d.cus != null ? d.cus.name : null,
                                         Email = d.cus.email,
                                         Password = d.cus.password,
                                         Phone = d.cus.phone,
                                         Address = d.cus.address,
                                         CityId = d.cus.city_id,
                                         CityName = d.city.city_name,
                                         DistrictId = d.cus.district_id,
                                         DistrictName = d.dis != null ? d.dis.district_name : null,
                                         Active = d.cus.active,
                                         CusIdCard = d.cus.cus_id_card,
                                         CreateDate = d.cus.create_date
                                     }
                                     //BookingDetailsView = new ModalViews.BookingDetails.BookingDetailsView
                                     //{
                                     //    CarId = d.bd.car_id,
                                     //    BookingDetailsStatus = d.bd.booking_details_status,
                                     //    BookingDate = d.bd.booking_date,
                                     //    StartDate = d.bd.start_date,
                                     //    EndDate = d.bd.end_date,
                                     //    ActualReturnDate = d.bd.actual_return_date,
                                     //    PricePerCar = d.bd != null ? d.bd.price_per_car : 0,
                                     //    Fine = d.bd.fine,
                                     //    Refund = d.bd.refund,
                                     //    StatusReturn = d.bd.status_return,
                                     //    CarView = new ModalViews.Car.CarView
                                     //    {
                                     //        CateId = d.car.cate_id,
                                     //        Brand = d.car != null ? d.car.brand : null,
                                     //        Model = d.car.model,
                                     //        PricePerDay = d.car.price_per_day ?? 0,
                                     //        CarStatus = d.car.car_status,
                                     //        Image = d.car.image,
                                     //        LicensePlate = d.car.license_plate,
                                     //        SeatCount = d.car.seat_count,
                                     //        Color = d.car.color,
                                     //        CarTypeId = d.car.car_type_id,
                                     //        CarTypeName = d.ctype.car_type_name,
                                     //        Active = d.car.active,
                                     //        CategoryName = d.cate.title
                                     //    }
                                     //}
                                 };
                             }).ToHashSet();
                return items;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<BookingView>();
        }
        public HashSet<BookingView> FindAllWaiting(string filter = null)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rawItems = (from b in en.tbl_Booking
                                join cus in en.tbl_Customer on b.cus_id equals cus.cus_id into cusGroup
                                from cus in cusGroup.DefaultIfEmpty()
                                join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                                from dis in disGroup.DefaultIfEmpty()
                                join city in en.tbl_City on cus.city_id equals city.city_id into cityGroup
                                from city in cityGroup.DefaultIfEmpty()
                                join bd in en.tbl_Booking_details on b.booking_id equals bd.booking_id into bdGroup
                                from bd in bdGroup.DefaultIfEmpty()
                                join car in en.tbl_Car on bd.car_id equals car.car_id into carGroup
                                from car in carGroup.DefaultIfEmpty()
                                join ctype in en.tbl_Car_type on car.car_type_id equals ctype.car_type_id into carTypeGroup
                                from ctype in carTypeGroup.DefaultIfEmpty()
                                join cate in en.tbl_Category on car.cate_id equals cate.cate_id into cateGroup
                                from cate in cateGroup.DefaultIfEmpty()
                                join pt in en.tbl_Payment_type on b.payment_type_id equals pt.payment_type_id into ptGroup
                                from pt in ptGroup.DefaultIfEmpty()
                                where b.is_cancel != 1 && b.booking_status == "Waiting"
                                select new
                                {
                                    b,
                                    cus,
                                    dis,
                                    city,
                                    bd,
                                    car,
                                    ctype,
                                    cate,
                                    pt
                                }).ToList();
                if (!string.IsNullOrEmpty(filter))
                {
                    rawItems = rawItems
                             .Where(x => x.cus != null && (
                                       (x.cus.cus_id_card != null && x.cus.cus_id_card.Contains(filter)) ||
                                       (x.cus.name != null && x.cus.name.Contains(filter)) ||
                                       (x.cus.phone != null && x.cus.phone.Contains(filter)))).ToList();
                }
                var items = rawItems
                             .GroupBy(d => d.b.booking_id)
                             .Select(g =>
                             {
                                 var d = g.First();
                                 return new BookingView
                                 {
                                     ID = d.b.booking_id,
                                     CusId = d.b.cus_id,
                                     BookingStatus = d.b.booking_status,
                                     Discount = d.b.discount,
                                     Deposit = d.b.deposit,
                                     PaymentTypeId = d.b.payment_type_id,
                                     PaymentTypeName = d.pt.name,
                                     TotalPrice = d.b.total_price,
                                     OrderDate = d.b.order_date,
                                     Paid = d.b.paid,
                                     CustomerView = new ModalViews.Customer.CustomerView
                                     {
                                         Name = d.cus != null ? d.cus.name : null,
                                         Email = d.cus.email,
                                         Password = d.cus.password,
                                         Phone = d.cus.phone,
                                         Address = d.cus.address,
                                         CityId = d.cus.city_id,
                                         CityName = d.city.city_name,
                                         DistrictId = d.cus.district_id,
                                         DistrictName = d.dis != null ? d.dis.district_name : null,
                                         Active = d.cus.active,
                                         CusIdCard = d.cus.cus_id_card,
                                         CreateDate = d.cus.create_date
                                     }
                                     //BookingDetailsView = new ModalViews.BookingDetails.BookingDetailsView
                                     //{
                                     //    CarId = d.bd.car_id,
                                     //    BookingDetailsStatus = d.bd.booking_details_status,
                                     //    BookingDate = d.bd.booking_date,
                                     //    StartDate = d.bd.start_date,
                                     //    EndDate = d.bd.end_date,
                                     //    ActualReturnDate = d.bd.actual_return_date,
                                     //    PricePerCar = d.bd != null ? d.bd.price_per_car : 0,
                                     //    Fine = d.bd.fine,
                                     //    Refund = d.bd.refund,
                                     //    StatusReturn = d.bd.status_return,
                                     //    CarView = new ModalViews.Car.CarView
                                     //    {
                                     //        CateId = d.car.cate_id,
                                     //        Brand = d.car != null ? d.car.brand : null,
                                     //        Model = d.car.model,
                                     //        PricePerDay = d.car.price_per_day ?? 0,
                                     //        CarStatus = d.car.car_status,
                                     //        Image = d.car.image,
                                     //        LicensePlate = d.car.license_plate,
                                     //        SeatCount = d.car.seat_count,
                                     //        Color = d.car.color,
                                     //        CarTypeId = d.car.car_type_id,
                                     //        CarTypeName = d.ctype.car_type_name,
                                     //        Active = d.car.active,
                                     //        CategoryName = d.cate.title
                                     //    }
                                     //}
                                 };
                             }).ToHashSet();
                return items;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<BookingView>();
        }
        public int CheckTotalCar(int booking_id)
        {
            try
            {
                var carNumber = 0;
                DbCarRental en = new DbCarRental();
                carNumber = (from bd in en.tbl_Booking_details
                             where bd.booking_id == booking_id
                             select bd).Count();

                return carNumber;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        public HashSet<BookingView> FindAllPaging(string filter, int index =1, int pageSize = 10)
        {
            return new HashSet<BookingView>();
        }
        public BookingView IsExistBooking(int? cus_id, string status)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from bk in en.tbl_Booking
                          where bk.cus_id == cus_id && bk.booking_status == status
                          select new BookingView
                          {
                              ID = bk.booking_id
                          }).FirstOrDefault();
                return rs;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public bool UpdateDepositCash(BookingView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Booking.Where(d => d.booking_id == entity.ID).FirstOrDefault();
                if (item != null)
                {
                    item.deposit_cash = entity.DepositCash;
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
        public bool UpdateConfirmComplete(BookingView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Booking.Where(d=>d.booking_id==entity.ID).FirstOrDefault();
                if (item != null)
                {
                    item.payment_confirm = entity.PaymentConfirm;
                    item.booking_status = entity.BookingStatus;
                    item.complete_date = entity.CompletedDate;
                    en.SaveChanges();
                    return true;
                }
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public bool CancelBooking(BookingView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Booking.Where(d => d.booking_id == entity.ID).FirstOrDefault();
                item.booking_status = entity.BookingStatus;
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
        public decimal? GetDailyRevenue(DateTime day)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var totalDailyRevenue = (from b in en.tbl_Booking
                                         where b.complete_date == day
                                         select b.total_price).Sum();
                return totalDailyRevenue ?? 0;
            }
            catch(EntityException ex)
            {
                Debug.Write(ex.Message);
            }
            return 0;
        }

        public decimal? GetMonthlyRevenue(int month)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var totalMonthlyRevenue = (from b in en.tbl_Booking
                                           where b.complete_date.Value.Month == month
                                           select b.total_price).Sum();
                return totalMonthlyRevenue ?? 0;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }

        public int CountDailyBooking(DateTime day)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var dailyBooking = (from b in en.tbl_Booking
                                    where b.order_date == day
                                    select b.booking_id).Count();
                return dailyBooking;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }
        public int CountMonthlyBookig(int month)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var monthlyBooking = (from b in en.tbl_Booking
                                      where b.order_date.Value.Month == month
                                      select b.booking_id).Count();
                return monthlyBooking;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }

        public int CountBookingCanceled()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var bookingCanceled = (from b in en.tbl_Booking
                                       where b.is_cancel == 1
                                       select b.booking_id).Count();
                return bookingCanceled;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }
        public int CountBookingProcessing()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var bookingProcessing = (from b in en.tbl_Booking
                                         where b.booking_status == "Waiting"
                                         select b.booking_id).Count();
                return bookingProcessing;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }
        public int CountBookingCompleted()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var bookingCompleted = (from b in en.tbl_Booking
                                        where b.booking_status == "Completed"
                                        select b.booking_id).Count();
                return bookingCompleted;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }
    }
}
