﻿using BOs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public CarController()
        {
            _context = new OldCarShowroomNetworkContext();
        }
        [HttpGet]
        [Route("carName/{carId}")]
        public ActionResult<List<CarName>> listCarNames(int carId) {


            return _context.CarNames.Where(c => c.ManufactoryId == carId).ToList();

        }
        /* public ActionResult<Pet> Create(Pet pet)
         {
             pet.Id = _petsInMemoryStore.Any() ?
                      _petsInMemoryStore.Max(p => p.Id) + 1 : 1;
             _petsInMemoryStore.Add(pet);

             return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
         }*/
        [HttpGet]
        [Route("soldCount")]
        public ActionResult<int> GetSoldCarCount()
        {
            int soldCount = _context.Cars.Count(c => c.Notification == 3);
            return Ok(soldCount);
        }
        [HttpGet]
        [Route("totalSales")]
        public ActionResult<long> GetTotalCarSales()
        {
            long totalSales = (long)_context.Cars.Sum(c => (c.Price * 0.1) ?? 0);
            return Ok(totalSales);
        }
        [HttpGet]
        [Route("revenueByMonth")]
        public ActionResult<List<RevenueByMonth>> GetRevenueByMonth()
        {
            // Lấy danh sách các tháng từ cơ sở dữ liệu
            var months = Enumerable.Range(1, 12).ToList();

            // Khởi tạo danh sách để lưu doanh thu của từng tháng
            List<RevenueByMonth> revenueByMonths = new List<RevenueByMonth>();

            // Lặp qua từng tháng và tính doanh thu
            for (int i = 0; i < months.Count; i++)
            {
                int month = months[i];
                long totalRevenue = (long)_context.Cars
                    .Where(c => c.CreatedAt.Value.Month == month)
                    .Sum(c => (c.Price * 0.1)  ?? 0) ; // Tính tổng doanh thu của các xe bán trong tháng

                RevenueByMonth revenue = new RevenueByMonth
                {
                    Month = month,
                    TotalRevenue = totalRevenue
                };

                revenueByMonths.Add(revenue);
            }

            return Ok(revenueByMonths);
        }



    }
}
