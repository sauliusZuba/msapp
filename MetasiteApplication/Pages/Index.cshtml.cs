using System;
using System.Collections.Generic;
using System.Linq;
using MetasiteApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MetasiteApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MetasiteDbContext _context;
        private const string SMS_MESSAGES = "SmsMessages";
        private const string CALLS = "Calls";
        private const string TOP_FIVE_SMS = "TopFiveSms";
        private const string TOP_FIVE_CALLS = "TopFiveCalls";
        private readonly string[] PartialNames = new string[] { SMS_MESSAGES, CALLS, TOP_FIVE_SMS, TOP_FIVE_CALLS };

        public IndexModel(MetasiteDbContext context)
        {
            _context = context;
            
        }

        public List<string> PagePartials { get; set; }

        public void OnGet()
        {
            PagePartials = new List<string>();
            PagePartials.AddRange(PartialNames.ToList());
        }

        public IActionResult OnGetSearch(string pagePartial, DateTime? dateFrom, DateTime? dateUpTo)
        {

            return new PartialViewResult
            {
                ViewName = "_GridPartial",
                ViewData = new ViewDataDictionary<GridViewModel>(ViewData, ToGridViewModel(dateFrom, dateUpTo, pagePartial)),
            };

        }

        private GridViewModel ToGridViewModel(DateTime? dateFrom, DateTime? dateUpTo, string pagePartial)
        {
            var result = new GridViewModel();
            result.Name = pagePartial;

            switch (pagePartial)
            {
                case SMS_MESSAGES:
                    result.Items.AddRange((from sms in _context.SmsMessages.Where(x => x.Date >= dateFrom && x.Date <= dateUpTo)
                                           from siteClient in _context.SiteClients.Where(x => x.SiteClientId == sms.SiteClientId)
                                           select new GridItemViewModel()
                                           {
                                               Date = sms.Date,
                                               Msisdn = siteClient.Msisdn
                                           }).OrderBy(x => x.Date).ToList());
                    result.ColumnVisibilityMap = new string[] { "Date" };
                    result.ShowCount = true;
                    break;

                case CALLS:
                    result.Items.AddRange((from call in _context.Calls.Where(x => x.Date >= dateFrom && x.Date <= dateUpTo)
                                           from siteClient in _context.SiteClients.Where(x => x.SiteClientId == call.SiteClientId)
                                           select new GridItemViewModel()
                                           {
                                               Date = call.Date,
                                               Msisdn = siteClient.Msisdn,
                                               Duration = call.Duration
                                           }).OrderBy(x => x.Date).ToList());
                    result.ColumnVisibilityMap = new string[] { "Duration", "Date" };
                    result.ShowCount = true;
                    break;

                case TOP_FIVE_SMS:
                    result.Items.AddRange((from sms in _context.SmsMessages.Where(x => x.Date >= dateFrom && x.Date <= dateUpTo)
                                           group sms by sms.SiteClientId into groupSms
                                           from ss in _context.SiteClients.Where(x => x.SiteClientId == groupSms.Key)
                                           select new GridItemViewModel()
                                           {
                                               Count = groupSms.Count(),
                                               Msisdn = ss.Msisdn
                                           }).OrderByDescending(x => x.Count).Take(5).ToList());
                    result.ColumnVisibilityMap = new string[] { "Count" };
                    result.ShowCount = false;
                    break;

                case TOP_FIVE_CALLS:
                    result.Items.AddRange((from call in _context.Calls.Where(x => x.Date >= dateFrom && x.Date <= dateUpTo)
                                           group call by call.SiteClientId into groupCall
                                           from ss in _context.SiteClients.Where(x => x.SiteClientId == groupCall.Key)
                                           select new GridItemViewModel()
                                           {
                                               Duration = groupCall.Sum(x => x.Duration),
                                               Msisdn = ss.Msisdn
                                           }).OrderByDescending(x => x.Duration).Take(5).ToList());
                    result.ColumnVisibilityMap = new string[] { "Duration" };
                    result.ShowCount = false;
                    break;

                default:
                    break;
            }

            return result;
        }

    }
}
