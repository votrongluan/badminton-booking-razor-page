﻿using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class ServiceManager : IServiceManager
{
    private readonly IRepositoryManager _repoManager;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IClubService> _clubService;
    private readonly Lazy<ICityService> _cityService;
    private readonly Lazy<IDistrictService> _districtService;
    private readonly Lazy<IAccountService> _accountService;
    private readonly Lazy<IBookingService> _bookingService;
    private readonly Lazy<ICourtService> _courtService;
    private readonly Lazy<ICourtTypeService> _courtTypeService;
    private readonly Lazy<IMatchService> _matchService;
    private readonly Lazy<IReviewService> _reviewService;
    private readonly Lazy<ISlotService> _slotService;
    private readonly Lazy<IAvailableBookingTypeService> _availabelBookingService;
    private readonly Lazy<IBookingTypeService> _bookingTypeService;
    private readonly Lazy<IBookingDetailService> _bookingDetailService;

    public ServiceManager(IRepositoryManager repoManager)
    {
        _repoManager = repoManager;
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_repoManager));
        _clubService = new Lazy<IClubService>(() => new ClubService(_repoManager));
        _cityService = new Lazy<ICityService>(() => new CityService(repoManager));
        _districtService = new Lazy<IDistrictService>(() => new DistrictService(repoManager));
        _accountService = new Lazy<IAccountService>(() => new AccountService(repoManager));
        _bookingService = new Lazy<IBookingService>(() => new BookingService(repoManager));
        _courtService = new Lazy<ICourtService>(() => new CourtService(repoManager));
        _courtTypeService = new Lazy<ICourtTypeService>(() => new CourtTypeService(repoManager));
        _matchService = new Lazy<IMatchService>(() => new MatchService(repoManager));
        _reviewService = new Lazy<IReviewService>(() => new ReviewService(repoManager));
        _slotService = new Lazy<ISlotService>(() => new SlotService(repoManager));
        _availabelBookingService = new Lazy<IAvailableBookingTypeService>(() => new AvailableBookingTypeService(repoManager));
        _bookingTypeService = new Lazy<IBookingTypeService>(() => new BookingTypeService(repoManager));
        _bookingDetailService = new Lazy<IBookingDetailService>(() => new BookingDetailService(repoManager));
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;

    public IClubService ClubService => _clubService.Value;
    public ICityService CityService => _cityService.Value;
    public IDistrictService DistrictService => _districtService.Value;
    public IAccountService AccountService => _accountService.Value;
    public IBookingService BookingService => _bookingService.Value;
    public ICourtService CourtService => _courtService.Value;
    public ICourtTypeService CourtTypeService => _courtTypeService.Value;
    public IMatchService MatchService => _matchService.Value;
    public IReviewService ReviewService => _reviewService.Value;
    public ISlotService SlotService => _slotService.Value;
    public IAvailableBookingTypeService AvailableBookingTypeService => _availabelBookingService.Value;
    public IBookingTypeService BookingTypeService => _bookingTypeService.Value;

    public IBookingDetailService BookingDetailService => _bookingDetailService.Value;
}