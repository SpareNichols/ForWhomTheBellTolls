using BellScheduleManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BellScheduleManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleRule> ScheduleRules { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
