﻿using BusinessObjects.Entities;
using Repositories.IRepo;
using Services.IService;

namespace Services.Service;

public class ClubService : IClubService
{
    private readonly IRepositoryManager _repo;

    public ClubService(IRepositoryManager repositoryManager)
    {
        _repo = repositoryManager;
    }

    public bool CheckPhoneExisted(string phone)
    {
        return _repo.Club.GetAllClubs().Any(e => e.ClubPhone.Equals(phone));
    }

    public List<Club> GetAllClubs()
    {
        return _repo.Club.GetAllClubs();
    }

    public void AddClub(Club club)
    {
        _repo.Club.AddClub(club);
    }

    public void DeleteClub(int clubId)
    {
        _repo.Club.DeleteClub(clubId);
    }

    public Club GetClubById(int clubId)
    {
        return _repo.Club.GetClubById(clubId);
    }
}