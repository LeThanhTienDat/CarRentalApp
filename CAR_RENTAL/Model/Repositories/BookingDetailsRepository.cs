using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.BookingDetails;
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
    internal class BookingDetailsRepository:IRepository<BookingDetailsView>
    {
        private static BookingDetailsRepository _instance = null;
        public static BookingDetailsRepository Instance
        {
            get
            {
                if( _instance == null)
                {
                    _instance = new BookingDetailsRepository();
                }
                return _instance;
            }
        }

        public BookingDetailsView FindById(int id)
        {
            return new BookingDetailsView();
        }
        public HashSet<BookingDetailsView> FindByCusId(int? cus_id)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var items = (from bd in en.tbl_Booking_details
                             join car in en.tbl_Car on bd.car_id equals car.car_id into carGroup
                             from car in carGroup.DefaultIfEmpty()
                             join b in en.tbl_Booking on bd.booking_id equals b.booking_id into bGroup
                             from b in bGroup.DefaultIfEmpty()
                             where b.cus_id == cus_id && b.booking_status == "Waiting"
                             select new BookingDetailsView
                             {
                                 ID = bd.booking_details_id,
                                 LicensePlate = car.license_plate,
                                 Model = car.model,
                                 SeatCount = car.seat_count,
                                 StartDate = bd.start_date,
                                 EndDate = bd.end_date,
                                 ActualReturnDate = bd.actual_return_date,
                                 StatusReturn = bd.status_return,
                                 Total = bd.price_per_car,
                                 CarId = bd.car_id,
                                 CarView = new ModalViews.Car.CarView
                                 {

                                     PricePerDay = car.price_per_day ?? 0
                                 }
                             }).ToHashSet();
                return items;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex);
            }
            return new HashSet<BookingDetailsView>();
        }
        public HashSet<BookingDetailsView> FindByBookingId(int? booking_id)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var items = (from bd in en.tbl_Booking_details
                             join car in en.tbl_Car on bd.car_id equals car.car_id into carGroup
                             from car in carGroup.DefaultIfEmpty()
                             join b in en.tbl_Booking on bd.booking_id equals b.booking_id into bGroup
                             from b in bGroup.DefaultIfEmpty()
                             where b.booking_id == booking_id
                             select new BookingDetailsView
                             {
                                 ID = bd.booking_details_id,
                                 LicensePlate = car.license_plate,
                                 Model = car.model,
                                 SeatCount = car.seat_count,
                                 StartDate = bd.start_date,
                                 EndDate = bd.end_date,
                                 ActualReturnDate = bd.actual_return_date,
                                 StatusReturn = bd.status_return,
                                 Total = bd.price_per_car,
                                 CarId = bd.car_id,
                                 Fine = bd.fine ?? 0,
                                 Refund = bd.refund ?? 0,
                                 CarView = new ModalViews.Car.CarView
                                 {

                                     PricePerDay = car.price_per_day ?? 0
                                 }
                             }).ToHashSet();
                return items;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex);
            }
            return new HashSet<BookingDetailsView>();
        }

        public void Create(BookingDetailsView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = new tbl_Booking_details
                {
                    booking_id = entity.BookingId,
                    car_id = entity.CarId,
                    booking_details_status = entity.BookingDetailsStatus,
                    booking_date = entity.BookingDate,
                    start_date = entity.StartDate,
                    end_date = entity.EndDate,
                    price_per_car = entity.PricePerCar
                };
                en.tbl_Booking_details.Add(item);
                en.SaveChanges();
                entity.ID = item.booking_details_id;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public bool Update(BookingDetailsView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Booking_details.Where(d => d.booking_details_id == entity.ID).FirstOrDefault();
                item.start_date = entity.StartDate;
                item.end_date = entity.EndDate;
                item.price_per_car = entity.PricePerCar;
                item.status_return = entity.StatusReturn != null ? entity.StatusReturn : 0;
                item.actual_return_date = entity.ActualReturnDate;
                en.SaveChanges();
                return true;
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public bool Delete(BookingDetailsView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var itemDel = en.tbl_Booking_details.Where(d=>d.booking_details_id == entity.ID).FirstOrDefault();
                if(itemDel != null)
                {
                    en.tbl_Booking_details.Remove(itemDel);
                    en.SaveChanges();
                    return true;
                }
            }catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }

        public HashSet<BookingDetailsView> GetAll()
        {
            return new HashSet<BookingDetailsView>();
        }
        public HashSet<BookingDetailsView> GetAllPaging(int index =1, int pageSize = 10)
        {
            return new HashSet<BookingDetailsView>();
        }
        public HashSet<BookingDetailsView> FindAll(string filter)
        {
            return new HashSet<BookingDetailsView>();
        }
        public HashSet<BookingDetailsView>FindAllPaging(string filter, int index=1, int pageSize = 10)
        {
            return new HashSet<BookingDetailsView>();
        }
        public HashSet<BookingDetailsView> FindAllNotReturn(int booking_id)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from bd in en.tbl_Booking_details
                          where bd.booking_id == booking_id && bd.status_return != 1
                          select new BookingDetailsView
                          {
                              ID = bd.booking_id ?? 0
                          }).ToHashSet();
                return rs;
            }
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<BookingDetailsView>();
        }
    }
}
