using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Domain.Interfaces;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    class TicketControllerTests
    {
        private Mock<ITicketService> _ticketService;

        [TestMethod]
        public void GetAsync_Return_All_Tickets()
        {

        }
    }
}
