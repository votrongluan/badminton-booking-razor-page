﻿using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Repositories.IRepo;

namespace Repositories.Repo;

public class SlotRepository : ISlotRepository
{
    public void AddSlot(Slot slot) => SlotDao.Add(slot);

    public void DeleteSlot(int id)
    {
        var slot = GetSlotById(id);
        SlotDao.Delete(slot);
    }

    public List<Slot> GetAllByClubId(int id)
    {
        return SlotDao.FindByCondition(e => e.ClubId == id).OrderBy(e => e.StartTime).ToList();
    }

    public List<Slot> GetAllSlot()
    {
        return SlotDao.GetAll().OrderBy(e => e.StartTime).ToList();
    }

    public Slot GetSlotById(int id)
    {
        return SlotDao.FindByCondition(e => e.SlotId == id).FirstOrDefault();
    }

    public void UpdateSlot(Slot slot)
    {
        SlotDao.Update(slot);
    }
}