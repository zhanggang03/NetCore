using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace ViewModelMap
{
    public class RegisterMappings
    {
        public static void Initialize()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DeptAutoMap>();
            }
            );
        }
    }
}
