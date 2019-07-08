using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL;
using System.ComponentModel;
using ModelDTO;
using static Common.CommonResponse;
using Model;
using AutoMapper;
using Common;
using Newtonsoft.Json;

namespace MSCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDeptBll deptBll;
        private IUserBll userBll;

        public UserController(IDeptBll _deptBll, IUserBll _userBll)
        {
            deptBll = _deptBll;
            userBll = _userBll;
        }

        [HttpGet, Route("GetAllDeparments")]
        [Description("得到所有科室信息")]
        public ServiceResponseMessage<List<DeptDTO>> GetAllDeparments()
        {
            var response = new ServiceResponseMessage<List<DeptDTO>>();
            var depts = deptBll.GetAllDepts();
            response.Data = Mapper.Map<List<Dept>, List<DeptDTO>>(depts);
            Logger.Info(JsonConvert.SerializeObject(response));
            return response;
        }

        [HttpGet, Route("GetAllUsers")]
        [Description("得到所有人员信息")]
        public ServiceResponseMessage<List<UserDTO>> GetAllUsers()
        {
            var response = new ServiceResponseMessage<List<UserDTO>>();
            var users = userBll.GetAllUsers();
            response.Data = Mapper.Map<List<User>, List<UserDTO>>(users);
            return response;
        }

        [HttpGet, Route("GetUsersByDept/{deptCode}")]
        [Description("得到科室下面人员信息")]
        public ServiceResponseMessage<List<UserDTO>> GetUsersByDept(Guid deptCode)
        {
            var response = new ServiceResponseMessage<List<UserDTO>>();
            var users = userBll.GetUsersByDeptCode(deptCode);
            response.Data = Mapper.Map<List<User>, List<UserDTO>>(users);
            return response;
        }

        [HttpPost]
        [Description("增加科室")]
        public ServiceResponseMessage<DeptDTO> CreateDept(DeptDTO requestDto)
        {
            var response = new ServiceResponseMessage<DeptDTO>();
            Dept dept = Mapper.Map<DeptDTO, Dept>(requestDto);
            bool bl = deptBll.CreateDept(dept);
            if (bl)
            {
                response.Data = Mapper.Map<Dept, DeptDTO>(dept);
            }
            else
            {
                response.Success = false;
                response.Message = "插入数据失败";
            }
            return response;

        }
    }
}