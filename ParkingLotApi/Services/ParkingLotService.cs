﻿using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Model;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotDbContext parkinglotDbContext;

        public ParkingLotService(ParkingLotDbContext parkinglotDbContext)
        {
            this.parkinglotDbContext = parkinglotDbContext;
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public async Task<List<ParkingLotDto>> GetAll()
        {
            var parkinglots = parkinglotDbContext.Parkinglots.Include(_ => _.Orders).ToList();
            return parkinglots.Select(parkinglotEntity => new ParkingLotDto(parkinglotEntity)).ToList();
        }

        public async Task<ParkingLotDto> GetById(long id)
        {
            var foundParkinglot = await parkinglotDbContext.Parkinglots.Include(_ => _.Orders).FirstOrDefaultAsync(parkinglot => parkinglot.Id == id);

            return new ParkingLotDto(foundParkinglot);
        }

        public async Task<OrderDto> AddOrderById(long id, OrderDto orderDto)
        {
            var foundParkinglot = await parkinglotDbContext.Parkinglots.Include(_ => _.Orders).FirstOrDefaultAsync(parkinglot => parkinglot.Id == id);
            var orderEntity = orderDto.ToEntity();
            foundParkinglot.Orders.Add(orderEntity);
            await parkinglotDbContext.SaveChangesAsync();
            return new OrderDto(orderEntity);
        }

        public async Task<OrderDto> UpdateById(long id, int orderid)
        {
            var foundParkinglot = await parkinglotDbContext.Parkinglots.Include(_ => _.Orders).FirstOrDefaultAsync(parkinglot => parkinglot.Id == id);
            var foundOrder = foundParkinglot.Orders.FirstOrDefault(order => order.Id == orderid);
            foundOrder.CloseTime = GetTimestamp(DateTime.Now);
            foundOrder.Status = "close";
            await parkinglotDbContext.SaveChangesAsync();
            return new OrderDto(foundOrder);
        }

        public async Task<int> AddParkinglot(ParkingLotDto parkinglotDto)
        {
            //convert dto to entity
            ParkingLotEntity parkinglotEntity = parkinglotDto.ToEntity();
            //save entity to db
            await parkinglotDbContext.Parkinglots.AddAsync(parkinglotEntity);
            await parkinglotDbContext.SaveChangesAsync();

            return parkinglotEntity.Id;
        }

        public async Task DeleteParkingLot(int id)
        {
            var foundParkinglot = await parkinglotDbContext.Parkinglots.Include(_ => _.Orders).FirstOrDefaultAsync(parkinglot => parkinglot.Id == id);
            parkinglotDbContext.Parkinglots.Remove(foundParkinglot);
            await parkinglotDbContext.SaveChangesAsync();
        }
    }
}
