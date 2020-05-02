﻿using System;
using System.Collections.Generic;
using Services.Models;

namespace Services.Interfaces
{
    public interface IBuildingService
    {
        BuildingSm Get(Guid id);

        IEnumerable<BuildingSm> GetAll();
    }
}
