﻿using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepo;

namespace Repositories.Repo;

public class ReviewRepository : IReviewRepository
{
    public List<Review> GetAllReviews ()
    {
        return ReviewDao.GetAll().OrderByDescending(e => e.ReviewId).ToList();
    }

    public Review GetReviewById (int reviewId)
    {
        return GetAllReviews().FirstOrDefault(e => e.ReviewId == reviewId);
    }

    public void DeleteReview (int reviewId)
    {
        var review = GetReviewById(reviewId);
        ReviewDao.Delete(review);
    }

    public void UpdateReview (Review review)
    {
        ReviewDao.Update(review);
    }

    public void AddReview (Review review)
    {
        ReviewDao.Add(review);
    }

    public List<Review> GetAllByClubId (int clubId)
    {
        return ReviewDao.GetAll().Include(e => e.User).Where(e => e.ClubId == clubId).OrderByDescending(e => e.ReviewId).ToList();
    }
}