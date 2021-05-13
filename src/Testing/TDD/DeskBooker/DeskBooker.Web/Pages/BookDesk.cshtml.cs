using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeskBooker.Web.Pages
{
    public class BookDeskModel : PageModel
    {
        private IDeskBookingRequestProcessor _deskBookingRequestProcessor;

        [BindProperty]
        public DeskBookingRequest DeskBookingRequest { get; set; }

        public BookDeskModel(IDeskBookingRequestProcessor deskBookingRequestProcessor)
        {
            _deskBookingRequestProcessor = deskBookingRequestProcessor;
        }

        public void OnPost()
        {
            _deskBookingRequestProcessor.BookDesk(DeskBookingRequest);
        }
    }
}
