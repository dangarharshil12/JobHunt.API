﻿using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IProfileRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetUsersAsync();
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(User user);
    }
}
