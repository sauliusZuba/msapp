using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MetasiteApplication.Data
{
    public class MetasiteDbContext : DbContext
    {
        public MetasiteDbContext(DbContextOptions<MetasiteDbContext> options) : base(options)
        {
        }
        public DbSet<SiteClient> SiteClients { get; set; }
        public DbSet<Sms> SmsMessages { get; set; }
        public DbSet<Call> Calls { get; set; }
    }

    public class SiteClient
    {
        public int SiteClientId { get; set; }
        public string Msisdn { get; set; }

        public ICollection<Sms> SmsMessages { get; set; }
        public ICollection<Call> Calls { get; set; }
    }

    public class Sms
    {
        public int SmsId { get; set; }
        public DateTime Date { get; set; }

        public int SiteClientId { get; set; }
        public SiteClient SiteClient { get; set; }
    }

    public class Call
    {
        public int CallId { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }

        public int SiteClientId { get; set; }
        public SiteClient SiteClient { get; set; }
    }
}
