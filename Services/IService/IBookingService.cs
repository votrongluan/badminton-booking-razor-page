﻿using BusinessObjects.Dtos.Booking;
using BusinessObjects.Entities;

namespace Services.IService;

public interface IBookingService
{
    List<Booking> GetAllBookings();
    List<Booking> GetAllBookingsWithBookingDetails();
    void AddBooking(Booking booking);
    void DeleteBooking(int bookingId);
    void DeleteBookingDetail(int bookingId);
    Booking GetBookingById(int bookingId);
    Booking GetBookingByIdNoInclude(int bookingId);
    void UpdateBooking(Booking booking);
    (bool status, int bookId) BookLichThiDau(BookingRequestDto dto);
    (bool status, int bookId) BookLichNgay(BookingRequestDto dto);
    (bool status, int bookId) BookLichCoDinh(BookingRequestDto dto);

    (bool status, int bookId, int price) BookLichOffline(DateOnly date, TimeOnly startTime, TimeOnly endTime, int courtId,
        int clubId, int userId);
}