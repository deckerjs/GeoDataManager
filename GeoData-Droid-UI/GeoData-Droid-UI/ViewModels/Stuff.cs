using sensortest.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace sensortest.ViewModels
{
    public class Stuff
    {
        private readonly ISensorValuesRepository _repository;

        public Stuff(ISensorValuesRepository repository)
        {
            Console.WriteLine("bla");
            _repository = repository;
        }
    }
}
