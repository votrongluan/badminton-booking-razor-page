﻿using BusinessObjects.Entities;

namespace Repositories.IRepo;

public interface ICityRepository
{
    List<City> GetAllCities();
}