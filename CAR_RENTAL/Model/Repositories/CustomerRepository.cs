using CAR_RENTAL.Model.Entities;
using CAR_RENTAL.Model.ModalViews.Booking;
using CAR_RENTAL.Model.ModalViews.BookingDetails;
using CAR_RENTAL.Model.ModalViews.Car;
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
    internal class CustomerRepository : IRepository<CustomerView>
    {
        private static CustomerRepository _instance = null;
        public static CustomerRepository Instance
        {
            get
            {
                if (_instance == null)
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
            catch (EntityException ex)
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
                var item = en.tbl_Customer.Where(d => d.cus_id == entity.ID).FirstOrDefault();
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
                    item.cus_id_card = entity.CusIdCard;
                    en.SaveChanges();
                    return true;
                }
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }
        public bool Delete(CustomerView entity)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var item = en.tbl_Customer.Where(d => d.cus_id == entity.ID).FirstOrDefault();
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
            catch (EntityException ex)
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
                              CityName = ci != null ? ci.city_name : null,
                              DistrictName = dis != null ? dis.district_name : null,
                              Active = cus.active,
                              CusIdCard = cus.cus_id_card,
                              CreateDate = cus.create_date
                          }).ToHashSet();
                return rs;
            }
            catch (EntityException ex)
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
        public HashSet<CustomerView> FindAllPaging(string filter, int indec = 1, int pageSize = 10)
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
                if (item != null)
                {
                    return true;
                }
            }
            catch (EntityException ex)
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
            catch (EntityException ex)
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
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public CustomerView FindGuest(string phone, string idCard)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rawItems = (from cus in en.tbl_Customer
                                join b in en.tbl_Booking on cus.cus_id equals b.cus_id into bookingGroup
                                from b in bookingGroup.DefaultIfEmpty()
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
                                where cus.phone == phone && cus.cus_id_card == idCard
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
                var items = rawItems.Where(x => x.b != null).GroupBy(d => d.b.booking_id).ToList();
                if (items.Count == 0)
                {
                    return null;
                }
                else
                {
                    var d = items.First().First();
                    return new CustomerView
                    {
                        ID = d.cus.cus_id,
                        Name = d.cus != null ? d.cus.name : null,
                        Email = d.cus.email,
                        Password = d.cus.password,
                        Phone = d.cus.phone,
                        Address = d.cus.address,
                        CityId = d.cus.city_id,
                        CityName = d.city != null ? d.city.city_name : null,
                        DistrictId = d.cus.district_id,
                        DistrictName = d.dis != null ? d.dis.district_name : null,
                        Active = d.cus.active,
                        CusIdCard = d.cus.cus_id_card,
                        CreateDate = d.cus.create_date,
                        BookingViews = items.Select(g =>
                            {
                            var x = g.First();
                            return new BookingView
                            {
                                ID = x.b.booking_id,
                                CusId = x.b.cus_id,
                                BookingStatus = x.b.booking_status,
                                Discount = x.b.discount,
                                Deposit = x.b.deposit,
                                DepositCash = x.b.deposit_cash,
                                PaymentTypeId = x.b.payment_type_id,
                                PaymentTypeName = x.pt?.name,
                                TotalPrice = x.b.total_price,
                                OrderDate = x.b.order_date,
                                Paid = x.b.paid,
                                IsCancel = x.b.is_cancel ?? 0,
                                CancelDate = x.b.cancel_date,
                                ReasonCancel = x.b.reason_cancel,
                                CompletedDate = x.b.complete_date,
                                PaymentConfirm = x.b.payment_confirm == 1 ? 1 : 0,
                                BookingDetailsView = g
                                    .Where(m => m.bd != null)
                                    .Select(m => new BookingDetailsView
                                    {
                                        ID = m.bd.booking_details_id,
                                        BookingId = m.bd.booking_id,
                                        CarId = m.car?.car_id ?? 0,
                                        BookingDetailsStatus = m.bd.booking_details_status,
                                        BookingDate = m.bd.booking_date,
                                        StartDate = m.bd.start_date,
                                        EndDate = m.bd.end_date,
                                        ActualReturnDate = m.bd.actual_return_date,
                                        PricePerCar = m.bd.price_per_car ?? 0,
                                        Refund = m.bd.refund,
                                        Fine = m.bd.fine,
                                        StatusReturn = m.bd.status_return,
                                        CarView = new CarView
                                        {
                                            ID = m.car?.car_id ?? 0,
                                            CateId = m.car?.cate_id ?? 0,
                                            Brand = m.car?.brand,
                                            Model = m.car?.model,
                                            PricePerDay = m.car?.price_per_day ?? 0,
                                            CarStatus = m.car?.car_status,
                                            LicensePlate = m.car?.license_plate,
                                            Image = m.car?.image,
                                            SeatCount = m.car?.seat_count ?? 0,
                                            Color = m.car?.color,
                                            Active = m.car.active ?? 0,
                                            CarTypeId = m.car?.car_type_id ?? 0,
                                            CarTypeName = m.ctype?.car_type_name,
                                            CategoryName = m.cate?.title
                                        }
                                    }).ToHashSet()
                                };
                         }).ToHashSet()
                    };
                };
            }   
            catch(EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new CustomerView();
        }
        
        //Report
        public HashSet<CustomerView> FindCreateByDate(DateTime date)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from cus in en.tbl_Customer
                          join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          join ci in en.tbl_City on cus.city_id equals ci.city_id into ciGroup
                          from ci in ciGroup.DefaultIfEmpty()
                          where cus.create_date == date
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
                              CityName = ci != null ? ci.city_name : null,
                              DistrictName = dis != null ? dis.district_name : null,
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
        public HashSet<CustomerView> FindCreateByMonth(int month)
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from cus in en.tbl_Customer
                          join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          join ci in en.tbl_City on cus.city_id equals ci.city_id into ciGroup
                          from ci in ciGroup.DefaultIfEmpty()
                          where cus.create_date.Value.Month == month
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
                              CityName = ci != null ? ci.city_name : null,
                              DistrictName = dis != null ? dis.district_name : null,
                              Active = cus.active,
                              CusIdCard = cus.cus_id_card,
                              CreateDate = cus.create_date
                          }).ToHashSet();
                return rs;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CustomerView>();
        }
        public HashSet<CustomerView> FindTotalActive()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from cus in en.tbl_Customer
                          join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          join ci in en.tbl_City on cus.city_id equals ci.city_id into ciGroup
                          from ci in ciGroup.DefaultIfEmpty()
                          where cus.active == 1
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
                              CityName = ci != null ? ci.city_name : null,
                              DistrictName = dis != null ? dis.district_name : null,
                              Active = cus.active,
                              CusIdCard = cus.cus_id_card,
                              CreateDate = cus.create_date
                          }).ToHashSet();
                return rs;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CustomerView>();
        }
        public HashSet<CustomerView> FindTotalDeactive()
        {
            try
            {
                DbCarRental en = new DbCarRental();
                var rs = (from cus in en.tbl_Customer
                          join dis in en.tbl_District on cus.district_id equals dis.district_id into disGroup
                          from dis in disGroup.DefaultIfEmpty()
                          join ci in en.tbl_City on cus.city_id equals ci.city_id into ciGroup
                          from ci in ciGroup.DefaultIfEmpty()
                          where cus.active == null || cus.active == 0
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
                              CityName = ci != null ? ci.city_name : null,
                              DistrictName = dis != null ? dis.district_name : null,
                              Active = cus.active,
                              CusIdCard = cus.cus_id_card,
                              CreateDate = cus.create_date
                          }).ToHashSet();
                return rs;
            }
            catch (EntityException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new HashSet<CustomerView>();
        }
    }
}
