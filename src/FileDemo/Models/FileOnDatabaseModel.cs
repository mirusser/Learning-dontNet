﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileDemo.Models
{
    public class FileOnDatabaseModel : FileModel
    {
        public byte[] Data { get; set; }
    }
}
