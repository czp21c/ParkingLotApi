﻿using ParkingLotApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto()
        {
        }

        public ParkingLotDto(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
        }

        public ParkingLotDto(ParkingLotEntity parkinglotEntity)
        {
            Name = parkinglotEntity.Name;
            Capacity = parkinglotEntity.Capacity;
            Location = parkinglotEntity.Location;
            OrderDtos = parkinglotEntity.Orders?.Select(orderEntity => new OrderDto(orderEntity)).ToList();
        }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public List<OrderDto>? OrderDtos { get; set; }

        public ParkingLotEntity ToEntity()
        {
            return new ParkingLotEntity()
            {
                Name = this.Name,
                Capacity = this.Capacity,
                Location = this.Location,
                Orders = OrderDtos?.Select(orderDto => orderDto.ToEntity()).ToList(),
            };
        }
    }
}
