using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetasiteTaskApplication.Models
{
    public class DataSeeder
    {
        public static void SeedData(MetasiteDbContext context)
        {
            if (!context.SiteClients.Any())
            {
                var generator = new RandomGenerator();

                BuilderSetup.DisablePropertyNamingFor<SiteClient, int>(x => x.SiteClientId);
                var siteClients = Builder<SiteClient>.CreateListOfSize(100)
                    .All()
                    .With(c => c.Msisdn = (37060000000 + generator.Next(1, 9999999)).ToString())
                    .Build();
                context.SiteClients.AddRange(siteClients.ToArray());
                context.SaveChanges();

                siteClients = context.SiteClients.ToArray();

                BuilderSetup.DisablePropertyNamingFor<Sms, int>(x => x.SmsId);
                var smss = Builder<Sms>.CreateListOfSize(500)
                    .All()
                    .With(x => x.SiteClient = RandomItemFromSiteClients(siteClients))
                    .With(x => x.Date = RandomDate(generator))
                    .Build();
                context.SmsMessages.AddRange(smss.ToArray());
                context.SaveChanges();

                BuilderSetup.DisablePropertyNamingFor<Call, int>(x => x.CallId);
                var calls = Builder<Call>.CreateListOfSize(500)
                    .All()
                    .With(x => x.SiteClient = RandomItemFromSiteClients(siteClients))
                    .With(x => x.Date = RandomDate(generator))
                    .With(x => x.Duration = generator.Next(5, 1200))
                    .Build();
                context.Calls.AddRange(calls.ToArray());
                context.SaveChanges();
            }
        }

        private static SiteClient RandomItemFromSiteClients(IList<SiteClient> siteClients)
        {
            return Pick<SiteClient>.RandomItemFrom(siteClients);
        }

        private static DateTime RandomDate(RandomGenerator generator)
        {
            var start = new DateTime(2018, 1, 1);
            var range = (DateTime.Now - start).TotalSeconds;
            return start.AddSeconds(generator.Next(0, range));
        }
    }
}
