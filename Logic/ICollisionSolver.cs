﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface ICollisionSolver : IDisposable
    {
        public void Update();
        public AabbTree CollisionTree { get; }
    }
}
