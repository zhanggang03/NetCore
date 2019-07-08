using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Model;
using ModelDTO;

namespace ViewModelMap
{
    public class DeptAutoMap : Profile
    {
        public DeptAutoMap()
        {
            CreateMap<Dept, DeptDTO>();
            CreateMap<User, UserDTO>();
            //CreateMap<DeptDTO, Dept>();
        }
    }
}
