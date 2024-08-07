﻿using BusinessObjects.Dtos.Match;
using BusinessObjects.Entities;

namespace WebAppRazor.Mappers;

public static class MatchMapper
{
    public static MatchResponseDto ToMatchResponseDto(this Match match)
    {
        var bookingDetail = match.Booking?.BookingDetails.ElementAtOrDefault(0);
        var matchDateStringArray = match.Booking?.BookingDetails.Select(e => e.BookDate?.ToString("dd/MM/yyyy") ?? "").ToArray();
        var matchDate = string.Join(" , ", matchDateStringArray);
        var formatStartTime = bookingDetail.StartTime.Value.Hour.ToString("D2") + ":" + match.Booking.BookingDetails.ElementAt(0).StartTime.Value.Minute.ToString("D2");
        var formatEndTime = bookingDetail.EndTime.Value.Hour.ToString("D2") + ":" + match.Booking.BookingDetails.ElementAt(0).EndTime.Value.Minute.ToString("D2");

        return new MatchResponseDto()
        {
            ClubId = match.Booking.ClubId,
            MatchId = match.MatchId,
            CourtId = match.Booking.BookingDetails.ElementAt(0).CourtId,
            Description = match.Description,
            ClubName = match.Booking?.Club?.ClubName,
            Address = $"{match.Booking?.Club?.Address}, {match.Booking?.Club?.District.DistrictName}, {match.Booking?.Club?.District.City.CityName}",
            MatchDate = matchDate,
            MatchTime = $"{formatStartTime} - {formatEndTime}",
            Title = match.Title,
            MatchDateOnly = bookingDetail.BookDate,
        };
    }
}