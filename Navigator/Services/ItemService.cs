﻿using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Enums;
using DataLayer;
using Services.Interfaces;
using Services.Models;

namespace Services
{
    public class ItemService : IItemService
    {
        private readonly ICacheService _cacheService;
        private readonly ITypeItemService _typeItemService;
        private readonly INodeService _nodeService;
        private readonly ILevelService _levelService;

        public ItemService(ICacheService cacheService, ITypeItemService typeItemService, INodeService nodeService, ILevelService levelService)
        {
            _typeItemService = typeItemService;
            _nodeService = nodeService;
            _levelService = levelService;
            _cacheService = cacheService;
        }

        public ItemSm Get(Guid id)
        {
            ItemSm FirstGetFunc()
            {
                using (var db = new NavigatorContext())
                {
                    var item = db.Items.Find(id);
                    if (item == null)
                    {
                        return null;
                    }

                    return new ItemSm()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Number = item.Number,
                        Repair = item.Repair,
                        Level = _levelService.Get(item.LevelId),
                        TypeItem = _typeItemService.Get(item.TypeItemId),
                        Nodes = _nodeService.GetLine(item.NodeId),
                    };
                }
            }

            return _cacheService.Get(id, FirstGetFunc);
        }

        public IList<ItemSm> Get(LevelSm level, ItemType? type = null)
        {
            using (var db = new NavigatorContext())
            {
                var query = db.Items.Where(x => x.LevelId == level.Id);
                if (type.HasValue)
                {
                    query = query.Where(x => x.TypeItem.Code == (int)type);
                }

                return query
                    .AsEnumerable()
                    .Select(x => Get(x.Id))
                    .ToList();
            }
        }

        public Guid Get(string number)
        {
            using (var db = new NavigatorContext())
            {
                var id = db.Items.Where(x => x.Number.ToLower() == number.ToLower().Trim())
                    .Select(x => x.Id)
                    .FirstOrDefault();

                return id;
            }
        }
        // Todo: выводить только уникальные значения объектов
        public IList<ItemSm> GetAll()
        {
            using (var db = new NavigatorContext())
            {
                if (db.Items.Any())
                {
                    return db.Items
                        .AsEnumerable()
                        .Select(x => Get(x.Id))
                        .Distinct()
                        .ToList();
                }
                return null;
            }
        }
    }
}
