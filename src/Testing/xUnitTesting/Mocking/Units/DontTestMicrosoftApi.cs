﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTesting.Mocking.Units
{
    public class DontTestMicrosoftApi
    {
        private readonly IFiles _files;

        public DontTestMicrosoftApi(IFiles files)
        {
            _files = files;
        }

        public Task SaveFile(string path, Stream file)
        {
            //more work
            var fileStream = _files.OpenWriteStreamTo(path);
            //more work
            return file.CopyToAsync(fileStream);
        }

        public interface IFiles
        {
            Stream OpenWriteStreamTo(string path);
        }
    }
}
